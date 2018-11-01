USE [DataExtractor]
GO
/****** Object:  UserDefinedTableType [dbo].[EnumListUDT]    Script Date: 7/17/2018 12:53:22 PM ******/
CREATE TYPE [dbo].[EnumListUDT] AS TABLE(
	[SetId] [int] NULL,
	[MemberId] [int] NULL,
	[SetDesc] [nvarchar](300) NULL,
	[MemberDesc] [nvarchar](300) NULL
)
GO

/****** Object:  StoredProcedure [dbo].[GetEnumList]    Script Date: 7/17/2018 12:53:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE TABLE [dbo].[EnumList](
	[SetId] [int] NOT NULL,
	[MemberId] [int] NOT NULL,
	[SetDesc] [nvarchar](300) NOT NULL,
	[MemberDesc] [nvarchar](300) NULL,
 CONSTRAINT [pk_EnumListCompSetIdMemberId] PRIMARY KEY CLUSTERED 
(
	[SetId] ASC,
	[MemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- =============================================
-- Author:		JCI
-- Create date: 7/9/2018
-- Description:	Return the entire EnumList
-- =============================================
CREATE PROCEDURE [dbo].[GetEnumList]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT enum.SetId, enum.MemberId, enum.SetDesc, enum.MemberDesc
	FROM EnumList enum

	return
	
END
GO


-- =============================================
-- Author:		JCI
-- Create date: 7/9/2018
-- Description:	Insert the EnumList, checks for duplicates
-- =============================================
CREATE PROCEDURE [dbo].[InsertEnumList]
	@EnumListUDT AS EnumListUDT READONLY
AS
BEGIN
	INSERT INTO EnumList(
		[SetId]
		,[MemberId]
		,[SetDesc]
		,[MemberDesc]
	)
	SELECT el.SetId, el.MemberId, el.SetDesc, el.MemberDesc
	FROM @EnumListUDT el
	LEFT JOIN (SELECT elt.SetId, elt.MemberId
			   FROM EnumList elt) elt ON elt.SetId = el.SetId AND elt.MemberId = el.MemberId
	WHERE elt.SetId IS NULL
	
END

GO



