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


commit
--rollback
