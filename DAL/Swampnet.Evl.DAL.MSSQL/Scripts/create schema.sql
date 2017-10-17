USE [dbmain]
GO
/****** Object:  Table [dbo].[ApiKey]    Script Date: 17/10/2017 20:48:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApiKey](
	[Id] [uniqueidentifier] NOT NULL,
	[CreatedOnUtc] [datetime2](7) NOT NULL,
	[OrganisationId] [uniqueidentifier] NULL,
	[RevokedOnUtc] [datetime2](7) NULL,
 CONSTRAINT [PK_ApiKey] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Event]    Script Date: 17/10/2017 20:48:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Event](
	[Id] [uniqueidentifier] NOT NULL,
	[Category] [nvarchar](2000) NOT NULL,
	[LastUpdatedUtc] [datetime2](7) NOT NULL,
	[Source] [nvarchar](2000) NOT NULL,
	[SourceVersion] [nvarchar](max) NULL,
	[Summary] [nvarchar](2000) NOT NULL,
	[TimestampUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventProperties]    Script Date: 17/10/2017 20:48:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventProperties](
	[EventId] [uniqueidentifier] NOT NULL,
	[InternalPropertyId] [bigint] NOT NULL,
 CONSTRAINT [PK_EventProperties] PRIMARY KEY CLUSTERED 
(
	[EventId] ASC,
	[InternalPropertyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventTags]    Script Date: 17/10/2017 20:48:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventTags](
	[EventId] [uniqueidentifier] NOT NULL,
	[InternalTagId] [bigint] NOT NULL,
 CONSTRAINT [PK_EventTags] PRIMARY KEY CLUSTERED 
(
	[EventId] ASC,
	[InternalTagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Organisation]    Script Date: 17/10/2017 20:48:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organisation](
	[Id] [uniqueidentifier] NOT NULL,
	[ApiKey] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Organisation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Property]    Script Date: 17/10/2017 20:48:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Property](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Category] [nvarchar](2000) NULL,
	[Name] [nvarchar](2000) NOT NULL,
	[Value] [nvarchar](2000) NOT NULL,
 CONSTRAINT [PK_Property] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rule]    Script Date: 17/10/2017 20:48:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rule](
	[Id] [uniqueidentifier] NOT NULL,
	[ActionData] [nvarchar](max) NOT NULL,
	[ExpressionData] [nvarchar](max) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Rule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tag]    Script Date: 17/10/2017 20:48:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tag](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ApiKey]  WITH CHECK ADD  CONSTRAINT [FK_ApiKey_Organisation_OrganisationId] FOREIGN KEY([OrganisationId])
REFERENCES [dbo].[Organisation] ([Id])
GO
ALTER TABLE [dbo].[ApiKey] CHECK CONSTRAINT [FK_ApiKey_Organisation_OrganisationId]
GO
ALTER TABLE [dbo].[EventProperties]  WITH CHECK ADD  CONSTRAINT [FK_EventProperties_Event_EventId] FOREIGN KEY([EventId])
REFERENCES [dbo].[Event] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EventProperties] CHECK CONSTRAINT [FK_EventProperties_Event_EventId]
GO
ALTER TABLE [dbo].[EventProperties]  WITH CHECK ADD  CONSTRAINT [FK_EventProperties_Property_InternalPropertyId] FOREIGN KEY([InternalPropertyId])
REFERENCES [dbo].[Property] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EventProperties] CHECK CONSTRAINT [FK_EventProperties_Property_InternalPropertyId]
GO
ALTER TABLE [dbo].[EventTags]  WITH CHECK ADD  CONSTRAINT [FK_EventTags_Event_EventId] FOREIGN KEY([EventId])
REFERENCES [dbo].[Event] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EventTags] CHECK CONSTRAINT [FK_EventTags_Event_EventId]
GO
ALTER TABLE [dbo].[EventTags]  WITH CHECK ADD  CONSTRAINT [FK_EventTags_Tag_InternalTagId] FOREIGN KEY([InternalTagId])
REFERENCES [dbo].[Tag] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EventTags] CHECK CONSTRAINT [FK_EventTags_Tag_InternalTagId]
GO
