-- Populate initial data
begin tran

declare @evl_organisation uniqueidentifier = '60FACD3A-2232-4E15-9F3C-61289CDDD544'
declare @mocked_organisation uniqueidentifier = '8A6B0C75-F158-4FAC-8668-B51F89FAA8EE'

insert into [evl].[organisation] (Id, ApiKey, [Description], [Name]) values
	(@evl_organisation, '25C135A0-B574-4A9B-BC37-4F0694017896', 'Evl', 'Evl'),
	(@mocked_organisation, '3B94A54F-FDF2-4AFF-AA80-A35ED5836841', 'Mocked organisation', 'Mocked')

insert into [evl].[apiKey] (Id, CreatedOnUtc, OrganisationId, RevokedOnUtc) values
	-- Evl
	('25C135A0-B574-4A9B-BC37-4F0694017896', GetUtcDate(), @evl_organisation, null),

	-- Mocked
	('3B94A54F-FDF2-4AFF-AA80-A35ED5836841', GetUtcDate(), @mocked_organisation, null),
	('58BAD582-C6CF-407A-B482-502FB423CD55', GetUtcDate(), @mocked_organisation, null)

-- Permissions
insert into evl.Permission (IsEnabled, Name) values
	(1, 'rule.view'),
	(1, 'rule.create'),
	(1, 'rule.edit'),
	(1, 'rule.delete'),

	(1, 'organisation.view'),
	(1, 'organisation.create'),
	(1, 'organisation.edit'),
	(1, 'organisation.delete'),
	(1, 'organisation.view-all')

-- Roles
insert into evl.[Role] (Name) values
	('owner'),
	('admin'),
	('user')

insert into evl.RolePermissions (RoleId, PermissionId)
select r.Id, p.Id
from evl.[Role] r, evl.Permission p
where r.name = 'admin'
and p.name in (
	'rule.view',
	'rule.create',
	'rule.edit',
	'rule.delete',

	'organisation.view',
	'organisation.create',
	'organisation.edit',
	'organisation.delete',
	'organisation.view-all'
)

insert into evl.RolePermissions (RoleId, PermissionId)
select r.Id, p.Id
from evl.[Role] r, evl.Permission p
where r.name = 'user'
and p.name in (
	'organisation.view'
)

-- Profiles
insert into evl.[Profile] (Title, Firstname, Lastname, KnownAs, OrganisationId, [Key]) values
	('Mr', 'Pete', 'Whitby', 'pj', @mocked_organisation, '@todo-pjw-001'),
	('Mr', 'Testy', 'McTestface', 'tf', @mocked_organisation, '@todo-tf-001')

-- Add admin & user roles
insert into evl.ProfileRoles (ProfileId, RoleId)
select p.Id, r.Id
from evl.[profile] p, evl.[role] r
where p.[key] = '@todo-pjw-001'
and r.name in ('admin', 'user')

-- Add admin & user roles
insert into evl.ProfileRoles (ProfileId, RoleId)
select
	p.Id,
	r.Id
from evl.[profile] p, evl.[role] r
where p.[key] = '@todo-tf-001'
and r.name in ('user')


--commit
rollback
