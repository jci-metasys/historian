USE [DataExtractor]
GO

/****** Object:  Table [dbo].[FqrGuid]    Script Date: 8/6/2018 1:52:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FqrGuid](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Fqr] [nvarchar](400) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[PointName] [nvarchar](400) NULL,
	[PointType] [nvarchar](400) NULL,
 CONSTRAINT [PK_FqrGuid] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  StoredProcedure [dbo].[InsertFqrGuid]    Script Date: 8/28/2018 3:10:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:          JCI
-- Create date: 7/20/2018
-- Description:     Insert the FQR with its specific GUID
-- =============================================
CREATE PROCEDURE [dbo].[InsertFqrGuid] 
       @Fqr nvarchar(400),
       @Guid uniqueidentifier,
       @PointName nvarchar(400),
	   @PointType nvarchar(400)
AS
BEGIN
       INSERT INTO FqrGuid (Fqr, Guid, PointName, PointType)
       Values(@Fqr, @Guid, @PointName, @PointType)
END


/****** Object:  StoredProcedure [dbo].[GetAllFqrGuids]    Script Date: 8/6/2018 1:52:55 PM ******/
SET ANSI_NULLS ON
GO


/****** Object:  StoredProcedure [dbo].[GetAllFqrGuids]    Script Date: 8/6/2018 1:52:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:          JCI
-- Create date: 8/1/2018
-- Description:     Get all FQRs with their GUIDs
-- =============================================
CREATE PROCEDURE [dbo].[GetAllFqrGuids]
       
AS
BEGIN
       SELECT Fqr, Guid, PointName, PointType
       FROM FqrGuid
END

/****** Object:  StoredProcedure [dbo].[DeleteAllFqrGuid]    Script Date: 8/6/2018 1:53:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:          JCI
-- Create date: 7/20/2018
-- Description:     Removes all entries from the FqrGuid table
-- =============================================
CREATE PROCEDURE [dbo].[DeleteAllFqrGuid] 
AS
BEGIN
       DELETE FROM FqrGuid
END
