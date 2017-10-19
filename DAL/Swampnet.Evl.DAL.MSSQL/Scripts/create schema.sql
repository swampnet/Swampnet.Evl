/****** Object:  Schema [evl]    Script Date: 19/10/2017 09:55:04 ******/
CREATE SCHEMA [evl]
GO
/****** Object:  Table [evl].[ApiKey]    Script Date: 19/10/2017 09:55:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [evl].[ApiKey](
	[Id] [uniqueidentifier] NOT NULL,
	[CreatedOnUtc] [datetime2](7) NOT NULL,
	[OrganisationId] [uniqueidentifier] NOT NULL,
	[RevokedOnUtc] [datetime2](7) NULL,
 CONSTRAINT [PK_ApiKey] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [evl].[Event]    Script Date: 19/10/2017 09:55:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [evl].[Event](
	[Id] [uniqueidentifier] NOT NULL,
	[Category] [nvarchar](2000) NOT NULL,
	[LastUpdatedUtc] [datetime2](7) NOT NULL,
	[OrganisationId] [uniqueidentifier] NOT NULL,
	[Source] [nvarchar](2000) NOT NULL,
	[SourceVersion] [nvarchar](max) NULL,
	[Summary] [nvarchar](max) NOT NULL,
	[TimestampUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [evl].[EventProperties]    Script Date: 19/10/2017 09:55:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [evl].[EventProperties](
	[EventId] [uniqueidentifier] NOT NULL,
	[InternalPropertyId] [bigint] NOT NULL,
 CONSTRAINT [PK_EventProperties] PRIMARY KEY CLUSTERED 
(
	[EventId] ASC,
	[InternalPropertyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [evl].[EventTags]    Script Date: 19/10/2017 09:55:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [evl].[EventTags](
	[EventId] [uniqueidentifier] NOT NULL,
	[InternalTagId] [bigint] NOT NULL,
 CONSTRAINT [PK_EventTags] PRIMARY KEY CLUSTERED 
(
	[EventId] ASC,
	[InternalTagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [evl].[Organisation]    Script Date: 19/10/2017 09:55:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [evl].[Organisation](
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
/****** Object:  Table [evl].[Property]    Script Date: 19/10/2017 09:55:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [evl].[Property](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Category] [nvarchar](225) NULL,
	[Name] [nvarchar](225) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Property] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [evl].[Rule]    Script Date: 19/10/2017 09:55:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [evl].[Rule](
	[Id] [uniqueidentifier] NOT NULL,
	[ActionData] [xml] NOT NULL,
	[ExpressionData] [xml] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[OrganisationId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Rule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [evl].[Tag]    Script Date: 19/10/2017 09:55:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [evl].[Tag](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_ApiKey_OrganisationId]    Script Date: 19/10/2017 09:55:05 ******/
CREATE NONCLUSTERED INDEX [IX_ApiKey_OrganisationId] ON [evl].[ApiKey]
(
	[OrganisationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Event_OrganisationId]    Script Date: 19/10/2017 09:55:05 ******/
CREATE NONCLUSTERED INDEX [IX_Event_OrganisationId] ON [evl].[Event]
(
	[OrganisationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_EventProperties_InternalPropertyId]    Script Date: 19/10/2017 09:55:05 ******/
CREATE NONCLUSTERED INDEX [IX_EventProperties_InternalPropertyId] ON [evl].[EventProperties]
(
	[InternalPropertyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_EventTags_InternalTagId]    Script Date: 19/10/2017 09:55:05 ******/
CREATE NONCLUSTERED INDEX [IX_EventTags_InternalTagId] ON [evl].[EventTags]
(
	[InternalTagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Property_Category_Name]    Script Date: 19/10/2017 09:55:05 ******/
CREATE NONCLUSTERED INDEX [IX_Property_Category_Name] ON [evl].[Property]
(
	[Category] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Property_Name]    Script Date: 19/10/2017 09:55:05 ******/
CREATE NONCLUSTERED INDEX [IX_Property_Name] ON [evl].[Property]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Rule_OrganisationId]    Script Date: 19/10/2017 09:55:05 ******/
CREATE NONCLUSTERED INDEX [IX_Rule_OrganisationId] ON [evl].[Rule]
(
	[OrganisationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Tag_Name]    Script Date: 19/10/2017 09:55:05 ******/
CREATE NONCLUSTERED INDEX [IX_Tag_Name] ON [evl].[Tag]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [evl].[ApiKey]  WITH CHECK ADD  CONSTRAINT [FK_ApiKey_Organisation_OrganisationId] FOREIGN KEY([OrganisationId])
REFERENCES [evl].[Organisation] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [evl].[ApiKey] CHECK CONSTRAINT [FK_ApiKey_Organisation_OrganisationId]
GO
ALTER TABLE [evl].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_Organisation_OrganisationId] FOREIGN KEY([OrganisationId])
REFERENCES [evl].[Organisation] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [evl].[Event] CHECK CONSTRAINT [FK_Event_Organisation_OrganisationId]
GO
ALTER TABLE [evl].[EventProperties]  WITH CHECK ADD  CONSTRAINT [FK_EventProperties_Event_EventId] FOREIGN KEY([EventId])
REFERENCES [evl].[Event] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [evl].[EventProperties] CHECK CONSTRAINT [FK_EventProperties_Event_EventId]
GO
ALTER TABLE [evl].[EventProperties]  WITH CHECK ADD  CONSTRAINT [FK_EventProperties_Property_InternalPropertyId] FOREIGN KEY([InternalPropertyId])
REFERENCES [evl].[Property] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [evl].[EventProperties] CHECK CONSTRAINT [FK_EventProperties_Property_InternalPropertyId]
GO
ALTER TABLE [evl].[EventTags]  WITH CHECK ADD  CONSTRAINT [FK_EventTags_Event_EventId] FOREIGN KEY([EventId])
REFERENCES [evl].[Event] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [evl].[EventTags] CHECK CONSTRAINT [FK_EventTags_Event_EventId]
GO
ALTER TABLE [evl].[EventTags]  WITH CHECK ADD  CONSTRAINT [FK_EventTags_Tag_InternalTagId] FOREIGN KEY([InternalTagId])
REFERENCES [evl].[Tag] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [evl].[EventTags] CHECK CONSTRAINT [FK_EventTags_Tag_InternalTagId]
GO
ALTER TABLE [evl].[Rule]  WITH CHECK ADD  CONSTRAINT [FK_Rule_Organisation_OrganisationId] FOREIGN KEY([OrganisationId])
REFERENCES [evl].[Organisation] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [evl].[Rule] CHECK CONSTRAINT [FK_Rule_Organisation_OrganisationId]
GO
