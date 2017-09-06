import { Component, Input } from '@angular/core';
import { ActionDefinition, ActionMetaData, Property } from '../../entities/entities';

@Component({
	selector: 'action-definition',
	templateUrl: './action-definition.component.html',
	styleUrls: ['./action-definition.component.css']
})
export class ActionDefinitionComponent {
	@Input() action: ActionDefinition;
    @Input() metaData: ActionMetaData;

	deleteAction() {
		this.action.isActive = false;
    }

    getPropertyMetaData(property: Property) {
        return this.metaData.properties.find(p => p.name == property.name);
    }
}

