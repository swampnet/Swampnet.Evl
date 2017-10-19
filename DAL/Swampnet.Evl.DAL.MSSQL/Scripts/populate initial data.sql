-- Populate initial data
begin tran

declare @default_organisation uniqueidentifier = '8A6B0C75-F158-4FAC-8668-B51F89FAA8EE'
declare @default_api_key uniqueidentifier = '3B94A54F-FDF2-4AFF-AA80-A35ED5836841'

insert into [evl].[organisation] (Id, ApiKey, [Description], [Name]) values
	(@default_organisation, @default_api_key, 'Event Logging', 'Evl')

insert into [evl].[apiKey] (Id, CreatedOnUtc, OrganisationId, RevokedOnUtc) values
	(@default_api_key, GetUtcDate(), @default_organisation, null),
	('58BAD582-C6CF-407A-B482-502FB423CD55', GetUtcDate(), @default_organisation, null)


commit
--rollback
