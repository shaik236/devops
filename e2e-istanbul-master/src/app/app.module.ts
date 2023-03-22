import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { WelcomeComponent } from './welcome.component';
import {AutomationResultsComponent} from './AutomationResults.component';
import {ConfigureDashboardComponent} from './ConfigureDashboard.component';
import {AutomationResultsDetailComponent} from './AutomationResultsDetail.component'
import { DashboardComponent } from './Dashboard.component';


@NgModule({
  declarations: [
    AppComponent,
    WelcomeComponent, AutomationResultsComponent,DashboardComponent,AutomationResultsDetailComponent, ConfigureDashboardComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot([
        { path: 'welcome', component: WelcomeComponent },
        { path: 'automationresults', component: AutomationResultsComponent },
        { path: 'automationresultsdetail', component: AutomationResultsDetailComponent },
        { path: 'dashboard', component: DashboardComponent },
        { path: 'ConfigureDashboard', component: ConfigureDashboardComponent },
        { path: '', redirectTo: 'welcome', pathMatch: 'full'},
        { path: '**', redirectTo: 'welcome', pathMatch: 'full'}
    ]),
    
    
    
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
