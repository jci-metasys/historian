USE [DataExtractor]
GO

/****** Object:  Table [dbo].[Annotations]    Script Date: 7/25/2018 11:03:57 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Alarms](
	[Id] [uniqueidentifier] NOT NULL,
	[ItemReference] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Message] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[IsAckRequired] [bit] NOT NULL,
	[Type] [nvarchar](50) NULL,
	[Priority] [int] NOT NULL,
	[TriggerValue] [nvarchar](50) NULL,
	[TriggerValueUnits] [nvarchar](50) NULL,
	[TriggerValueHref] [nvarchar](max) NULL,
	[CreationTime] [datetime] NOT NULL,
	[IsAcknowledged] [bit] NOT NULL,
	[IsDiscarded] [bit] NOT NULL,
	[Category] [nvarchar](50) NULL,
 CONSTRAINT [PK_Alarms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE TABLE [dbo].[Annotations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [uniqueidentifier] NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
	[User] [nvarchar](50) NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[Action] [varchar](20) NOT NULL,
CONSTRAINT [PK_Annotations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


