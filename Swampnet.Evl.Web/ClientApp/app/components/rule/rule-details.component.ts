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

    _rule?: Rule;
    _metaData: MetaData;

    constructor(
        private _api: ApiService,
        private _route: ActivatedRoute) {
	}

	// @TODO: We're probably going to be passing an ID into this component? (ie: /rules/abcd-efg)
    ngOnInit() {
        // @TODO: Need to unsub from this!
        this.sub = this._route.params.subscribe(params => {
            let id = params['id'];

			this._api.getMetaData().then((res: MetaData) => {
                this._metaData = res;

                // Load rule data
				this._api.getRule(id).then((res: Rule) => {
                    this._rule = res;
                }, (error) => {
                    console.log("Failed to get rule", error._body, "error");
                });
            }, (error) => {
                console.log("Failed to get meta", error._body, "error");
            });

        });


		//let id = "abcdefg-hijklmnop-qrs-tuv-wxyz";

  //      this._projectService.getMetaData().then((res: MetaData) => {
  //          this._metaData = res;

  //          // Load rule data
  //          this._projectService.getRule(id).then((res: Rule) => {
  //              this._rule = res;
  //          }, (error) => {
  //              console.log("Failed to get rule", error._body, "error");
  //          });
  //      }, (error) => {
  //          console.log("Failed to get meta", error._body, "error");
  //      });

	}

	clear() {
		this._rule = undefined;
	}

	clearExpression() {
		if (this._rule) {
			this._rule.expression = undefined;
		}
	}

	createRootExpression() {
		if (this._rule) {
			this._rule.expression = {
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
        if (this._rule) {
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

            if (!this._rule.actions) {
                this._rule.actions = [];
            }

            this._rule.actions.push(action);
        }
    }

    getActionMetaData(def: ActionDefinition) {
        return this._metaData.actionMetaData.find(a => a.type == def.type);
    }

	save() {
		this._api.saveRule(this._rule)
			.then(() => console.log("saved"),
			(e) => { console.log("failed", e._body, "error") }
			);
	}
}

