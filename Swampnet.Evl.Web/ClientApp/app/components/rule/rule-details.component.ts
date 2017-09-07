import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { Rule, Property, ActionDefinition, MetaData, ActionMetaData } from '../../entities/entities';

@Component({
	selector: 'rule-details',
	templateUrl: './rule-details.component.html',
	styleUrls: ['./rule-details.component.css']
})
export class RuleDetailsComponent {

    private sub: any;
    private id: string;

    rule?: Rule;
    metaData: MetaData;

    constructor(
        private api: ApiService,
        private route: ActivatedRoute) {
	}

	// @TODO: We're probably going to be passing an ID into this component? (ie: /rules/abcd-efg)
    ngOnInit() {
        // @TODO: Need to unsub from this!
        this.sub = this.route.params.subscribe(params => {
            let id = params['id'];

			this.api.getMetaData().then((res: MetaData) => {
                this.metaData = res;

                // Load rule data
				this.api.getRule(id).then((res: Rule) => {
                    this.rule = res;
                }, (error) => {
                    console.log("Failed to get rule", error._body, "error");
                });
            }, (error) => {
                console.log("Failed to get meta", error._body, "error");
            });

        });
	}

	clear() {
		this.rule = undefined;
	}

	clearExpression() {
		if (this.rule) {
			this.rule.expression = undefined;
		}
	}

	createRootExpression() {
		if (this.rule) {
			this.rule.expression = {
				operator: "MATCH_ALL",
				operand: "",
				argument: "",
				value: "",
				isActive: true,
				children: [
					{
						operator: "eq",
						operand: "Category",
						argument: "",
						value: "information",
						isActive: true,
						children: []
					}
				]
			};
		}
	}

    addAction(meta: ActionMetaData) {
        if (this.rule) {
            let action = {
                type: meta.type,
                isActive: true,
                properties: Array<Property>()
            };

            meta.properties.forEach(p => {
                action.properties.push({
                    name: p.name,
                    category: "",
                    value: ""
                });
            });

            if (!this.rule.actions) {
                this.rule.actions = [];
            }

            this.rule.actions.push(action);
        }
    }

    getActionMetaData(def: ActionDefinition) {
        return this.metaData.actionMetaData.find(a => a.type == def.type);
    }

	save() {
		this.api.saveRule(this.rule)
			.then(() => console.log("saved"),
			(e) => { console.log("failed", e._body, "error") }
			);
	}
}

