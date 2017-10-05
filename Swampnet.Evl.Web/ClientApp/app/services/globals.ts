import { Injectable, Inject } from '@angular/core';
import { EventSearchCriteria } from '../entities/entities';

@Injectable()
export class Globals {
	public criteria: EventSearchCriteria = {
		category: "Information",
		pageSize: 20,
		page: 0
	};
}