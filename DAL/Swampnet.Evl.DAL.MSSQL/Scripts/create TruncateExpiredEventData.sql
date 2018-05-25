/****** Object:  StoredProcedure [evl].[TrucateExpiredEventData]    Script Date: 25/05/2018 11:57:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Pj
-- Create Date: 
-- Description: Clear down expired event data
-- =============================================
CREATE PROCEDURE [evl].[TrucateExpiredEventData]
AS
BEGIN
    SET NOCOUNT ON

	-- Delete expired
	delete from evl.[event] where id in (
		select e.id from evl.[Event] e
			join evl.Organisation org
			 on e.OrganisationId = org.Id
		where 
			   ( e.Category <> 'debug'
				 and dateadd(day, org.EventDaysToLive, e.ModifiedOnUtc) < getUtcDate() )

			or ( e.Category = 'debug'
				 and dateadd(day, org.DebugEventDaysToLive, e.ModifiedOnUtc) < getUtcDate() )
	)


	-- Clean up orphan properties
	delete from evl.Property where id in (
		select 
			p.Id
		from evl.Property p
			left join evl.EventProperties ep
			 on p.id = ep.PropertyId
			left join evl.ActionProperties ap
			 on p.id = ap.PropertyId
			left join evl.OrganisationProperties op
			 on p.id = op.PropertyId
		where
			ep.PropertyId is null
			and ap.PropertyId is null
			and op.PropertyId is null
	)

END
GO


