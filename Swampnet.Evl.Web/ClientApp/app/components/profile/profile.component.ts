import { Component } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
    selector: 'profile',
	templateUrl: './profile.component.html',
	styleUrls: ['./profile.component.css']
})
export class ProfileComponent {

	profile: any;

    constructor(
        private api: ApiService) {
    }


	async ngOnInit() {
		try {
            this.profile = await this.api.getProfile();
		} catch (e) {
			console.error(e);
		}
    }

}
