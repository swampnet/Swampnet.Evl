import { Component } from '@angular/core';
import { ProjectRoleService } from '../../services/project-role.service';
import { RuleSummary } from '../../entities/entities';

@Component({
    selector: 'rules',
    templateUrl: './rules.component.html',
    styleUrls: ['./rules.component.css']
})
export class RulesComponent {
    public rules: RuleSummary[];

    constructor(
        private _projectService: ProjectRoleService) {
    }

    ngOnInit() {

        this._projectService.getRules().then((res: RuleSummary[]) => {
            this.rules = res;
        }, (error) => {
            console.log("Failed to get rule", error._body, "error");
        });

    }
}