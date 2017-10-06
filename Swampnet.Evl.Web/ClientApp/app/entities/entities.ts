
export interface RuleSummary {
    id: string;
    name: string;
    actions: string[];
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
	id?: string;
    name: string;
    isActive: boolean;
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

export interface EventSummary {
    id: string;
    category: string;
    summary: string;
    timestampUtc: Date;
}

export interface EventSearchCriteria {
    id?: string;
    source?: string;
    category?: string;
    summary?: string;
    timestampUtc?: Date;
    fromUtc?: Date;
    toUtc?: Date;
    pageSize?: number;
    page?: number;
}

export interface Event {
    id: string;
    category: string;
    summary: string;
    timestampUtc: Date;
    properties: Property[];
}

export interface Cfg {
    apiRoot: string;
}