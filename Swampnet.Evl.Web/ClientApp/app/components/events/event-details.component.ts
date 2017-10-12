import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Event } from '../../entities/entities';
import { ApiService } from '../../services/api.service';

@Component({
    selector: 'event-details',
    templateUrl: './event-details.component.html'
})
export class EventDetailsComponent {
    event: Event;
    private sub: any;

    constructor(
        private _api: ApiService,
        private _route: ActivatedRoute) {
    }

    ngOnInit() {
        this.loadEvent();
    }

    loadEvent() {
        this.sub = this._route.params.subscribe(params => {
            let id = params['id'];

            this._api.getEvent(id).then((res: Event) => {
                this.event = res;
            }, (error) => {
                console.log("Failed to get event", error._body, "error");
            });
        });
    }
}
