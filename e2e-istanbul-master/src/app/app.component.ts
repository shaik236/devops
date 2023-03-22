import { Component } from '@angular/core';

@Component({
  selector: 'pm-root',
  template: `
    <div>
        <nav class='navbar navbar-default'>
            <div class='container-fluid'>
                    <ul class='nav navbar-nav'>
                    <li><a [routerLink]="['/welcome']">Dashboard</a></li>
                    <li><a [routerLink]="['/automationresults']">Automation Results</a></li>
                    <li><a [routerLink]="['/dashboard']">Build Results</a></li>
                    <li><a [routerLink]="['/dashboard']">Automation Coverage</a></li>
                    <li><a [routerLink]="['/dashboard']">Code Coverage from Automation</a></li>
                    <li><a [routerLink]="['/dashboard']">Unit Test Results</a></li>
                    <li><a [routerLink]="['/dashboard']">Release Frequency</a></li>
                    <li><a [routerLink]="['/ConfigureDashboard']">Configure Dashboard</a></li>
                    
                </ul>
            </div>
        </nav>
        <div class='container'>
            <router-outlet></router-outlet>
        </div>
     </div>
    `
})
export class AppComponent {
  pageTitle: string = 'DevOps Project Chart';
}
