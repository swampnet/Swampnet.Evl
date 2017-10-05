﻿import { Component } from '@angular/core';
import { EventSummary, EventSearchCriteria } from '../../entities/entities';
import { ApiService } from '../../services/api.service';
import { Globals } from '../../services/globals';

@Component({
    selector: 'events',
    templateUrl: './events.component.html',
    styleUrls: ['./events.component.css']
})
export class EventsComponent {
    public events: EventSummary[];

	constructor(
		private globals: Globals,
		private api: ApiService) {
	}

	get criteria(): EventSearchCriteria {
		return this.globals.criteria;
	}


    ngOnInit() {
        this.searchEvents();
    }

    searchEvents() {
        //let max = new Date();

        //if (this.events) {
        //    max = this.events
        //        .map(x => x.timestampUtc)
        //        .reduce((a, b) => a > b ? a : b);
        //}

        //console.log("*** max: " + max);

        this.api.searchEvents(this.criteria).then((res: EventSummary[]) => {
            this.events = res;
        }, (error) => {
            console.log("Failed to get events", error._body, "error");
        });
    }
}
