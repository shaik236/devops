using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestExecutionPlanner
{
    class TestPoints
    {
        public Value[] value { get; set; }
        public int count { get; set; }
    }

    public class Value
    {
        public int id { get; set; }
        public int rev { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public object[] validationResults { get; set; }
        public string buildNumber { get; set; }
        public string status { get; set; }
        public string result { get; set; }
        public DateTime queueTime { get; set; }
        public DateTime startTime { get; set; }
        public DateTime finishTime { get; set; }

        public int buildNumberRevision { get; set; }
        public string sourceBranch { get; set; }
        public string sourceVersion { get; set; }

        public string priority { get; set; }
        public string reason { get; set; }

        public DateTime lastChangedDate { get; set; }

        public bool keepForever { get; set; }
        public bool retainedByRelease { get; set; }
        public object triggeredByBuild { get; set; }
        public string description { get; set; }
        public string state { get; set; }

        public string outcome { get; set; }

        public testCase testCase { get; set; }

        public string[] tags { get; set; }
    }

    public class testCase
    {
        public string id { get; set; }
    }


    


}
