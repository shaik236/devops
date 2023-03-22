import { Component} from '@angular/core';



@Component({
  templateUrl: './Dashboard.component.html',
  styleUrls: ['./Dashboard.component.css']

})
export class DashboardComponent {
  
    pageTitle: string = 'Build Results';
    hrefaddr: string = 'https://slb-swt.visualstudio.com/_projects';
    ngOnInit()
    {

    }
  }