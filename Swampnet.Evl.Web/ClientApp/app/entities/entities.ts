
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
    requiresOperand: boolean;
    requiresValue: boolean;
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
	tags?: string;
    timestampUtc?: Date;
    fromUtc?: Date;
    toUtc?: Date;
    pageSize?: number;
    page?: number;
}

export interface TriggerAction{
	timestampUtc: Date;
	type: string;
	error: string;
	properties: Property[];
}

export interface Trigger {
	timestampUtc: Date;
	ruleName: string;
	ruleId: string;
	actions: TriggerAction[];
}

export interface EventDetails {
    id: string;
    category: string;
    summary: string;
    timestampUtc: Date;
	properties: Property[];
	tags: string[];
	triggers: Trigger[];
}

export interface Cfg {
    apiRoot: string;
}