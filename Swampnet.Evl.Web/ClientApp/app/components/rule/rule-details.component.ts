import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
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
        private router: Router,
        private route: ActivatedRoute) {
	}

	// @TODO: We're probably going to be passing an ID into this component? (ie: /rules/abcd-efg)
    ngOnInit() {
        // @TODO: Need to unsub from this!
        this.sub = this.route.params.subscribe(params => {
            let id = params['id'];

			this.api.getMetaData().then((res: MetaData) => {
                this.metaData = res;

                if(id == "new"){
                    this.rule = this.createRootExpression();
                } else {
                    // Load rule data
                    this.api.getRule(id).then((res: Rule) => {
                        this.rule = res;
                    }, (error) => {
                        console.log("Failed to get rule", error._body, "error");
                    });
                }
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
        return {
            id: undefined,
            name: "New rule",
			isActive: true,
			order: 999999,
            expression: {
                operator: "MATCH_ALL",
                operand: "NULL",
                argument: "",
                value: "",
                isActive: true,
                children: [
                    {
                        operator: "EQ",
                        operand: "Category",
                        argument: "",
                        value: "Information",
                        isActive: true,
                        children: []
                    }
                ]
            },
            actions: []
        };
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
        if(this.rule != null && this.rule.id == undefined){
            this.api.createRule(this.rule)
			.then(() => {
                console.log("saved");
                this.router.navigate(['/rules']);                
            },
			(e) => { console.log("failed", e._body, "error") }
			);
        } else {
            this.api.updateRule(this.rule)
            .then(() => {
                console.log("saved");
                this.router.navigate(['/rules']);
            },
            (e) => { console.log("failed", e._body, "error") }
            );    
        }
    }
    
    cancel(){
        this.router.navigate(['/rules']);                        
    }

    delete(){
        if(this.rule && this.rule.id){
            this.api.deleteRule(this.rule.id).then((res: any) => {
                this.router.navigate(['/rules']);                
            }, (error) => {
                console.log("Failed to get rule", error._body, "error");
            });
        }
    }

}

