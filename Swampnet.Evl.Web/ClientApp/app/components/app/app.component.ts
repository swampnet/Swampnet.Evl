import { Component, ViewEncapsulation } from '@angular/core';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
	styleUrls: [
		'./app.component.css',
		'../../../../node_modules/dragula/dist/dragula.css',
		'../../../../node_modules/ng-pick-datetime/assets/style/picker.min.css'
	],
    encapsulation: ViewEncapsulation.None
})
export class AppComponent {
}
