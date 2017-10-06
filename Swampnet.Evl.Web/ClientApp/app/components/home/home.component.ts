import { Component } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent {

    constructor(
        private _api: ApiService) {
    }

    get date(): Date {
        return new Date();
    }
}
