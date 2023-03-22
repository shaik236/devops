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

namespace VSTSUploader
{
    
    public static class Program
    {
        //public static void Main(string[] args)
        public static void Main(string[] args)
        {
            string planName = "";
            string suiteName = "";
            string configName = "";
            string buildName = "";
            string fileName = @"C:\Users\xxxx\Test.xml";

            if (args.Length == 0)
            {
                System.Console.WriteLine("Please enter the argument.");
                return;
            }

            foreach (string arg in args)
            {
                string val = arg.ToLower();
                if (val.Contains("planname"))
                {
                    planName = val.Replace("-planname:", "");

                }
                if (val.Contains("suitename"))
                {
                    suiteName = val.Replace("-suitename:", "");

                }
                if (val.Contains("configname"))
                {
                    configName = val.Replace("-configname:", "");

                }
                if (val.Contains("buildname"))
                {
                    buildName = val.Replace("-buildname:", "");

                }

                if (val.Contains("filename"))
                {
                    fileName = val.Replace("-filename:", "");

                }

            }



            TestActivities testAct = new TestActivities();

            //string pattern = "*.xml";
            //var dirInfo = new DirectoryInfo(ConfigurationManager.AppSettings["TestResultsPath"]);
            //var file = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f).First();

            testAct.UploadTestResults(fileName, planName, suiteName, configName, buildName);

            Console.ReadLine();
            

        }
    }

}