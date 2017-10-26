import { Component } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
    selector: 'profile',
	templateUrl: './profile.component.html',
	styleUrls: ['./profile.component.css']
})
export class ProfileComponent {

	org: any;

    constructor(
        private api: ApiService) {
    }


	async ngOnInit() {
		try {
			this.org = await this.api.getOrganisation();
		} catch (e) {
			console.error(e);
		}
    }

}
