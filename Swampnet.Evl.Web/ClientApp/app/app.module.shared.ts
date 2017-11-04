import { NgModule } from '@angular/core';
import { CommonModule  } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';

import { ApiService } from './services/api.service'
import { Globals } from './services/globals'

import { RulesComponent } from './components/rule/rules.component';
import { RuleDetailsComponent } from './components/rule/rule-details.component';
import { ExpressionComponent } from './components/expression/expression.component';
import { ActionDefinitionComponent } from './components/action-definition/action-definition.component';
import { EventsComponent } from './components/events/events.component';
import { EventDetailsComponent } from './components/events/event-details.component';
import { EventsSearchCriteriaComponent } from './components/events/events-search-criteria.component';
import { ProfileComponent } from './components/profile/profile.component';

import { DragulaModule } from 'ng2-dragula/ng2-dragula';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        RulesComponent,
        RuleDetailsComponent,
        ExpressionComponent,
        ActionDefinitionComponent,
        EventsComponent,
		EventDetailsComponent,
		EventsSearchCriteriaComponent,
		ProfileComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
		FormsModule,
		DragulaModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },

            { path: 'rules', component: RulesComponent },
            { path: 'rules/:id', component: RuleDetailsComponent },

            { path: 'events', component: EventsComponent },
			{ path: 'events/:id', component: EventDetailsComponent },

			{ path: 'profile', component: ProfileComponent },

			{ path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [
		ApiService,
		Globals
    ]
})
export class AppModuleShared {
}
