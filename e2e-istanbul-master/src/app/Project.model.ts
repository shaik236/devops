export class Project {
    constructor(
        public ID: string,
        publicProjectName: string,
        public DisplayName: string,
        public URL: string,
        public  PAT: string,
        public PATExpiryDate: string,
        public TestPlanID: string,
        public TestSuiteID: string,
        public BuildDefinitionID: string,
        public ReleaseDefinitionID: string) { }
}