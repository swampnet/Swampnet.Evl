/****** Object:  Schema [evl]    Script Date: 03/11/2017 15:27:03 ******/
CREATE SCHEMA [evl]
GO
USE [db-main]
GO
/****** Object:  Table [evl].[Action]    Script Date: 12/05/2018 22:43:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[evl].[Action]') AND type in (N'U'))
BEGIN
CREATE TABLE [evl].[Action](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Error] [nvarchar](max) NULL,
	[TimestampUtc] [datetime2](7) NOT NULL,
	[TriggerId] [bigint] NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Action] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
ALTER AUTHORIZATION ON [evl].[Action] TO  SCHEMA OWNER 
GO
/****** Object:  Table [evl].[ActionProperties]    Script Date: 12/05/2018 22:43:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[evl].[ActionProperties]') AND type in (N'U'))
BEGIN
CREATE TABLE [evl].[ActionProperties](
	[ActionId] [bigint] NOT NULL,
	[PropertyId] [bigint] NOT NULL,
 CONSTRAINT [PK_ActionProperties] PRIMARY KEY CLUSTERED 
(
	[ActionId] ASC,
	[PropertyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
ALTER AUTHORIZATION ON [evl].[ActionProperties] TO  SCHEMA OWNER 
GO
/****** Object:  Table [evl].[ApiKey]    Script Date: 12/05/2018 22:43:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[evl].[ApiKey]') AND type in (N'U'))
BEGIN
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
END
GO
ALTER AUTHORIZATION ON [evl].[ApiKey] TO  SCHEMA OWNER 
GO
/****** Object:  Table [evl].[Event]    Script Date: 12/05/2018 22:43:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[evl].[Event]') AND type in (N'U'))
BEGIN
CREATE TABLE [evl].[Event](
	[Id] [uniqueidentifier] NOT NULL,
	[Category] [nvarchar](2000) NOT NULL,
	[ModifiedOnUtc] [datetime2](7) NOT NULL,
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
END
GO
ALTER AUTHORIZATION ON [evl].[Event] TO  SCHEMA OWNER 
GO
/****** Object:  Table [evl].[EventProperties]    Script Date: 12/05/2018 22:43:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[evl].[EventProperties]') AND type in (N'U'))
BEGIN
CREATE TABLE [evl].[EventProperties](
	[EventId] [uniqueidentifier] NOT NULL,
	[PropertyId] [bigint] NOT NULL,
 CONSTRAINT [PK_EventProperties] PRIMARY KEY CLUSTERED 
(
	[EventId] ASC,
	[PropertyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
ALTER AUTHORIZATION ON [evl].[EventProperties] TO  SCHEMA OWNER 
GO
/****** Object:  Table [evl].[EventTags]    Script Date: 12/05/2018 22:43:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[evl].[EventTags]') AND type in (N'U'))
BEGIN
CREATE TABLE [evl].[EventTags](
	[EventId] [uniqueidentifier] NOT NULL,
	[TagId] [bigint] NOT NULL,
 CONSTRAINT [PK_EventTags] PRIMARY KEY CLUSTERED 
(
	[EventId] ASC,
	[TagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
ALTER AUTHORIZATION ON [evl].[EventTags] TO  SCHEMA OWNER 
GO
/****** Object:  Table [evl].[Organisation]    Script Date: 12/05/2018 22:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[evl].[Organisation]') AND type in (N'U'))
BEGIN
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
END
GO
ALTER AUTHORIZATION ON [evl].[Organisation] TO  SCHEMA OWNER 
GO
/****** Object:  Table [evl].[Property]    Script Date: 12/05/2018 22:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[evl].[Property]') AND type in (N'U'))
BEGIN
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
END
GO
ALTER AUTHORIZATION ON [evl].[Property] TO  SCHEMA OWNER 
GO
/****** Object:  Table [evl].[Rule]    Script Date: 12/05/2018 22:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[evl].[Rule]') AND type in (N'U'))
BEGIN
CREATE TABLE [evl].[Rule](
	[Id] [uniqueidentifier] NOT NULL,
	[ActionData] [xml] NOT NULL,
	[ExpressionData] [xml] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Order] [int] NOT NULL,
	[OrganisationId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Rule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
ALTER AUTHORIZATION ON [evl].[Rule] TO  SCHEMA OWNER 
GO
/****** Object:  Table [evl].[Tag]    Script Date: 12/05/2018 22:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[evl].[Tag]') AND type in (N'U'))
BEGIN
CREATE TABLE [evl].[Tag](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[OrganisationId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
ALTER AUTHORIZATION ON [evl].[Tag] TO  SCHEMA OWNER 
GO
/****** Object:  Table [evl].[Trigger]    Script Date: 12/05/2018 22:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[evl].[Trigger]') AND type in (N'U'))
BEGIN
CREATE TABLE [evl].[Trigger](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[EventId] [uniqueidentifier] NOT NULL,
	[RuleId] [uniqueidentifier] NOT NULL,
	[RuleName] [nvarchar](max) NOT NULL,
	[TimestampUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Trigger] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
ALTER AUTHORIZATION ON [evl].[Trigger] TO  SCHEMA OWNER 
GO
/****** Object:  Index [IX_Action_TriggerId]    Script Date: 12/05/2018 22:43:01 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[evl].[Action]') AND name = N'IX_Action_TriggerId')
CREATE NONCLUSTERED INDEX [IX_Action_TriggerId] ON [evl].[Action]
(
	[TriggerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ActionProperties_PropertyId]    Script Date: 12/05/2018 22:43:01 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[evl].[ActionProperties]') AND name = N'IX_ActionProperties_PropertyId')
CREATE NONCLUSTERED INDEX [IX_ActionProperties_PropertyId] ON [evl].[ActionProperties]
(
	[PropertyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ApiKey_OrganisationId]    Script Date: 12/05/2018 22:43:01 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[evl].[ApiKey]') AND name = N'IX_ApiKey_OrganisationId')
CREATE NONCLUSTERED INDEX [IX_ApiKey_OrganisationId] ON [evl].[ApiKey]
(
	[OrganisationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Event_OrganisationId]    Script Date: 12/05/2018 22:43:01 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[evl].[Event]') AND name = N'IX_Event_OrganisationId')
CREATE NONCLUSTERED INDEX [IX_Event_OrganisationId] ON [evl].[Event]
(
	[OrganisationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_EventProperties_PropertyId]    Script Date: 12/05/2018 22:43:01 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[evl].[EventProperties]') AND name = N'IX_EventProperties_PropertyId')
CREATE NONCLUSTERED INDEX [IX_EventProperties_PropertyId] ON [evl].[EventProperties]
(
	[PropertyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_EventTags_TagId]    Script Date: 12/05/2018 22:43:01 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[evl].[EventTags]') AND name = N'IX_EventTags_TagId')
CREATE NONCLUSTERED INDEX [IX_EventTags_TagId] ON [evl].[EventTags]
(
	[TagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Property_Category_Name]    Script Date: 12/05/2018 22:43:01 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[evl].[Property]') AND name = N'IX_Property_Category_Name')
CREATE NONCLUSTERED INDEX [IX_Property_Category_Name] ON [evl].[Property]
(
	[Category] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Property_Name]    Script Date: 12/05/2018 22:43:01 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[evl].[Property]') AND name = N'IX_Property_Name')
CREATE NONCLUSTERED INDEX [IX_Property_Name] ON [evl].[Property]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Rule_OrganisationId]    Script Date: 12/05/2018 22:43:01 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[evl].[Rule]') AND name = N'IX_Rule_OrganisationId')
CREATE NONCLUSTERED INDEX [IX_Rule_OrganisationId] ON [evl].[Rule]
(
	[OrganisationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Tag_Name]    Script Date: 12/05/2018 22:43:01 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[evl].[Tag]') AND name = N'IX_Tag_Name')
CREATE NONCLUSTERED INDEX [IX_Tag_Name] ON [evl].[Tag]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Tag_OrganisationId]    Script Date: 12/05/2018 22:43:01 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[evl].[Tag]') AND name = N'IX_Tag_OrganisationId')
CREATE NONCLUSTERED INDEX [IX_Tag_OrganisationId] ON [evl].[Tag]
(
	[OrganisationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Trigger_EventId]    Script Date: 12/05/2018 22:43:01 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[evl].[Trigger]') AND name = N'IX_Trigger_EventId')
CREATE NONCLUSTERED INDEX [IX_Trigger_EventId] ON [evl].[Trigger]
(
	[EventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_Action_Trigger_TriggerId]') AND parent_object_id = OBJECT_ID(N'[evl].[Action]'))
ALTER TABLE [evl].[Action]  WITH CHECK ADD  CONSTRAINT [FK_Action_Trigger_TriggerId] FOREIGN KEY([TriggerId])
REFERENCES [evl].[Trigger] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_Action_Trigger_TriggerId]') AND parent_object_id = OBJECT_ID(N'[evl].[Action]'))
ALTER TABLE [evl].[Action] CHECK CONSTRAINT [FK_Action_Trigger_TriggerId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_ActionProperties_Action_ActionId]') AND parent_object_id = OBJECT_ID(N'[evl].[ActionProperties]'))
ALTER TABLE [evl].[ActionProperties]  WITH CHECK ADD  CONSTRAINT [FK_ActionProperties_Action_ActionId] FOREIGN KEY([ActionId])
REFERENCES [evl].[Action] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_ActionProperties_Action_ActionId]') AND parent_object_id = OBJECT_ID(N'[evl].[ActionProperties]'))
ALTER TABLE [evl].[ActionProperties] CHECK CONSTRAINT [FK_ActionProperties_Action_ActionId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_ActionProperties_Property_PropertyId]') AND parent_object_id = OBJECT_ID(N'[evl].[ActionProperties]'))
ALTER TABLE [evl].[ActionProperties]  WITH CHECK ADD  CONSTRAINT [FK_ActionProperties_Property_PropertyId] FOREIGN KEY([PropertyId])
REFERENCES [evl].[Property] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_ActionProperties_Property_PropertyId]') AND parent_object_id = OBJECT_ID(N'[evl].[ActionProperties]'))
ALTER TABLE [evl].[ActionProperties] CHECK CONSTRAINT [FK_ActionProperties_Property_PropertyId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_ApiKey_Organisation_OrganisationId]') AND parent_object_id = OBJECT_ID(N'[evl].[ApiKey]'))
ALTER TABLE [evl].[ApiKey]  WITH CHECK ADD  CONSTRAINT [FK_ApiKey_Organisation_OrganisationId] FOREIGN KEY([OrganisationId])
REFERENCES [evl].[Organisation] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_ApiKey_Organisation_OrganisationId]') AND parent_object_id = OBJECT_ID(N'[evl].[ApiKey]'))
ALTER TABLE [evl].[ApiKey] CHECK CONSTRAINT [FK_ApiKey_Organisation_OrganisationId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_Event_Organisation_OrganisationId]') AND parent_object_id = OBJECT_ID(N'[evl].[Event]'))
ALTER TABLE [evl].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_Organisation_OrganisationId] FOREIGN KEY([OrganisationId])
REFERENCES [evl].[Organisation] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_Event_Organisation_OrganisationId]') AND parent_object_id = OBJECT_ID(N'[evl].[Event]'))
ALTER TABLE [evl].[Event] CHECK CONSTRAINT [FK_Event_Organisation_OrganisationId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_EventProperties_Event_EventId]') AND parent_object_id = OBJECT_ID(N'[evl].[EventProperties]'))
ALTER TABLE [evl].[EventProperties]  WITH CHECK ADD  CONSTRAINT [FK_EventProperties_Event_EventId] FOREIGN KEY([EventId])
REFERENCES [evl].[Event] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_EventProperties_Event_EventId]') AND parent_object_id = OBJECT_ID(N'[evl].[EventProperties]'))
ALTER TABLE [evl].[EventProperties] CHECK CONSTRAINT [FK_EventProperties_Event_EventId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_EventProperties_Property_PropertyId]') AND parent_object_id = OBJECT_ID(N'[evl].[EventProperties]'))
ALTER TABLE [evl].[EventProperties]  WITH CHECK ADD  CONSTRAINT [FK_EventProperties_Property_PropertyId] FOREIGN KEY([PropertyId])
REFERENCES [evl].[Property] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_EventProperties_Property_PropertyId]') AND parent_object_id = OBJECT_ID(N'[evl].[EventProperties]'))
ALTER TABLE [evl].[EventProperties] CHECK CONSTRAINT [FK_EventProperties_Property_PropertyId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_EventTags_Event_EventId]') AND parent_object_id = OBJECT_ID(N'[evl].[EventTags]'))
ALTER TABLE [evl].[EventTags]  WITH CHECK ADD  CONSTRAINT [FK_EventTags_Event_EventId] FOREIGN KEY([EventId])
REFERENCES [evl].[Event] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_EventTags_Event_EventId]') AND parent_object_id = OBJECT_ID(N'[evl].[EventTags]'))
ALTER TABLE [evl].[EventTags] CHECK CONSTRAINT [FK_EventTags_Event_EventId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_EventTags_Tag_TagId]') AND parent_object_id = OBJECT_ID(N'[evl].[EventTags]'))
ALTER TABLE [evl].[EventTags]  WITH CHECK ADD  CONSTRAINT [FK_EventTags_Tag_TagId] FOREIGN KEY([TagId])
REFERENCES [evl].[Tag] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_EventTags_Tag_TagId]') AND parent_object_id = OBJECT_ID(N'[evl].[EventTags]'))
ALTER TABLE [evl].[EventTags] CHECK CONSTRAINT [FK_EventTags_Tag_TagId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_Rule_Organisation_OrganisationId]') AND parent_object_id = OBJECT_ID(N'[evl].[Rule]'))
ALTER TABLE [evl].[Rule]  WITH CHECK ADD  CONSTRAINT [FK_Rule_Organisation_OrganisationId] FOREIGN KEY([OrganisationId])
REFERENCES [evl].[Organisation] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_Rule_Organisation_OrganisationId]') AND parent_object_id = OBJECT_ID(N'[evl].[Rule]'))
ALTER TABLE [evl].[Rule] CHECK CONSTRAINT [FK_Rule_Organisation_OrganisationId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_Tag_Organisation_OrganisationId]') AND parent_object_id = OBJECT_ID(N'[evl].[Tag]'))
ALTER TABLE [evl].[Tag]  WITH CHECK ADD  CONSTRAINT [FK_Tag_Organisation_OrganisationId] FOREIGN KEY([OrganisationId])
REFERENCES [evl].[Organisation] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_Tag_Organisation_OrganisationId]') AND parent_object_id = OBJECT_ID(N'[evl].[Tag]'))
ALTER TABLE [evl].[Tag] CHECK CONSTRAINT [FK_Tag_Organisation_OrganisationId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_Trigger_Event_EventId]') AND parent_object_id = OBJECT_ID(N'[evl].[Trigger]'))
ALTER TABLE [evl].[Trigger]  WITH CHECK ADD  CONSTRAINT [FK_Trigger_Event_EventId] FOREIGN KEY([EventId])
REFERENCES [evl].[Event] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[evl].[FK_Trigger_Event_EventId]') AND parent_object_id = OBJECT_ID(N'[evl].[Trigger]'))
ALTER TABLE [evl].[Trigger] CHECK CONSTRAINT [FK_Trigger_Event_EventId]
GO
