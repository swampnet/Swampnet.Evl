export interface RuleSummary {
    id: string;
    name: string;
}

export interface Expression {
	operator: string;
	operand: string;
	argument: string;
	value: string;
	isActive: boolean;
	children: Expression[];
}

export interface Property {
	category: string;
	name: string;
	value: string;
}

export interface ActionDefinition {
	type: string;
	properties?: Property[];
	isActive: boolean;
}

export interface Rule {
	id: string;
	name: number;
	expression?: Expression;
	actions?: ActionDefinition[];
}

export interface Option {
    display: string;
    value: string;
}

export interface MetaDataCapture {
    name: string;
    isRequired: boolean;
    dataType: string;
    options: Option[];
}

export interface ActionMetaData {
    type: string;
    properties: MetaDataCapture[];
}

export interface ExpressionOperator {
    code: string;
    display: string;
    isGroup: boolean;
}

export interface MetaData{
    actionMetaData: ActionMetaData[];
    operands: MetaDataCapture[];
    operators: ExpressionOperator[];
}