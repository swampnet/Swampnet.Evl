import { Component } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent {
    public stats: any;

    constructor(
        private _api: ApiService) {
    }

    get date(): Date {
        return new Date();
    }

    ngOnInit() {
        this.loadStats();
    }

    loadStats() {
        this._api.getStats().then((res: any) => {
            this.stats = res;
        }, (error) => {
            console.log("Failed to get stats", error._body, "error");
        });
    }
}
