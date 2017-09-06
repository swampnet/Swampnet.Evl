import { Component, Input } from '@angular/core';
import { Expression, MetaData, ExpressionOperator, MetaDataCapture } from '../../entities/entities';

@Component({
    selector: 'expression-view',
    templateUrl: './expression.component.html',
    styleUrls: ['./expression.component.css']
})
export class ExpressionComponent {
	@Input() expression: Expression;
    @Input() isRoot: boolean;
    @Input() metaData: MetaData;

	isMouseOverDelete: boolean;

    addChild() {
        if (!this.expression.children) {
            this.expression.children = [];
        }

        this.expression.children.push({
            operator: "eq",
            operand: "Summary", // We need a default summary for some reason?
            argument: "",
            value: "",
            isActive: true,
            children: []
        });
    }

	addGroup() {
		if (!this.expression.children) {
			this.expression.children = [];
		}

		this.expression.children.push({
			operator: "match_all",
			operand: "",
			argument: "",
			value: "",
			isActive: true,
			children: []
		});
	}

	delMouseEnter() {
		this.isMouseOverDelete = true;
	}

	delMouseLeave() {
		this.isMouseOverDelete = false;
	}


    get expressionOperators(): ExpressionOperator[] {
        return this.metaData.operators.filter(o => !o.isGroup);
    }

    get groupOperators(): ExpressionOperator[] {
        return this.metaData.operators.filter(o => o.isGroup);
    }

    get operands(): MetaDataCapture[] {
        return this.metaData.operands;
    }

    getOperandMetaData(op: string) {
        return this.metaData.operands.find(i => i.name == op);
    }

    // We don't actually delete stuff, just mark as inactive. We can let the server actually delete stuff if it
    // wants to (if we deleted it here, the server wouldn't even have any way of knowing it had been deleted!)
    deleteExpression() {
        this.expression.isActive = false;        
    };


    // It's a container if the operand requires child exprssions
    get isContainer(): boolean {
        return this.expression.operator.toUpperCase() === "MATCH_ALL"
            || this.expression.operator.toUpperCase() === "MATCH_ANY";
	}


	get isActive(): boolean {
		return this.expression.isActive;
	}
}
