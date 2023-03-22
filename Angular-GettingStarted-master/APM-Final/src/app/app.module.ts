import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { WelcomeComponent } from './welcome.component';





@NgModule({
  declarations: [
    AppComponent,
    WelcomeComponent  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot([
        { path: 'welcome', component: WelcomeComponent },     
        
        { path: '', redirectTo: 'welcome', pathMatch: 'full'},
        { path: '**', redirectTo: 'welcome', pathMatch: 'full'},
        
    ]),
    
    
    
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }