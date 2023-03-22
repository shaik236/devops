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

namespace VSTSUploader
{
    public class TestRunobject
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public bool isAutomated { get; set; }
        public string iteration { get; set; }
        public Owner owner { get; set; }
        public Project project { get; set; }
        public DateTime startedDate { get; set; }
        public string state { get; set; }
        public Plan plan { get; set; }
        public string postProcessState { get; set; }
        public int totalTests { get; set; }
        public int incompleteTests { get; set; }
        public int notApplicableTests { get; set; }
        public int passedTests { get; set; }
        public int unanalyzedTests { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime lastUpdatedDate { get; set; }
        public Lastupdatedby lastUpdatedBy { get; set; }
        public int revision { get; set; }
        public string comment { get; set; }
        public Runstatistic[] runStatistics { get; set; }
        public string webAccessUrl { get; set; }
    }

    public class TestPlanobject
    {
        public Value[] value { get; set; }
        public int count { get; set; }
    }

    public class TestPointobject
    {
        public Value[] value { get; set; }
        public int count { get; set; }
    }

    public class TestResultobject
    {
        public int count { get; set; }
        public Value[] value { get; set; }
    }
    
    public class Testrun
    {
        public string id { get; set; }
    }

    public class Buildobject
    {
        public int count { get; set; }
        public Value[] value { get; set; }
    }

    public class Value
    {
        public int id { get; set; }
        public int rev { get; set; }
        public Fields fields { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public object[] tags { get; set; }
        public object[] validationResults { get; set; }
        public Plan[] plans { get; set; }
        public Triggerinfo triggerInfo { get; set; }
        public string buildNumber { get; set; }
        public string status { get; set; }
        public string result { get; set; }
        public DateTime queueTime { get; set; }
        public DateTime startTime { get; set; }
        public DateTime finishTime { get; set; }
        public Definition definition { get; set; }
        public int buildNumberRevision { get; set; }
        public string sourceBranch { get; set; }
        public string sourceVersion { get; set; }
        public Queue queue { get; set; }
        public string priority { get; set; }
        public string reason { get; set; }
        public Requestedfor requestedFor { get; set; }
        public Requestedby requestedBy { get; set; }
        public DateTime lastChangedDate { get; set; }
        public Lastchangedby lastChangedBy { get; set; }
        public Orchestrationplan orchestrationPlan { get; set; }
        public Logs logs { get; set; }
        public Repository repository { get; set; }
        public bool keepForever { get; set; }
        public bool retainedByRelease { get; set; }
        public object triggeredByBuild { get; set; }
        public string description { get; set; }
        public string state { get; set; }
    }

    public class Fields
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public Project project { get; set; }
        public Area area { get; set; }
        public string iteration { get; set; }
        public object owner { get; set; }
        public int revision { get; set; }
        public string state { get; set; }
        public Rootsuite rootSuite { get; set; }
        public string clientUrl { get; set; }
        public string _14b4state { get; set; }
        public Assignedto assignedTo { get; set; }
        public bool automated { get; set; }
        public Configuration configuration { get; set; }
        public Lasttestrun lastTestRun { get; set; }
        public Lastresult lastResult { get; set; }
        public string outcome { get; set; }
        public string lastResultState { get; set; }
        public Testcase testCase { get; set; }
        public Workitemproperty[] workItemProperties { get; set; }
        public Lastresultdetails lastResultDetails { get; set; }
        public string lastRunBuildNumber { get; set; }
        public Testrun testRun { get; set; }
        public int priority { get; set; }
        public Lastupdatedby lastUpdatedBy { get; set; }
        public string automatedTestName { get; set; }
        public int rev { get; set; }
        public Fields fields { get; set; }
    }
    public class Self
    {
        public string href { get; set; }
    }

    public class Web
    {
        public string href { get; set; }
    }

    public class Timeline
    {
        public string href { get; set; }
    }

    public class Properties
    {
    }

    public class Triggerinfo
    {
    }

    public class Definition
    {
        public object[] drafts { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string uri { get; set; }
        public string path { get; set; }
        public string type { get; set; }
        public string queueStatus { get; set; }
        public int revision { get; set; }
        public Project project { get; set; }
    }

    public class Queue
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public Pool pool { get; set; }
    }

    public class Pool
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Requestedfor
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public _Links1 _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class _Links1
    {
        public Avatar avatar { get; set; }
    }

    public class Avatar
    {
        public string href { get; set; }
    }

    public class Requestedby
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public _Links2 _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class _Links2
    {
        public Avatar1 avatar { get; set; }
    }

    public class Avatar1
    {
        public string href { get; set; }
    }

    public class Lastchangedby
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public _Links3 _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class _Links3
    {
        public Avatar2 avatar { get; set; }
    }

    public class Avatar2
    {
        public string href { get; set; }
    }

    public class Orchestrationplan
    {
        public string planId { get; set; }
    }

    public class Logs
    {
        public int id { get; set; }
        public string type { get; set; }
        public string url { get; set; }
    }

    public class Repository
    {
        public string id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public object clean { get; set; }
        public bool checkoutSubmodules { get; set; }
    }

    public class Plan
    {
        public string planId { get; set; }
    }

    public class Owner
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public _Links _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class _Links
    {
        public Avatar avatar { get; set; }
    }

    

    public class Project
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Area
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Rootsuite
    {
        public string id { get; set; }
    }
       
    public class Lastupdatedby
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public _Links1 _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }
    
    public class Runstatistic
    {
        public string state { get; set; }
        public string outcome { get; set; }
        public int count { get; set; }
    }

    public class Assignedto
    {
        public string displayName { get; set; }
        public string id { get; set; }
    }

    public class Configuration
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Lasttestrun
    {
        public string id { get; set; }
    }

    public class Lastresult
    {
        public string id { get; set; }
    }

    public class Testcase
    {
        public string id { get; set; }
    }

    public class Lastresultdetails
    {
        public int duration { get; set; }
        public DateTime dateCompleted { get; set; }
        public Runby runBy { get; set; }
    }

    public class Runby
    {
        public string displayName { get; set; }
        public string id { get; set; }
    }

    public class Workitemproperty
    {
        public Workitem workItem { get; set; }
    }

    public class Workitem
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class TestConfigobject
    {
        public Value[] value { get; set; }
        public int count { get; set; }
    }

    public class RequestExecutor
    {
        
        public string MakeRequest(string requestType, string apiparam, string bodyparam = "")
        {
            HttpWebResponse response;
            string responseText = "";
            bool responseType = false;


            string apiParam = ConfigurationManager.AppSettings["VSTSUrl"] + apiparam + "api-version=5.0-preview.2";

            if (bodyparam.Equals(string.Empty))
                responseType = ProcessRequestWithoutBody(requestType, apiParam, out response);
            else
                responseType = ProcessRequestWithBody(requestType, apiParam, bodyparam, out response);

            if (responseType)
            {
                responseText = ReadResponse(response);                

                //Console.WriteLine(responseText);
                response.Close();
            }
            else
                Console.WriteLine("Request Processing failed for  - " + apiparam);

            return responseText;
        }

        private static string ReadResponse(HttpWebResponse response)
        {
            using (Stream responseStream = response.GetResponseStream())
            {
                Stream streamToRead = responseStream;
                if (response.ContentEncoding.ToLower().Contains("gzip"))
                {
                    streamToRead = new GZipStream(streamToRead, CompressionMode.Decompress);
                }
                else if (response.ContentEncoding.ToLower().Contains("deflate"))
                {
                    streamToRead = new DeflateStream(streamToRead, CompressionMode.Decompress);
                }

                using (StreamReader streamReader = new StreamReader(streamToRead, Encoding.UTF8))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        private bool ProcessRequestWithoutBody(string requestType, string apiUri, out HttpWebResponse response)
        {
            response = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUri);

                request.UserAgent = "Fiddler";
                request.Headers.Set(HttpRequestHeader.Authorization, ConfigurationManager.AppSettings["BasicAuth"]);
                request.ContentType = "application/json";
                request.Method = requestType;

                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                Console.WriteLine("Exception for url - " + apiUri + "is " + e.Message);
                if (e.Status == WebExceptionStatus.ProtocolError)
                    response = (HttpWebResponse)e.Response;
                else return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception for url - " + apiUri + "is " + ex.Message);
                if (response != null) response.Close();
                return false;
            }

            return true;
        }

        private bool ProcessRequestWithBody(string requestType, string apiUri, string bodyparam,out HttpWebResponse response)
        {
            response = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUri);

                request.UserAgent = "Fiddler";
                request.Headers.Set(HttpRequestHeader.Authorization, ConfigurationManager.AppSettings["BasicAuth"]);
                request.ContentType = "application/json";
                request.Method = requestType;
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = bodyparam;
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                Console.WriteLine("Exception for url - " + apiUri + "is " + e.Message);
                if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
                else return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception for url - " + apiUri + "is " + ex.Message);
                if (response != null) response.Close();
                return false;
            }

            return true;
        }

    }
}
