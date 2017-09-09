import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { RuleSummary } from '../../entities/entities';

@Component({
    selector: 'rules',
    templateUrl: './rules.component.html',
    styleUrls: ['./rules.component.css']
})
export class RulesComponent {
    public rules: RuleSummary[];

    constructor(
        private router: Router,        
        private api: ApiService) {
    }

    ngOnInit() {

        this.api.getRules().then((res: RuleSummary[]) => {
            this.rules = res;
        }, (error) => {
            console.log("Failed to get rule", error._body, "error");
        });

    }

    newRule(){
        this.router.navigate(['/rules/new']);
    }
}