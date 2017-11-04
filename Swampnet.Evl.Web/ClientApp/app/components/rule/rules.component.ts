import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { RuleSummary } from '../../entities/entities';
import { DragulaService } from 'ng2-dragula/ng2-dragula';

@Component({
    selector: 'rules',
    templateUrl: './rules.component.html',
    styleUrls: ['./rules.component.css']
})
export class RulesComponent implements OnInit, OnDestroy {
    public rules: RuleSummary[];
	private dropSubscription: any;

    constructor(
        private router: Router,        
		private api: ApiService,
		private dragula: DragulaService) {
		console.log("RulesComponent.ctor");

		this.dragula.setOptions('bag-items', {
			revertOnSpill: true
		});
    }


	ngOnInit() {
		console.log("RulesComponent.OnInit");

		this.dropSubscription = this.dragula
			.dropModel
			.subscribe((x: any) => {
				this.reorder();
			});

		this.api.getRules().then((res: RuleSummary[]) => {
			this.rules = res;
        }, (error) => {
            console.log("Failed to get rule", error._body, "error");
        });
	}


	ngOnDestroy() {
		console.log("RulesComponent.OnDestroy");

		this.dropSubscription.unsubscribe();
		this.dragula.destroy('bag-items');
	}


    newRule(){
        this.router.navigate(['/rules/new']);
	}


	reorder() {
		console.log("reorder");
		var i: number;
		for (i = 0; i < this.rules.length; i++) {
			this.rules[i].order = i;
		}
		this.api.reorderRules(this.rules);
	}
}