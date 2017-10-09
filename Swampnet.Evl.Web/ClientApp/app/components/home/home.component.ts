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

    async loadStats() {
        try {
            this.stats = await this._api.getStats();
        } catch(e) {
            console.error(e);
        }
    }

}
