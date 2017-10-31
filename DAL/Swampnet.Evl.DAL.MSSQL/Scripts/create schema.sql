/****** Object:  Schema [evl]    Script Date: 31/10/2017 10:23:46 ******/
CREATE SCHEMA [evl]
GO
/****** Object:  Table [evl].[Action]    Script Date: 31/10/2017 10:23:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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

GO
/****** Object:  Table [evl].[ActionProperties]    Script Date: 31/10/2017 10:23:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [evl].[ActionProperties](
	[ActionId] [bigint] NOT NULL,
	[PropertyId] [bigint] NOT NULL,
 CONSTRAINT [PK_ActionProperties] PRIMARY KEY CLUSTERED 
(
	[ActionId] ASC,
	[PropertyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [evl].[ApiKey]    Script Date: 31/10/2017 10:23:46 ******/
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
/****** Object:  Table [evl].[Event]    Script Date: 31/10/2017 10:23:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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

GO
/****** Object:  Table [evl].[EventProperties]    Script Date: 31/10/2017 10:23:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [evl].[EventProperties](
	[EventId] [uniqueidentifier] NOT NULL,
	[PropertyId] [bigint] NOT NULL,
 CONSTRAINT [PK_EventProperties] PRIMARY KEY CLUSTERED 
(
	[EventId] ASC,
	[PropertyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [evl].[EventTags]    Script Date: 31/10/2017 10:23:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [evl].[EventTags](
	[EventId] [uniqueidentifier] NOT NULL,
	[TagId] [bigint] NOT NULL,
 CONSTRAINT [PK_EventTags] PRIMARY KEY CLUSTERED 
(
	[EventId] ASC,
	[TagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [evl].[Organisation]    Script Date: 31/10/2017 10:23:46 ******/
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
/****** Object:  Table [evl].[Permission]    Script Date: 31/10/2017 10:23:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [evl].[Permission](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IsEnabled] [bit] NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [evl].[Profile]    Script Date: 31/10/2017 10:23:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [evl].[Profile](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Firstname] [nvarchar](max) NULL,
	[OrganisationId] [uniqueidentifier] NOT NULL,
	[Key] [nvarchar](256) NOT NULL,
	[KnownAs] [nvarchar](max) NULL,
	[Lastname] [nvarchar](max) NULL,
	[Title] [nvarchar](max) NULL,
 CONSTRAINT [PK_Profile] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [evl].[ProfileRoles]    Script Date: 31/10/2017 10:23:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [evl].[ProfileRoles](
	[ProfileId] [bigint] NOT NULL,
	[RoleId] [bigint] NOT NULL,
 CONSTRAINT [PK_ProfileRoles] PRIMARY KEY CLUSTERED 
(
	[ProfileId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [evl].[Property]    Script Date: 31/10/2017 10:23:46 ******/
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
/****** Object:  Table [evl].[Role]    Script Date: 31/10/2017 10:23:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [evl].[Role](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [evl].[RolePermissions]    Script Date: 31/10/2017 10:23:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [evl].[RolePermissions](
	[PermissionId] [bigint] NOT NULL,
	[RoleId] [bigint] NOT NULL,
 CONSTRAINT [PK_RolePermissions] PRIMARY KEY CLUSTERED 
(
	[PermissionId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [evl].[Rule]    Script Date: 31/10/2017 10:23:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [evl].[Rule](
	[Id] [uniqueidentifier] NOT NULL,
	[ActionData] [xml] NOT NULL,
	[CreatedOnUtc] [datetime2](7) NOT NULL,
	[ExpressionData] [xml] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[ModifiedOnUtc] [datetime2](7) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Order] [int] NOT NULL,
	[OrganisationId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Rule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [evl].[Tag]    Script Date: 31/10/2017 10:23:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [evl].[Tag](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[OrganisationId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [evl].[Trigger]    Script Date: 31/10/2017 10:23:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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

GO
/****** Object:  Index [IX_Action_TriggerId]    Script Date: 31/10/2017 10:23:46 ******/
CREATE NONCLUSTERED INDEX [IX_Action_TriggerId] ON [evl].[Action]
(
	[TriggerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ActionProperties_PropertyId]    Script Date: 31/10/2017 10:23:46 ******/
CREATE NONCLUSTERED INDEX [IX_ActionProperties_PropertyId] ON [evl].[ActionProperties]
(
	[PropertyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ApiKey_OrganisationId]    Script Date: 31/10/2017 10:23:46 ******/
CREATE NONCLUSTERED INDEX [IX_ApiKey_OrganisationId] ON [evl].[ApiKey]
(
	[OrganisationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Event_OrganisationId]    Script Date: 31/10/2017 10:23:46 ******/
CREATE NONCLUSTERED INDEX [IX_Event_OrganisationId] ON [evl].[Event]
(
	[OrganisationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_EventProperties_PropertyId]    Script Date: 31/10/2017 10:23:46 ******/
CREATE NONCLUSTERED INDEX [IX_EventProperties_PropertyId] ON [evl].[EventProperties]
(
	[PropertyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_EventTags_TagId]    Script Date: 31/10/2017 10:23:46 ******/
CREATE NONCLUSTERED INDEX [IX_EventTags_TagId] ON [evl].[EventTags]
(
	[TagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Profile_Key]    Script Date: 31/10/2017 10:23:46 ******/
CREATE NONCLUSTERED INDEX [IX_Profile_Key] ON [evl].[Profile]
(
	[Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Profile_OrganisationId]    Script Date: 31/10/2017 10:23:46 ******/
CREATE NONCLUSTERED INDEX [IX_Profile_OrganisationId] ON [evl].[Profile]
(
	[OrganisationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProfileRoles_RoleId]    Script Date: 31/10/2017 10:23:46 ******/
CREATE NONCLUSTERED INDEX [IX_ProfileRoles_RoleId] ON [evl].[ProfileRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Property_Category_Name]    Script Date: 31/10/2017 10:23:46 ******/
CREATE NONCLUSTERED INDEX [IX_Property_Category_Name] ON [evl].[Property]
(
	[Category] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Property_Name]    Script Date: 31/10/2017 10:23:46 ******/
CREATE NONCLUSTERED INDEX [IX_Property_Name] ON [evl].[Property]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_RolePermissions_RoleId]    Script Date: 31/10/2017 10:23:46 ******/
CREATE NONCLUSTERED INDEX [IX_RolePermissions_RoleId] ON [evl].[RolePermissions]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Rule_OrganisationId]    Script Date: 31/10/2017 10:23:46 ******/
CREATE NONCLUSTERED INDEX [IX_Rule_OrganisationId] ON [evl].[Rule]
(
	[OrganisationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Tag_Name]    Script Date: 31/10/2017 10:23:46 ******/
CREATE NONCLUSTERED INDEX [IX_Tag_Name] ON [evl].[Tag]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Tag_OrganisationId]    Script Date: 31/10/2017 10:23:46 ******/
CREATE NONCLUSTERED INDEX [IX_Tag_OrganisationId] ON [evl].[Tag]
(
	[OrganisationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Trigger_EventId]    Script Date: 31/10/2017 10:23:46 ******/
CREATE NONCLUSTERED INDEX [IX_Trigger_EventId] ON [evl].[Trigger]
(
	[EventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [evl].[Action]  WITH CHECK ADD  CONSTRAINT [FK_Action_Trigger_TriggerId] FOREIGN KEY([TriggerId])
REFERENCES [evl].[Trigger] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [evl].[Action] CHECK CONSTRAINT [FK_Action_Trigger_TriggerId]
GO
ALTER TABLE [evl].[ActionProperties]  WITH CHECK ADD  CONSTRAINT [FK_ActionProperties_Action_ActionId] FOREIGN KEY([ActionId])
REFERENCES [evl].[Action] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [evl].[ActionProperties] CHECK CONSTRAINT [FK_ActionProperties_Action_ActionId]
GO
ALTER TABLE [evl].[ActionProperties]  WITH CHECK ADD  CONSTRAINT [FK_ActionProperties_Property_PropertyId] FOREIGN KEY([PropertyId])
REFERENCES [evl].[Property] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [evl].[ActionProperties] CHECK CONSTRAINT [FK_ActionProperties_Property_PropertyId]
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
ALTER TABLE [evl].[EventProperties]  WITH CHECK ADD  CONSTRAINT [FK_EventProperties_Property_PropertyId] FOREIGN KEY([PropertyId])
REFERENCES [evl].[Property] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [evl].[EventProperties] CHECK CONSTRAINT [FK_EventProperties_Property_PropertyId]
GO
ALTER TABLE [evl].[EventTags]  WITH CHECK ADD  CONSTRAINT [FK_EventTags_Event_EventId] FOREIGN KEY([EventId])
REFERENCES [evl].[Event] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [evl].[EventTags] CHECK CONSTRAINT [FK_EventTags_Event_EventId]
GO
ALTER TABLE [evl].[EventTags]  WITH CHECK ADD  CONSTRAINT [FK_EventTags_Tag_TagId] FOREIGN KEY([TagId])
REFERENCES [evl].[Tag] ([Id])
GO
ALTER TABLE [evl].[EventTags] CHECK CONSTRAINT [FK_EventTags_Tag_TagId]
GO
ALTER TABLE [evl].[Profile]  WITH CHECK ADD  CONSTRAINT [FK_Profile_Organisation_OrganisationId] FOREIGN KEY([OrganisationId])
REFERENCES [evl].[Organisation] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [evl].[Profile] CHECK CONSTRAINT [FK_Profile_Organisation_OrganisationId]
GO
ALTER TABLE [evl].[ProfileRoles]  WITH CHECK ADD  CONSTRAINT [FK_ProfileRoles_Profile_ProfileId] FOREIGN KEY([ProfileId])
REFERENCES [evl].[Profile] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [evl].[ProfileRoles] CHECK CONSTRAINT [FK_ProfileRoles_Profile_ProfileId]
GO
ALTER TABLE [evl].[ProfileRoles]  WITH CHECK ADD  CONSTRAINT [FK_ProfileRoles_Role_RoleId] FOREIGN KEY([RoleId])
REFERENCES [evl].[Role] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [evl].[ProfileRoles] CHECK CONSTRAINT [FK_ProfileRoles_Role_RoleId]
GO
ALTER TABLE [evl].[RolePermissions]  WITH CHECK ADD  CONSTRAINT [FK_RolePermissions_Permission_PermissionId] FOREIGN KEY([PermissionId])
REFERENCES [evl].[Permission] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [evl].[RolePermissions] CHECK CONSTRAINT [FK_RolePermissions_Permission_PermissionId]
GO
ALTER TABLE [evl].[RolePermissions]  WITH CHECK ADD  CONSTRAINT [FK_RolePermissions_Role_RoleId] FOREIGN KEY([RoleId])
REFERENCES [evl].[Role] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [evl].[RolePermissions] CHECK CONSTRAINT [FK_RolePermissions_Role_RoleId]
GO
ALTER TABLE [evl].[Rule]  WITH CHECK ADD  CONSTRAINT [FK_Rule_Organisation_OrganisationId] FOREIGN KEY([OrganisationId])
REFERENCES [evl].[Organisation] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [evl].[Rule] CHECK CONSTRAINT [FK_Rule_Organisation_OrganisationId]
GO
ALTER TABLE [evl].[Tag]  WITH CHECK ADD  CONSTRAINT [FK_Tag_Organisation_OrganisationId] FOREIGN KEY([OrganisationId])
REFERENCES [evl].[Organisation] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [evl].[Tag] CHECK CONSTRAINT [FK_Tag_Organisation_OrganisationId]
GO
ALTER TABLE [evl].[Trigger]  WITH CHECK ADD  CONSTRAINT [FK_Trigger_Event_EventId] FOREIGN KEY([EventId])
REFERENCES [evl].[Event] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [evl].[Trigger] CHECK CONSTRAINT [FK_Trigger_Event_EventId]
GO
