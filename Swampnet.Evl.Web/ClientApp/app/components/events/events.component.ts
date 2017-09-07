import { Component } from '@angular/core';
import { EventSummary, EventSearchCriteria } from '../../entities/entities';
import { ApiService } from '../../services/api.service';

@Component({
    selector: 'events',
    templateUrl: './events.component.html',
    styleUrls: ['./events.component.css']
})
export class EventsComponent {
    public events: EventSummary[];
    public criteria: EventSearchCriteria = {
        category: "Information"
    };

    constructor(
        private _api: ApiService) {
    }

    ngOnInit() {
        this.searchEvents();
    }

    searchEvents() {
        this._api.searchEvents(this.criteria).then((res: EventSummary[]) => {
            this.events = res;
        }, (error) => {
            console.log("Failed to get events", error._body, "error");
        });
    }
}
