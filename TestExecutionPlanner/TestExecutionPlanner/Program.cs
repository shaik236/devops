using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Xml;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Data.OleDb;
using System.IO;
using System.Net.Mail;

namespace TestExecutionPlanner
{
    public class TestCase
    {
        public string ID { set; get; }
        public string ExecuteFlag { set; get; }
        public string Priority { set; get; }
        //public string Duration { set; get; }
        public string AreaPath { set; get; }
        public string Dependent { set; get; }

        public string TestVM { set; get; }

        public TestCase(string id, string priority, string executeFlag, string dependent, string testVM)
        {
            this.ID = id;
            this.Priority = priority;
            this.ExecuteFlag = executeFlag;
            this.Dependent = dependent;
            this.TestVM = testVM;
        }


    }

    class Program
    {
        public static string testCaseID = string.Empty;
        public static List<TestCase> testCaseIDList;
        public static string testCaseListFileName;
        public static void Main(string[] args)
        {
            string testItemsGroup = string.Empty;
            string testcasesheet = string.Empty;
            string testcaseListFile = string.Empty;
            string testItemsFilePath = string.Empty;
            string testCaseFileRefFile = string.Empty;
            string option = "";
            string testvm = "";
            bool fetchFromExcel = true;
            string agentName = string.Empty;
            

            //if (args.Length == 0)
            //{
            //    Console.WriteLine("Invalid list of arguments");
            //    return;
            //}
            //else
            //    Console.WriteLine("Arguments count is - " + args.Length);

            foreach (string arg in args)
            {
                string val = arg.ToLower();
                string actualVal = arg;

                if (val.Contains("testcaseid"))
                {
                    testCaseID = actualVal.Split(':')[1];
                    testCaseIDList.Add(new TestCase(testCaseID, "", "", "",""));
                    fetchFromExcel = false;
                    Console.WriteLine("TestCase ID passed is - " + testCaseID);
                }

                if (val.Contains("testitemspath"))
                {
                    testItemsFilePath = val.Replace("-testitemspath:", "");
                    Console.WriteLine("Test Items Path passed is - " + testItemsFilePath);
                }

                if (val.Contains("testitemsgroup"))
                {
                    testItemsGroup = actualVal.Split(':')[1];
                    Console.WriteLine("Test Items Group passed is - " + testItemsGroup);
                }

                if (val.Contains("testcaselistfile"))
                {
                    testcaseListFile = val.Replace("-testcaselistfile:", "");
                    Console.WriteLine("Test case list file passed is - " + testcaseListFile);
                    testCaseListFileName = testcaseListFile;
                    if (fetchFromExcel)
                    testCaseIDList = GetTestCasesFromTestList(testCaseListFileName);
                }

                if (val.Contains("testcasesheet"))
                {
                    testcasesheet = actualVal.Split(':')[1];
                    Console.WriteLine("Test case list file passed is - " + testcasesheet);
                }

                if (val.Contains("testcasefilereffile"))
                {
                    testCaseFileRefFile = actualVal.Split(':')[1];
                    Console.WriteLine("Test case file ref file passed is - " + testCaseFileRefFile);
                }

                if (val.Contains("option"))
                {
                    option = actualVal.Split(':')[1];
                    //Console.WriteLine("option passed is - " + option);
                }

                if (val.Contains("agentname"))
                {
                    agentName = actualVal.Split(':')[1];
                    //Console.WriteLine("agentname passed is - " + agentName);
                }

                if (val.Contains("vmid"))
                {
                    testvm = actualVal.Split(':')[1];
                    Console.WriteLine("vm passed is - " + testvm);
                }

            }

            if (string.IsNullOrEmpty(option))
            {
                option = ConfigurationManager.AppSettings["option"];
            }

            if (string.IsNullOrEmpty(testcaseListFile) && option != "GetAgentTestSet" && option != "VerifyAgentsOnline" && option != "AddFileReferencesForTestCase" && option != "PrepareSanityTestList")
            {
                testcaseListFile = ConfigurationManager.AppSettings["TestCasesListFile"];
                testCaseListFileName = testcaseListFile;
                if (fetchFromExcel)
                    testCaseIDList = GetTestCasesFromTestList(testCaseListFileName);
            }

            

            //if (fetchFromExcel)
            //{
            //    testCaseIDList = GetTestCasesFromTestList(testCaseListFileName);
            //}

            if (string.IsNullOrEmpty(testItemsFilePath))
            {
                testItemsFilePath = ConfigurationManager.AppSettings["TestItemsPath"];
            }

            if (string.IsNullOrEmpty(testItemsGroup))
            {
                testItemsGroup = ConfigurationManager.AppSettings["TestItemsGroup"];
            }

            if (string.IsNullOrEmpty(testCaseFileRefFile))
            {
                testCaseFileRefFile = ConfigurationManager.AppSettings["testCaseFileRefFile"];
            }

            

            if (string.IsNullOrEmpty(testvm))
            {
                testvm = ConfigurationManager.AppSettings["TestVM"];
            }


            switch (option)
            {
                case "UpdateTestItemsList": UpdateTestItemsList(testItemsFilePath, testItemsGroup, testvm);
                                            break;

                case "UpdateTestItemsListByGroup":
                    UpdateTestItemsListByGroup(testItemsFilePath, testvm);
                    break;

                case "UpdateTestItemsTextFile":
                    UpdateTestItemsTextFile(testItemsFilePath, testvm);
                    break;


                case "PrepareSanityTestList":
                    PrepareSanityTestList();
                    break;

                case "AddFileReferencesForTestCase":
                    AddFileReferencesForTestCase(testCaseFileRefFile, "89228"); //AddFileReferencesForTestCase(testCaseFileRefFile, testCaseID);
                    break;

                case "GetAgentTestSet":
                    GetAgentTestSet(agentName);
                    break;

                case "VerifyAgentsOnline":
                    VerifyAgentsOnline();
                    break;

                default: Console.WriteLine("Incorrect Option");
                            break;

            }
        }
        public static List<TestCase> GetTestCasesFromTestList(string testCaseListFile)
        {
            Console.WriteLine("Read1");
            List<TestCase> testList = new List<TestCase>();

            var fileName = testCaseListFile;
            var connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=NO;TypeGuessRows=0;ImportMixedTypes=Text\"";
            var ds = new DataSet(); 
            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                var sheets = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TestList" });
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT * FROM [TestList$]";

                    var adapter = new OleDbDataAdapter(cmd);
                    adapter.Fill(ds);
                }
                
                conn.Close();
            }

                   
            int iter = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if(iter != 0)
                    testList.Add(new TestCase(Convert.ToString(dr.ItemArray[0]), Convert.ToString(dr.ItemArray[1]), Convert.ToString(dr.ItemArray[2]), Convert.ToString(dr.ItemArray[3]), Convert.ToString(dr.ItemArray[4])));

                iter++;
            }


            //var myData = ds.Tables[0].AsEnumerable().Select(r => new TestCase(r.Field<string>("TestCaseID"), "", "", "", ""));
            //{
            //    ID = r.Field<string>("TestCaseID"),
            //    Age = r.Field<int>("Age")
            //});
            //var list = myData.ToList();


            //List<TestCase> testList = new List<TestCase>();

            //if (testCaseListFile.Contains("xls") || testCaseListFile.Contains("xlsx") || testCaseListFile.Contains("csv"))
            //{
            //    Excel.Application xlApp;
            //    Excel.Workbook xlWorkBook;
            //    Excel.Worksheet xlWorkSheet;
            //    Excel.Range range;

            //    string str;
            //    double db = 0;
            //    int rCnt;
            //    int cCnt;
            //    int rw = 0;
            //    int cl = 0;
            //    string testCaseID;
            //    string priority = "";
            //    string executeFlag = "";
            //    string dependent = "";
            //    string testVM = "";

            //    xlApp = new Excel.Application();
            //    xlWorkBook = xlApp.Workbooks.Open(testCaseListFile, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            //    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets["TestList"];

            //    range = xlWorkSheet.UsedRange;
            //    rw = range.Rows.Count;
            //    cl = range.Columns.Count;



            //    for (rCnt = 2; rCnt <= rw; rCnt++)
            //    {

            //        if (((range.Cells[rCnt, 1] as Excel.Range).Value2) != null)
            //            testCaseID = ((range.Cells[rCnt, 1] as Excel.Range).Value2).ToString();
            //        else
            //            continue;

            //        if (((range.Cells[rCnt, 2] as Excel.Range).Value2) != null)
            //            priority = ((range.Cells[rCnt, 2] as Excel.Range).Value2).ToString();

            //        if (((range.Cells[rCnt, 3] as Excel.Range).Value2) != null)
            //            executeFlag = ((range.Cells[rCnt, 3] as Excel.Range).Value2).ToString();

            //        if (((range.Cells[rCnt, 4] as Excel.Range).Value2) != null)
            //            dependent = ((range.Cells[rCnt, 4] as Excel.Range).Value2).ToString();

            //        if (((range.Cells[rCnt, 5] as Excel.Range).Value2) != null)
            //            testVM = ((range.Cells[rCnt, 5] as Excel.Range).Value2).ToString();

            //        if (executeFlag == "Y")
            //        {
            //            testList.Add(new TestCase(testCaseID, priority, executeFlag, dependent, testVM));

            //        }

            //    }

            //    xlWorkBook.Close(true, null, null);
            //    xlApp.Quit();

            //    Marshal.ReleaseComObject(xlWorkSheet);
            //    Marshal.ReleaseComObject(xlWorkBook);
            //    Marshal.ReleaseComObject(xlApp);
            //}
            Console.WriteLine("Read11");

            return testList;
        }
        public static void UpdateTestItemsList(string testItemsFilePath, string testItemsGroup, string testVM ="")
        {
            Console.WriteLine("Test Items Path picked is - " + testItemsFilePath);
            Console.WriteLine("Test Items Group picked is - " + testItemsGroup);
            Console.WriteLine("Test VM picked is - " + testVM);


            List<TestCase> testList = new List<TestCase>();

            if (!(string.IsNullOrEmpty(testVM)))
            {
                
                foreach (TestCase test in testCaseIDList)
                {                    
                    if(test.TestVM == testVM && test.ExecuteFlag == "Y")
                    {
                        testList.Add(test);                        
                    }
                }
            }
            else
                testList = testCaseIDList;

            Console.WriteLine("Test to be updated count is - " + testList.Count);

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(testItemsFilePath);
                var tmp1 = doc.SelectNodes("//testItems/children/testItem");
                foreach (XmlNode item in tmp1)
                {
                    //Console.WriteLine(item.Attributes[0].Value);
                    if (item.Attributes[0].Value == testItemsGroup)
                    {
                        Console.WriteLine("Test group matched and is - " + testItemsGroup);
                        var tmp2 = item.SelectNodes("children/testItem");

                        foreach (XmlNode itemsub in tmp2)
                        {
                            itemsub.Attributes["enabled"].Value = "False";
                        }

                        foreach (TestCase testID in testList)
                        {

                            testCaseID = testID.ID;

                            foreach (XmlNode itemsub in tmp2)
                            {
                                //Console.WriteLine(itemsub.Attributes[0].Value);
                                if (itemsub.Attributes[0].Value.Contains(testCaseID))
                                {
                                    Console.WriteLine(itemsub.Attributes[0].Value);
                                    itemsub.Attributes["enabled"].Value = "True";
                                }
                            }
                        }

                        if (item.Attributes["enabled"].Value == "False")
                            item.Attributes["enabled"].Value = "True";
                    }
                    else
                    {
                        if (item.Attributes["enabled"].Value == "True")
                            item.Attributes["enabled"].Value = "False";
                    }

                }

                doc.Save(testItemsFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception while Test Items list update - " + ex.Message);
            }

        }
        public static void UpdateTestItemsListByGroup(string testItemsFilePath, string testVM = "")
        {
            Console.WriteLine("Test Items Path picked is - " + testItemsFilePath);
            
            Console.WriteLine("Test VM picked is - " + testVM);


            List<TestCase> testList = new List<TestCase>();

            if (!(string.IsNullOrEmpty(testVM)))
            {

                foreach (TestCase test in testCaseIDList)
                {
                    if (test.TestVM == testVM && test.ExecuteFlag == "Y")
                    {
                        testList.Add(test);
                    }
                }
            }
            else
                testList = testCaseIDList;

            Console.WriteLine("Test to be updated count is - " + testList.Count);

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(testItemsFilePath);
                var tmp1 = doc.SelectNodes("//testItems/children/testItem");

                foreach (XmlNode item in tmp1)
                {
                    bool ismatched = false;

                    foreach (TestCase testID in testList)
                    {
                        if (item.Attributes[0].Value == testID.ID)
                        {
                            ismatched = true;

                        }
                    }

                    if(ismatched)
                    {
                        if (item.Attributes["enabled"].Value == "False")
                            item.Attributes["enabled"].Value = "True";

                        var tmp2 = item.SelectNodes("children/testItem");

                        foreach (XmlNode itemsub in tmp2)
                        {
                            itemsub.Attributes["enabled"].Value = "True";
                        }

                    }
                    else
                    {
                        if (item.Attributes["enabled"].Value == "True")
                            item.Attributes["enabled"].Value = "False";
                    }


               

                }
                

                doc.Save(testItemsFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception while Test Items list update - " + ex.Message);
            }

        }
        public static void UpdateTestItemsTextFile(string testItemsFilePath, string testVM = "")
        {
            Console.WriteLine("Test Items Path picked is - " + testItemsFilePath);
            
            List<TestCase> testList = new List<TestCase>();

            if (!(string.IsNullOrEmpty(testVM)))
            {

                foreach (TestCase test in testCaseIDList)
                {
                    if (test.TestVM == testVM && test.ExecuteFlag == "Y")
                    {
                        testList.Add(test);
                    }
                }
            }
            else
                testList = testCaseIDList;

            File.WriteAllText(testItemsFilePath, String.Empty);
            int inc = 0;
            foreach (TestCase testID in testList)
            {

                testCaseID = testID.ID;
                Console.WriteLine("Test to be added is - " + testCaseID);
                if(inc == testList.Count - 1)
                File.AppendAllText(testItemsFilePath, string.Format("{0}", testCaseID));
                else
                    File.AppendAllText(testItemsFilePath, string.Format("{0}{1}", testCaseID, Environment.NewLine));

                inc++;

            }
            
        

        }
        public static void PrepareRerunTestList()
        {
            List<string> failedTestsList = new List<string>();

            if (ConfigurationManager.AppSettings["VSTSUrl"] == "VSTS")
                failedTestsList = GetFailedTests();
            else
            {
                //failedTestsList
            }

        }
        public static void PrepareSanityTestList()
        {
            List<string> SanityTestsList = new List<string>();

            List<string> failedTestsList = GetFailedTests();
            List<string> changeRelatedTestsList = GetTestsBasedOnChangeSet();
            List<string> ExisitingSanityTestsList = GetSanityTests();
            List<string> RegressionTestsList = new List<string>();

            List<TestCase> SanityTests = new List<TestCase>();

            SanityTestsList = ExisitingSanityTestsList;

            foreach (string test in failedTestsList)
            {
                if (!(SanityTestsList.Contains(test)))
                    SanityTestsList.Add(test);
            }

            int testcutoff = 0;

            foreach (string test in changeRelatedTestsList)
            {
                if (!(SanityTestsList.Contains(test)))
                {
                    if (testcutoff < 10)
                    {
                        foreach (TestCase testcase in testCaseIDList)
                        {
                            if (testcase.ID == test && testcase.Priority == "1" && testcase.Dependent == "N")
                                SanityTestsList.Add(test);
                        }
                    }
                    testcutoff++;
                }

            }

            //foreach (TestCase testcase in testCaseIDList)
            //{
            //    if (!(SanityTestsList.Contains(testcase.ID)))
            //        RegressionTestsList.Add(testcase.ID);
            //}

            //foreach (TestCase testcase in testCaseIDList)
            //{
            //    if (SanityTestsList.Contains(testcase.ID))
            //        SanityTests.Add(new TestCase(testcase.ID, testcase.Priority, testcase.ExecuteFlag, testcase.Dependent, testcase.TestVM));
            //}

            //CreateSanitySheet(SanityTests);




            //Create Sanity Sheet
            //Create Regression Sheet
        }
        public static List<string> GetFailedTests()
        {
            RequestExecutor requestExecutor = new RequestExecutor();
            string responseText = requestExecutor.MakeRequest("GET", "test/Plans/" + ConfigurationManager.AppSettings["TestPlanID"] + "/Suites/" + ConfigurationManager.AppSettings["TestSuiteID"] + "/points?" + "api-version=5.0-preview.2");
            var results = Newtonsoft.Json.JsonConvert.DeserializeObject<TestExecutionPlanner.TestPoints>(responseText);

            List<string> failedTestsList = new List<string>();

            foreach (var item in results.value)
            {
                if (item.outcome == "Failed")
                {
                    failedTestsList.Add(item.testCase.id);
                    Console.WriteLine("Failed Test Case id - " + item.testCase.id);
                }
            }

            return failedTestsList;
        }
        public static List<string> GetTestsBasedOnChangeSet()
        {
            RequestExecutor requestExecutor = new RequestExecutor();

            List<string> changesTestsList = new List<string>();

            //Get Previous and Current BUILD IDs for fetching changesets
            string responseText = requestExecutor.MakeRequest("GET", "build/builds?definitions=" + ConfigurationManager.AppSettings["BuildDefinitionID"] + "&minTime=" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + "&maxTime=" + DateTime.Now.ToString("yyyy-MM-dd") + "&" + "api-version=5.0-preview.2");
            var results = Newtonsoft.Json.JsonConvert.DeserializeObject<TestExecutionPlanner.TestPoints>(responseText);
            string currentBuildID = string.Empty;
            string previousBuildID = string.Empty;
            int buildIterator = 0;
            foreach (var item in results.value)
            {
                if (buildIterator == 0)
                    currentBuildID = item.id.ToString();

                if (buildIterator == 1)
                    previousBuildID = item.id.ToString();

                foreach (string tag in item.tags)
                {
                    if (tag == "BVTPassed")
                        previousBuildID = item.id.ToString();
                }

                buildIterator++;
            }

            //Get Changesets between two builds
            responseText = requestExecutor.MakeRequest("GET", "build/changes?fromBuildId=" + previousBuildID + "&toBuildId=" + currentBuildID + "&" + "api-version=5.0-preview.2");
            var changeSets = Newtonsoft.Json.JsonConvert.DeserializeObject<TestExecutionPlanner.ChangeSets>(responseText);
            List<string> changeSetIDs = new List<string>();
            foreach (var item in changeSets.value)
            {
                changeSetIDs.Add(item.id);
                Console.WriteLine("Change Set Id is - " + item.id);
            }

            List<string> changedFileList = new List<string>();
            List<string> addedFileList = new List<string>();
            List<string> deletedFileList = new List<string>();

            foreach (string Cid in changeSetIDs)
            {
                responseText = requestExecutor.MakeRequest("GET", "tfvc/changesets/" + Cid.Replace("C", "") + "/changes");
                var changesDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<TestExecutionPlanner.Changes>(responseText);

                foreach (var item in changesDetail.value)
                {
                    if (item.changeType == "add")
                        addedFileList.Add(item.item.path);

                    if (item.changeType == "delete")
                        deletedFileList.Add(item.item.path);

                    if (!(changedFileList.Contains(item.item.path)))
                    {
                        changedFileList.Add(item.item.path);
                        Console.WriteLine(item.item.path);
                    }
                }
            }


            foreach (string filePath in changedFileList)
            {
                List<string> testsForFile = new List<string>();

                testsForFile = GetTestsForFile(filePath);
                foreach (string test in testsForFile)
                {
                    if (!(changesTestsList.Contains(test)))
                    {
                        changesTestsList.Add(test);
                        Console.WriteLine("Identified Tests for Change is - " + test);
                    }
                }
            }

            //Email Added and other pieces of code

            string changedFileListMsg = string.Empty;
            string addedFileListMsg = string.Empty;
            string deletedFileListMsg = string.Empty;
            string AffectedTestsMsg = string.Empty;

            foreach (string filePath in changedFileList)
            {
                changedFileListMsg = changedFileListMsg + filePath + "\n";
            }

            foreach (string testch in changesTestsList)
            {
                AffectedTestsMsg = AffectedTestsMsg + testch + "\n";
            }

            foreach (string filePath in addedFileList)
            {
                addedFileListMsg = addedFileListMsg + filePath + "\n";
            }

            foreach (string filePath in deletedFileList)
            {
                deletedFileListMsg = deletedFileListMsg + filePath + "\n";
            }


            SendReport(changedFileListMsg, addedFileListMsg, deletedFileListMsg, AffectedTestsMsg, currentBuildID);


            return changesTestsList;
        }
        public static List<string> GetSanityTests()
        {
            List<string> sanityTests = new List<string>();

            RequestExecutor requestExecutor = new RequestExecutor();
            string responseText = requestExecutor.MakeRequest("GET", "test/Plans/" + ConfigurationManager.AppSettings["TestPlanID"] + "/Suites/" + ConfigurationManager.AppSettings["TestSuiteID"] + "/points?" + "api-version=5.0-preview.2");
            var results = Newtonsoft.Json.JsonConvert.DeserializeObject<TestExecutionPlanner.TestPoints>(responseText);

            foreach (var item in results.value)
            {
                responseText = requestExecutor.MakeRequest("GET", "wit/workitems/" + item.testCase.id + "?" + "api-version=5.0-preview.3");
                responseText = responseText.Replace("System.Tags", "SystemTags");
                var Wresults = Newtonsoft.Json.JsonConvert.DeserializeObject<TestExecutionPlanner.WorkItem>(responseText);

                sanityTests.Add(item.testCase.id.ToString());

                //if (!(string.IsNullOrEmpty(Wresults.fields.SystemTags)))
                //{

                //    if (Wresults.fields.SystemTags.Contains("BVT") || Wresults.fields.SystemTags.Contains("Sanity") || Wresults.fields.SystemTags.Contains("Smoke"))
                //    {
                //        sanityTests.Add(item.testCase.id.ToString());
                //        Console.WriteLine(Wresults.fields.SystemTags);
                //    }
                //}


            }

            return sanityTests;
        }
        public static void SendReport(string changedFileListMsg, string addedFileListMsg, string deletedFileListMsg, string AffectedTestsMsg, string build = "")
        {
            string to = "sshaik7@slb.com";
            string from = "sshaik7@slb.com";
            string server = "gateway.mail.slb.com";

            string val = System.Configuration.ConfigurationManager.AppSettings["MailReceipents"];


            MailMessage message = new MailMessage(from, val);

            message.Subject = "Imapcted Automation Tests for Build - " + build;

            if (string.IsNullOrEmpty(changedFileListMsg))
                changedFileListMsg = "NA";

            if (string.IsNullOrEmpty(addedFileListMsg))
                addedFileListMsg = "NA";

            if (string.IsNullOrEmpty(deletedFileListMsg))
                deletedFileListMsg = "NA";

            if (string.IsNullOrEmpty(AffectedTestsMsg))
                AffectedTestsMsg = "NA";

            string bodymessage = "Changed File List from Build: \n" + changedFileListMsg + "\n\nAdded Files list: \n" + addedFileListMsg + "\n\nDeleted Files list: \n" + deletedFileListMsg + "\n\nImpacted Automation Tests list: \n" + AffectedTestsMsg + "\n";



            message.Body = bodymessage;

            message.Body = message.Body + "\n\nThanks,\nAutomation";


            try
            {
                //message.Body = @"Test Email";
                SmtpClient client = new SmtpClient(server);
                // Credentials are necessary if the server requires the client 
                // to authenticate before it will send e-mail on the client's behalf.
                client.UseDefaultCredentials = true;
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                            ex.ToString());
            }
        }
        public static void AddFileReferencesForTestCase(string testCaseFileRefPath, string testCaseId)
        {
            //"C:\Users\sshaik7\Desktop\VSTS\TestExecutionPlanner\Test_17612_CoveredFiles.txt"
            string line;
            int counter = 0;
            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader(testCaseFileRefPath);
            while ((line = file.ReadLine()) != null)
            {
                if (counter > 0)
                {
                    if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                    {
                        Console.WriteLine("Blank line");
                    }
                    else
                    {
                        //System.Console.WriteLine(line);
                        Console.WriteLine(line.Split('\t')[0]);
                        Console.WriteLine(line.Split('\t')[0].Split('\\')[line.Split('\t')[0].Split('\\').Length - 1]);
                        string filePath = line.Split('\t')[0];
                        string newfilePath = string.Empty;
                        try
                        {
                            newfilePath = filePath.Substring(filePath.IndexOf("\\", 13)).Replace("\\", "/");
                            newfilePath = "$/InterACT Core" + newfilePath;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Non readable file reference - " + line);
                            continue;
                        }
                        if (!(VerifyFileRecordExists(newfilePath, line.Split('\t')[0].Split('\\')[line.Split('\t')[0].Split('\\').Length - 1], testCaseId)))
                        {
                            AddFileRecord(newfilePath.ToLower().Trim(), line.Split('\t')[0].Split('\\')[line.Split('\t')[0].Split('\\').Length - 1], testCaseId);
                        }

                    }
                }
                counter++;
            }

        }
        public static void CreateSanitySheet(List<TestCase> sanityList)
        {

            DeleteSanitySheet();

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;

            Excel.Worksheet xlNewSheet = null;
            Excel.Sheets xlSheets = null;

            string str;
            double db = 0;
            int rCnt;
            int cCnt;
            int rw = 0;
            int cl = 0;
            string testCaseID;
            string priority = "";
            string executeFlag = "";
            string dependent = "";
            string worksheetName = "AutoSanityPack";

            xlApp = new Excel.Application();

            try
            {
                xlApp.DisplayAlerts = false;

                //xlWorkBook = xlApp.Workbooks.Open(testCaseListFileName, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlWorkBook = xlApp.Workbooks.Open(@"C:\Users\sshaik7\Desktop\VSTS\TestExecutionPlanner\TestExecutionList.xlsx", 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlSheets = xlWorkBook.Sheets as Excel.Sheets;

                xlNewSheet = (Excel.Worksheet)xlSheets.Add(xlSheets[1], Type.Missing, Type.Missing, Type.Missing);
                xlNewSheet.Name = worksheetName;

                xlNewSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlNewSheet.Select();

                xlNewSheet.Cells[1, 1] = "TestCaseID";
                xlNewSheet.Cells[1, 2] = "Priority";
                xlNewSheet.Cells[1, 3] = "ExecuteFlag";
                xlNewSheet.Cells[1, 4] = "Dependent";
                xlNewSheet.Cells[1, 5] = "TestVM";

                int rownum = 2;
                foreach(TestCase item in sanityList)
                {
                    xlNewSheet.Cells[rownum, 1] = item.ID;
                    xlNewSheet.Cells[rownum, 2] = item.Priority;
                    xlNewSheet.Cells[rownum, 3] = item.ExecuteFlag;
                    xlNewSheet.Cells[rownum, 4] = item.Dependent;
                    xlNewSheet.Cells[rownum, 5] = "";
                    rownum++;
                }

                //xlNewSheet.Cells[1, 1] = "Sample test data";
                //xlNewSheet.Cells[1, 2] = "Date : " + DateTime.Now.ToShortDateString();

                xlWorkBook.Save();
                xlWorkBook.Close(Type.Missing, Type.Missing, Type.Missing);
                xlApp.Quit();

                Marshal.ReleaseComObject(xlSheets);
                Marshal.ReleaseComObject(xlNewSheet);
                Marshal.ReleaseComObject(xlWorkBook);
                Marshal.ReleaseComObject(xlApp);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                xlApp.Quit();
                Marshal.ReleaseComObject(xlSheets);
                Marshal.ReleaseComObject(xlNewSheet);
                //Marshal.ReleaseComObject(xlWorkBook);
                Marshal.ReleaseComObject(xlApp);
            }
        }
        public static void DeleteSanitySheet()
        {
           
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Sheets xlSheets = null;

                xlApp = new Excel.Application();
                xlApp.DisplayAlerts = false;
            try
            {


                //xlWorkBook = xlApp.Workbooks.Open(testCaseListFileName, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlWorkBook = xlApp.Workbooks.Open(@"C:\Users\sshaik7\Desktop\VSTS\TestExecutionPlanner\TestExecutionList.xlsx", 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlSheets = xlWorkBook.Sheets as Excel.Sheets;

                foreach (Excel.Worksheet sheet in xlWorkBook.Sheets)
                {
                    // Check the name of the current sheet
                    if (sheet.Name == "AutoSanityPack")
                    {
                        xlWorkBook.Sheets["AutoSanityPack"].Delete();
                        break; // Exit the loop now
                    }
                }

                xlWorkBook.Save();
                xlWorkBook.Close(Type.Missing, Type.Missing, Type.Missing);
                xlApp.Quit();

                Marshal.ReleaseComObject(xlSheets);
                Marshal.ReleaseComObject(xlWorkBook);
                Marshal.ReleaseComObject(xlApp);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);                
                xlApp.Quit();
                Marshal.ReleaseComObject(xlSheets);
                
                Marshal.ReleaseComObject(xlApp);
            }
        }
        public static DataTable ConvertListToDataTable(List<TestCase> testslist)
        {
            // New table.
            DataTable table = new DataTable();
            
            table.Columns.Add("TestCaseID", typeof(string));
            table.Columns.Add("Priority", typeof(string));
            table.Columns.Add("ExecuteFlag", typeof(string));
            table.Columns.Add("Dependent", typeof(string));
            table.Columns.Add("TestVM", typeof(string));
            

            // Add rows.
            foreach (TestCase test in testslist)
            {
                table.Rows.Add(test.ID, test.Priority, test.ExecuteFlag, test.Dependent, "");
            }

            return table;
        }
        public static void CreateRegressionSheet()
        {

        }
        public static List<string> GetTestsForFile(string filePath)
        {
            List<string> tests = new List<string>();

            SqlConnection con = new SqlConnection("Data Source=localhost;Initial Catalog=InteractCodeCoverage;User ID=sa;Password=summer90");
            string sql = "select distinct TestCaseId from [InteractCodeCoverage].[dbo].[FileToTestCaseAssociation] a, [InteractCodeCoverage].[dbo].[FileList] b where a.FileId = b.FileId and b.FilePath = '" + filePath.ToLower().Trim() + "'; ";

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    tests.Add(dr["TestCaseId"].ToString());
                }

                con.Close();
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                con.Close();
            }
            return tests;
        }
        public static void VerifyAgentsOnline()
        {
            RequestExecutor requestExecutor = new RequestExecutor();

            List<string> agentsList = new List<string>();
            List<int> agentsIdsList = new List<int>();

            //Get Previous and Current BUILD IDs for fetching changesets
            string responseText = requestExecutor.MakeRequest("GET", "distributedtask/pools/9/agents?includeCapabilities=true&includeAssignedRequest=true");
            var results = Newtonsoft.Json.JsonConvert.DeserializeObject<TestExecutionPlanner.Agent>(responseText);
            foreach (var item in results.value)
            {

                if (item.userCapabilities == null)
                    continue;

                if(item.userCapabilities.AutoGroup == "REG1")
                {
                    if (item.status == "online")
                    {
                        agentsList.Add(item.name);
                    }
                    else
                        Console.WriteLine("Agent - " + item.name + " is offline, make it online");
                   
                }
                

            }

            if(agentsList.Count < 4)
            {
                Console.WriteLine("Agents available are not enough, avail count is - " + agentsList.Count);
                Environment.Exit(-1);
            }
        }
        public static string GetAgentTestSet(string agentName)
        {
            RequestExecutor requestExecutor = new RequestExecutor();
                        
            string TestSet = string.Empty;

            //Get Previous and Current BUILD IDs for fetching changesets
            string responseText = requestExecutor.MakeRequest("GET", "distributedtask/pools/9/agents?includeCapabilities=true&includeAssignedRequest=true");
            var results = Newtonsoft.Json.JsonConvert.DeserializeObject<TestExecutionPlanner.Agent>(responseText);
            foreach (var item in results.value)
            {
                if (item.name == agentName)
                {

                    TestSet = item.userCapabilities.TestSet;
                    Console.WriteLine(TestSet);
                    return TestSet;
                }
            }

            return TestSet;
        }
        public static bool VerifyFileRecordExists(string filePath, string fileName, string testCaseId)
        {
            SqlConnection con = new SqlConnection("Data Source=localhost;Initial Catalog=InteractCodeCoverage;User ID=sa;Password=summer90");
            string sql = "SELECT FileId FROM [InteractCodeCoverage].[dbo].[FileList] where FilePath = '" + filePath + "' and FileName = '" + fileName + "';";

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader dr = cmd.ExecuteReader();
                string FileID = "";
                while (dr.Read())
                {
                    FileID = dr["FileId"].ToString();
                }
                dr.Close();
                con.Close();

                if (string.IsNullOrEmpty(FileID))
                {
                    return false;
                }

                sql = "SELECT count(TestCaseId) FROM [InteractCodeCoverage].[dbo].[FileToTestCaseAssociation] where FileId = " + FileID + ";";
                con.Open();
                cmd = new SqlCommand(sql, con);
                Int32 count = (Int32)cmd.ExecuteScalar();
                con.Close();

                if (count > 0)
                {
                    return true;
                }
                else
                    return false;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                con.Close();
                return false;
            }
            
        }
        public static bool AddFileRecord(string filePath, string fileName, string testCaseId)
        {
            SqlConnection con = new SqlConnection("Data Source=localhost;Initial Catalog=InteractCodeCoverage;User ID=sa;Password=summer90");
            string sql = "insert into [InteractCodeCoverage].[dbo].[FileList](FileName,FilePath, RecordCreatedDate) values ('" + fileName + "', '" + filePath + "', GETDATE());";

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.ExecuteNonQuery();

                sql = "SELECT FileId FROM [InteractCodeCoverage].[dbo].[FileList] where FilePath = '" + filePath + "' and FileName = '" + fileName + "';";

                    cmd = new SqlCommand(sql, con);
                    SqlDataReader dr = cmd.ExecuteReader();
                    string FileID = "";
                    while (dr.Read())
                    {
                        FileID = dr["FileId"].ToString();
                    }
                    dr.Close();
                    con.Close();

                    if (string.IsNullOrEmpty(FileID))
                    {
                        return false;
                    }
                    con.Open();
                    sql = "insert into [InteractCodeCoverage].[dbo].[FileToTestCaseAssociation](FileId,TestCaseId, LastUpdateDate) values ('" + FileID + "', '" + testCaseId + "', GETDATE());";
                    cmd = new SqlCommand(sql, con);
                    cmd.ExecuteNonQuery();


                    con.Close();

                    return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                con.Close();
                    return false;
            }
        }
    }


}
