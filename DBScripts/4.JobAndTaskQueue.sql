USE [DataExtractor]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--Create Job table
CREATE TABLE [dbo].[Job](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[Status] [tinyint] NOT NULL,
 CONSTRAINT [PK_Job] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Job] ADD  CONSTRAINT [DF_Job_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO

ALTER TABLE [dbo].[Job] ADD  CONSTRAINT [DF_Job_Status]  DEFAULT ((0)) FOR [Status]
GO

-- Create TaskQueue table
CREATE TABLE [dbo].[TaskQueue](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[JobId] [int] NOT NULL,
	[TaskType] [tinyint] NOT NULL,
	[TaskUrl] [varchar](max) NOT NULL,
	[IsCompleted] [bit] NOT NULL,
 CONSTRAINT [PK_TaskQueue] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[TaskQueue] ADD  CONSTRAINT [DF_TaskQueue_IsCompleted]  DEFAULT ((0)) FOR [IsCompleted]
GO

ALTER TABLE [dbo].[TaskQueue]  WITH CHECK ADD  CONSTRAINT [FK_TaskQueue_Job] FOREIGN KEY([JobId])
REFERENCES [dbo].[Job] ([Id])
GO

ALTER TABLE [dbo].[TaskQueue] CHECK CONSTRAINT [FK_TaskQueue_Job]
GO

--Create InsertJob SP
CREATE PROCEDURE [dbo].[InsertJob]
@startTime datetime,
@endTime datetime,
@dateCreated datetime
AS
BEGIN
	DECLARE @newJobId int

	INSERT INTO Job (DateCreated, StartTime, EndTime)
	VALUES(@dateCreated, @startTime, @endTime)

	SET @newJobId = @@IDENTITY

	SELECT @newJobId
END
GO

-- Create GetJobsByStatus
CREATE PROCEDURE [dbo].[GetJobsByStatus]
@status tinyint

AS
BEGIN
	SELECT Id, DateCreated, StartTime, EndTime, [Status]
	FROM Job
	WHERE [Status] = @status
END

GO

CREATE PROCEDURE [dbo].[GetUnsuccessfulJobs]

AS
BEGIN
	SELECT Id, DateCreated, StartTime, EndTime, [Status]
	FROM Job
	WHERE [Status] <> 2 -- executing
	AND [Status] <> 3 -- success 
END

GO

-- Create GetLatest Job
CREATE PROCEDURE [dbo].[GetLatestJob]

AS
BEGIN
	SELECT TOP (1) [Id]
      ,[DateCreated]
      ,[StartTime]
      ,[EndTime]
      ,[Status]
	  FROM [DataExtractor].[dbo].[Job] 
	  ORDER BY EndTime DESC
END

GO

-- Create UpdateJobStatus
CREATE PROCEDURE [dbo].[UpdateJobStatus]
@jobId int,
@status tinyint,
@deleteTasks bit
AS
BEGIN
	UPDATE Job
	SET [Status] = @status
	WHERE Id = @jobId

	IF @deleteTasks = 1 -- the job is successful, time to delete all the tasks in this job
	BEGIN
		DELETE FROM TaskQueue
		WHERE jobId = @jobId
	END
END

GO

-- InsertTask SP
CREATE PROCEDURE [dbo].[InsertTask]
	@jobId int, 
	@taskType tinyint,
	@taskUrl varchar(max)
AS
BEGIN
	DECLARE @newTaskId int

	INSERT INTO TaskQueue (JobId, TaskType, TaskUrl)
	VALUES(@jobId, @taskType, @taskUrl)

	SET @newTaskId = @@IDENTITY

	SELECT @newTaskId
END

GO

-- GetTasksByJobId SP
CREATE PROCEDURE [dbo].[GetTasksByJobId]
	@jobId int
AS
BEGIN

	SELECT Id, JobId, TaskType, TaskUrl, IsCompleted FROM TaskQueue
	WHERE JobId = @jobId

END

GO

-- Create UpdateTaskToComplete
CREATE PROCEDURE [dbo].[UpdateTaskToComplete]
	@taskId int
AS
BEGIN
	UPDATE TaskQueue
	SET IsCompleted = 1
	WHERE Id = @taskId
END

GO

--Create DeleteTasksByJob
CREATE PROCEDURE [dbo].[DeleteTasksByJob]
	@jobId int
AS
BEGIN
	DELETE FROM TaskQueue
	WHERE JobId = @jobId
END

GO