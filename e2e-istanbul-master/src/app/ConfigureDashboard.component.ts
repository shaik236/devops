import { Component, OnInit }  from '@angular/core';
import {TemplateRef, ViewChild} from '@angular/core';
import {Headers, Response} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {Projects} from './Projects';
import {Project} from './Project.model';
import 'rxjs/Rx';

@Component({
    templateUrl: './ConfigureDashboard.component.html'
})
export class ConfigureDashboardComponent {
    @ViewChild('readOnlyTemplate') readOnlyTemplate: TemplateRef<any>;
    @ViewChild('editTemplate') editTemplate: TemplateRef<any>;
    public pageTitle: string = 'Configure Dashboard';
    projects: Array<Projects>;
    selemp: Projects;
    isNewRecord: boolean;
    statusMessage: string = "Record Added / Updated/ Deleted Successfully";

    constructor() {
        this.projects = new Array<Projects>();
        this.projects;
let vr = new Projects;
vr.ID = "1";
vr.ProjectName = "AvocetMain";
vr.DisplayName = "Avocet_2017_2";
vr.URL = "https://slb5-swt.visualstudio.com";
vr.PAT = "************** ";
vr.PATExpiryDate = "25/08/2018";
vr.TestPlanID = "100590";
vr.TestSuiteID = "110473,110474";
vr.BuildDefinitionID = "1123451";
vr.ReleaseDefinitionID = "653";
        this.projects.push(vr);

        let vr1 = new Projects;
vr1.ID = "2";
vr1.ProjectName = "AvocetMain";
vr1.DisplayName = "Avocet_2017_1";
vr1.URL = "https://slb5-swt.visualstudio.com";
vr1.PAT = "************** ";
vr1.PATExpiryDate = "25/08/2018";
vr1.TestPlanID = "100590";
vr1.TestSuiteID = "110473,110474";
vr1.BuildDefinitionID = "1123451";
vr1.ReleaseDefinitionID = "256";

this.projects.push(vr1);

    }

    ngOnInit() {
        this.loadEmployee();
    }

    private loadEmployee() {


    }

    addEmp() {
        let vr = new Projects;
        vr.ID = "";
        vr.ProjectName = "";
        vr.DisplayName = "";
        vr.URL = "";
        vr.PAT = "";
        vr.PATExpiryDate = "";
        vr.TestPlanID = "";
        vr.TestSuiteID = "";
        vr.BuildDefinitionID = "";
        vr.ReleaseDefinitionID = "";

        this.selemp = vr;
        this
            .projects
            .push(this.selemp);
        this.isNewRecord = true;
        //return this.editTemplate;
    }
 
    //6. Edit Employee
    editEmployee(emp: Projects) {
        this.selemp = emp;
    }

    cancel() {
        this.selemp = null;
    }
    //10 Delete Employee
    deleteEmp(emp: Projects) {
        
        this.statusMessage = 'Record Deleted Successfully.',
        this.loadEmployee();
    }

    saveEmp() {
        if (this.isNewRecord) {
            //add a new Employee
            
                    this.statusMessage = 'Record Added Successfully.';
                    this.loadEmployee();
           
            this.isNewRecord = false;
            this.selemp = null;
 
        } else {
            //edit the record
            
                this.statusMessage = 'Record Updated Successfully.';
                    this.loadEmployee();
           
            this.selemp = null;
 
        }
    }

    loadTemplate(emp: Projects) {
        
        if (this.selemp && this.selemp.ID == emp.ID) {
            return this.editTemplate;
        } else {
            return this.readOnlyTemplate;
         }
        
         
 
    }
}
