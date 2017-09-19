import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';

import { ApiService } from './services/api.service'

import { RulesComponent } from './components/rule/rules.component';
import { RuleDetailsComponent } from './components/rule/rule-details.component';
import { ExpressionComponent } from './components/expression/expression.component';
import { ActionDefinitionComponent } from './components/action-definition/action-definition.component';
import { EventsComponent } from './components/events/events.component';
import { EventDetailsComponent } from './components/events/event-details.component';
import { EventsSearchCriteriaComponent } from './components/events/events-search-criteria.component';


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
        EventsSearchCriteriaComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },

            { path: 'rules', component: RulesComponent },
            { path: 'rules/:id', component: RuleDetailsComponent },

            { path: 'events', component: EventsComponent },
			{ path: 'events/:id', component: EventDetailsComponent },

            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [
        ApiService
    ]
})
export class AppModuleShared {
}
