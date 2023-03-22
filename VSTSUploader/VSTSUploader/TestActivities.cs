using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;

namespace VSTSUploader
{
    public class TestResult
    {
        public string ID { set; get; }
        
        public string Outcome { set; get; }
        
        public string PlanName { set; get; }
        public string SuiteName { set; get; }
        public string BuildName { set; get; }
        public string VersionName { set; get; }
        public string ConfigName { set; get; }

        public TestResult(string id, string outcome, string planname, string suitename, string buildname, string versionname, string configname)
        {
            this.ID = id;
            this.Outcome = outcome;
            this.PlanName = planname;
            this.SuiteName = suitename;
            this.VersionName = buildname;
            this.BuildName = versionname;
            this.ConfigName = configname;
        }


    }
    public class TestActivities
    {
        RequestExecutor requestExecutor = new RequestExecutor();

        public string TestRunName = "";

        public void UploadTestResults(string testResultPath, string planname, string suitename, string configname, string buildname)
        {
            try
            {
                TestResultAccessor testsAcessor = new TestResultAccessor();
                
                List<TestResult> testresults = testsAcessor.ReadTests(testResultPath);

                foreach (TestResult testItem in testresults)
                {
                    Console.WriteLine("Test ID is - " + testItem.ID + " and outcome is - " + testItem.Outcome);
                    
                   
                    
                }

                

                Console.WriteLine("Read Tests and count is - " + testresults.Count);

                Console.ReadLine();
                int testPlanID = 0, testSuiteID = 0, buildId = 0, testConfigId = 0;

                if (testresults == null || testresults.Count == 0)
                {
                    throw new Exception("No Test results found");
                }

                if (string.IsNullOrEmpty(planname))
                {
                    planname = testresults[0].PlanName;
                    testPlanID = GetTestPlanID(planname);
                }
                else
                    testPlanID = GetTestPlanID(planname);
                Console.WriteLine("Retrieved Test Plan Id is - " + testPlanID);

                if (testPlanID <= 0)
                    throw new Exception("Plan ID for Plan name - " + planname + " cannot be retrieved");

                if (string.IsNullOrEmpty(suitename))
                {
                    suitename = testresults[0].SuiteName;
                    testSuiteID = GetTestSuiteID(testPlanID, suitename);
                }
                else
                    testSuiteID = GetTestSuiteID(testPlanID, suitename);
                Console.WriteLine("Retrieved Test Suite Id is - " + testSuiteID);

                if (testSuiteID <= 0)
                    throw new Exception("Suite ID for Suite name - " + suitename + " cannot be retrieved");

                if (string.IsNullOrEmpty(buildname))
                {
                    buildname = testresults[0].BuildName;
                    buildId = GetBuildId(buildname);
                }
                else
                    buildId = GetBuildId(buildname);
                Console.WriteLine("Retrieved Build Id is - " + buildId);

                if (buildId <= 0)
                    throw new Exception("Build ID for Build name - " + buildname + " cannot be retrieved");

                if (string.IsNullOrEmpty(configname))
                {
                    configname = testresults[0].ConfigName;
                    testConfigId = GetTestConfigId(configname);
                }
                else
                    testConfigId = GetTestConfigId(configname);
                Console.WriteLine("Retrieved Test Config Id is - " + testConfigId);

                if (testConfigId <= 0)
                    throw new Exception("Config ID for Config name - " + configname + " cannot be retrieved");

                int testcaseID = 0;
                string testOutcome = "failed";
                string testCaseTitle = "";

                string jsonBody = "[";
                var totalRec = testresults.Count;
                var iter = 0;
                foreach (TestResult testItem in testresults)
                {
                    
                    // Console.WriteLine("Key = {0}, Value = {1}", testItem.Key, testItem.Value);
                    testcaseID = Int32.Parse(testItem.ID);
                    testOutcome = testItem.Outcome;
                    //testCaseTitle = testItem.Title;

                    var testPointID = GetTestPoint(testPlanID, testSuiteID, testcaseID);
                    Console.WriteLine("Retrieved Test Point ID is - " + testPointID);
                    var jsonres = FormTestResultJson(testcaseID, testConfigId, testPointID, testOutcome);
                    if(iter < (totalRec -1))
                    jsonBody = jsonBody + "{ \"testCase\":{\"id\":" + testcaseID.ToString() + "}," + "\"testPoint\": { \"id\":" + testPointID + "  }, \"outcome\": \"" + testOutcome + "\", \"configuration\": { \"id\": " + testConfigId + " } },";
                    else
                        jsonBody = jsonBody + "{ \"testCase\":{\"id\":" + testcaseID.ToString() + "}," + "\"testPoint\": { \"id\":" + testPointID + "  }, \"outcome\": \"" + testOutcome + "\", \"configuration\": { \"id\": " + testConfigId + " } }";

                    iter = iter + 1;
                    
                }

                jsonBody = jsonBody + "]";

                TestRunName = "AutomationTestRun_" + DateTime.Now.ToString("ddMMyyyy_hhmmss") + "_" + suitename + "_" + buildId.ToString();

                //var testRunID = 54526;
                var testRunID = CreateTestRun(testPlanID, buildId);
                Console.WriteLine("Created Test Run ID is - " + testRunID);

                AddTestResultToRun(testRunID, jsonBody);

                System.Threading.Thread.Sleep(60000);

                bool runCompleted = MarkRunAsComplete(testRunID, buildId);
                Console.WriteLine("Marked Run Status as completed for Test run ID - " + runCompleted);

                if (!runCompleted)
                {
                    Console.WriteLine("Run not completed");

                    System.Threading.Thread.Sleep(60000);

                    for (int i = 1; i < 10; i++)
                    {
                        System.Threading.Thread.Sleep(10000);
                        runCompleted = MarkRunAsComplete(testRunID, i);
                        Console.WriteLine("Marked Run Status as completed for Test run ID - " + runCompleted);
                        if (runCompleted)
                        {
                            Console.WriteLine("Run completed");
                            break;
                        }
                        else
                            Console.WriteLine("Run not completed and attempt is - " + i);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("**************************************************************************************************************");
                Console.WriteLine(ex.Message);
                Console.WriteLine("**************************************************************************************************************");
            }

        }

        public int CreateTestRun(int testPlanId, int buildId)
        {
            string jsonBody = "{\"name\":\"" + TestRunName + "\"," + "\"isAutomated\":\"false\"," + "\"plan\": {\"id\":" + testPlanId + "}," + "\"build\": {\"id\": " + buildId + " }, \"comment\":\"" + TestRunName + "In Progress" + "\" }";
            string responseText = requestExecutor.MakeRequest("POST", "test/runs?", jsonBody);
            var results = Newtonsoft.Json.JsonConvert.DeserializeObject<VSTSUploader.TestRunobject>(responseText);
            return results.id;
        }

        public int GetTestPlanID(string testPlanName)
        {
            string responseText = requestExecutor.MakeRequest("GET", "test/plans?filterActivePlans=true&");
            var results = Newtonsoft.Json.JsonConvert.DeserializeObject<VSTSUploader.TestPlanobject>(responseText);
            var testPlanID = 0;
            foreach (var item in results.value)
            {
                if (item.name == testPlanName)
                    testPlanID = item.id;
            }

            return testPlanID;
        }

        public int GetBuildId(string buildName)
        {
            string responseText = requestExecutor.MakeRequest("GET", "build/builds?$top=1000&");
            var results = Newtonsoft.Json.JsonConvert.DeserializeObject<VSTSUploader.Buildobject>(responseText);
            var buildId = 0;
            foreach (var item in results.value)
            {
                if (buildName.Contains(item.definition.name) && buildName.Contains(item.buildNumber))
                    buildId = item.id;
            }

            return buildId;
        }

        public int GetTestConfigId(string configName)
        {
            string responseText = requestExecutor.MakeRequest("GET", "test/configurations?$top=90&");
            var results = Newtonsoft.Json.JsonConvert.DeserializeObject<VSTSUploader.TestConfigobject>(responseText);
            var testConfigID = 0;
            foreach (var item in results.value)
            {
                //Console.WriteLine("item name is - " + item.name + " and input name is - " + configName);
                if (item.name == configName)
                    testConfigID = item.id;
            }

            return testConfigID;
        }

        public int GetTestSuiteID(int testPlanId, string testSuiteName)
        {
            string responseText = requestExecutor.MakeRequest("GET", "test/plans/" + testPlanId.ToString() + "/suites?");
            var results = Newtonsoft.Json.JsonConvert.DeserializeObject<VSTSUploader.TestPlanobject>(responseText);
            var testSuiteID = 0;
            foreach (var item in results.value)
            {
                if (item.name == testSuiteName)
                    testSuiteID = item.id;
            }

            return testSuiteID;
        }

        public int GetTestPoint(int testPlanId, int testSuiteId, int testCaseID)
        {
            string responseText = requestExecutor.MakeRequest("GET", "test/plans/" + testPlanId.ToString() + "/suites/" + testSuiteId + "/points?testcaseid=" + testCaseID + "&");
            var results = Newtonsoft.Json.JsonConvert.DeserializeObject<VSTSUploader.TestPointobject>(responseText);
            var testPointID = 0;
            foreach (var item in results.value)
            {
                testPointID = item.id;
            }

            return testPointID;
        }

        public string FormTestResultJson(int testCaseID, int testConfigID, int testPointID, string outcome)
        {
            string jsonBody = "[ { \"testCase\":{\"id\":" + testCaseID.ToString() + "}," + "\"testPoint\": { \"id\":" + testPointID + "  }, \"outcome\": \"Passed\", \"configuration\": { \"id\": " + testConfigID + " } }]";



            return jsonBody;

        }

        public void AddTestResultToRun(int testrunID, string jsonBody)
        {
            string responseText = requestExecutor.MakeRequest("POST", "test/runs/" + testrunID + "/results?", jsonBody);
            var results = Newtonsoft.Json.JsonConvert.DeserializeObject<VSTSUploader.TestResultobject>(responseText);
            if (results.count <= 0)
            {
                throw new Exception();
            }
            else
                Console.WriteLine("Added result");
            
                
        }

        public bool MarkRunAsComplete(int testRunID, int buildId, int iteration = 0)
        {
            string jsonBody = "{\"name\": \"" + TestRunName + "\", \"state\":\"Completed\"," + "\"comment\":\"Automation Run Completed for Build " + buildId + "_" + iteration + "\"}";
            string responseText = requestExecutor.MakeRequest("PATCH", "test/runs/" + testRunID.ToString() + "?", jsonBody);
            Console.WriteLine("******************************************************************************************");
            Console.WriteLine(responseText);
            Console.WriteLine("******************************************************************************************");
            var results = Newtonsoft.Json.JsonConvert.DeserializeObject<VSTSUploader.TestRunobject>(responseText);
            if (results.state != "Completed")
            {
                return false;
            }

            return true;
        }

        public void MarkRunInProgress(int testRunID)
        {
            string jsonBody = "{\"state\":\"Completed\"," + "\"comment\":\"TestComment1Comp\"}";
            string responseText = requestExecutor.MakeRequest("PATCH", "test/runs/" + testRunID.ToString() + "?", jsonBody);
            var results = Newtonsoft.Json.JsonConvert.DeserializeObject<VSTSUploader.TestRunobject>(responseText);
            if (results.state != "InProgress")
            {
                throw new Exception();
            }

        }
    }
}
