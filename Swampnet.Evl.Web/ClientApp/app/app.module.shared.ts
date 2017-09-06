import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';

import { ProjectRoleService } from './services/project-role.service'
import { RulesComponent } from './components/rule/rules.component';
import { RuleDetailsComponent } from './components/rule/rule-details.component';
import { ExpressionComponent } from './components/expression/expression.component';
import { ActionDefinitionComponent } from './components/action-definition/action-definition.component';


@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        RulesComponent,
        RuleDetailsComponent,
        ExpressionComponent,
        ActionDefinitionComponent
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

            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [
        ProjectRoleService
    ]
})
export class AppModuleShared {
}
