﻿import { Component, Input } from '@angular/core';
import { EventSearchCriteria } from '../../entities/entities';
import { ApiService } from '../../services/api.service';

@Component({
    selector: 'events-search-criteria',
	templateUrl: './events-search-criteria.component.html',
    styleUrls: ['./events-search-criteria.component.css']
})
export class EventsSearchCriteriaComponent {
    @Input() criteria: EventSearchCriteria;
}
