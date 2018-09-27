USE [DataExtractor]
GO

/****** Object:  StoredProcedure [dbo].[InsertAnnotation]    Script Date: 7/24/2018 4:37:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[InsertAnnotation]
	@parentId uniqueidentifier,
	@text nvarchar(max),
	@user nvarchar(50),
	@creationTime datetime,
	@action nvarchar(50)
AS
BEGIN
	INSERT INTO Annotations(
		[ParentId]
		,[Text]
		,[User]
		,[CreationTime]
		,[Action]
	)
	VALUES(
		@parentId,
		@text,
		@user,
		@creationTime,
		@action
	)
END

GO


