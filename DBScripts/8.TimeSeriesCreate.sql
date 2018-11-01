USE [DataExtractor]
GO
/****** Object:  Table [dbo].[TimeSeriesHistorical]    Script Date: 8/30/2018 9:27:01 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TimeSeriesHistorical](
	[Id] [uniqueidentifier] NOT NULL,
	[PointName] [nvarchar](100) NOT NULL,
	[PointGuid] [uniqueidentifier] NOT NULL,
	[PointType] [nvarchar](300) NOT NULL,
	[ItemReference] [nvarchar](300) NOT NULL,
	[IsReliable] [bit] NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[Units] [nvarchar](100) NOT NULL,
	[Value] [nvarchar](100) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  UserDefinedTableType [dbo].[TimeSeriesHistoricalListUDT]    Script Date: 9/6/2018 3:44:26 PM ******/
CREATE TYPE [dbo].[TimeSeriesHistoricalListUDT] AS TABLE(
	[PointName] [nvarchar](100) NULL,
	[PointGuid] [uniqueidentifier] NOT NULL,
	[PointType] [nvarchar](300) NULL,
	[ItemReference] [nvarchar](300) NULL,
	[IsReliable] [bit] NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[Units] [nvarchar](100) NOT NULL,
	[Value] [nvarchar](100) NOT NULL
)
GO


/****** Object:  StoredProcedure [dbo].[InsertTimeSeriesData]    Script Date: 8/30/2018 9:26:37 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		JCI
-- Create date: 7/9/2018
-- Description:	Insert the Time Series data into the table
-- =============================================
CREATE PROCEDURE [dbo].[InsertTimeSeriesData]
	@SampleListUDT AS TimeSeriesHistoricalListUDT READONLY
AS
BEGIN
       INSERT INTO TimeSeriesHistorical(
		[Id]
		,[PointName]
		,[PointGuid]
		,[PointType]
		,[ItemReference]
		,[IsReliable]
		,[TimeStamp]
		,[Units]
		,[Value])
       SELECT NEWID(), 
			p.PointName, 
			s.PointGuid, 
			p.PointType,
			p.Fqr, 
			s.IsReliable,
			s.TimeStamp,
			s.Units,
			s.Value
       FROM @SampleListUDT s
       LEFT JOIN (SELECT tsh.PointGuid, tsh.ItemReference, tsh.TimeStamp
                       FROM TimeSeriesHistorical tsh) tsh ON tsh.PointGuid = s.PointGuid AND tsh.ItemReference = s.ItemReference AND tsh.TimeStamp = s.TimeStamp
	   LEFT JOIN FqrGuid p ON p.guid = s.PointGuid
       WHERE tsh.PointGuid IS NULL AND tsh.ItemReference IS NULL

END



GO
