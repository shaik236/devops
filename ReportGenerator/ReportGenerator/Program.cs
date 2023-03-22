using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Configuration;
using System.Management;

namespace ReportGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string resultsPath = string.Empty;
            string buildName = string.Empty;

            foreach (string arg in args)
            {
                string val = arg.ToLower();
                string actualVal = arg;

                if (val.Contains("resultspath"))
                {
                    resultsPath = val.Replace("-resultspath:", "");
                    Console.WriteLine("Results Path passed is - " + resultsPath);
                }

                if (val.Contains("buildname"))
                {
                    buildName = actualVal.Replace("-buildname:", "");
                    Console.WriteLine("Build passed is - " + buildName);
                }


            }

            if (string.IsNullOrEmpty(resultsPath))
            {
                resultsPath = @"C:\Jmeter\AggregateReport.xml";
            }

            if (string.IsNullOrEmpty(buildName))
            {
                buildName = "";
            }

            try
            {
                string xmlFile = resultsPath;
                string htmlReport = GetPageHTML(xmlFile);
                //SendReport(htmlReport,false,buildName);

                //string xmlFile = args[0].ToString();
                //string bld = args[1].ToString();

                ////string xmlFile = @"D:\Workspace\TFSConnect\Res\Avocet_BVTAVOBVT-2017.xml";
                ////string bld = "AvocetMain_2017.2-PTC_20170817.1";


                //string htmlReport = GetPageHTML(xmlFile, false);
                //string htmrep = @"C:\Users\sshaik7\Desktop\devops\RegressionSummaryReplication.html";
                //SendReport(htmrep);
                ////htmlReport = @"C:\Shaik\ConvertedFile.html";
                //SendReport(htmlReport, true, bld);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// Get HTML for Xml Passed based on mode XSLT File
        /// </summary>
        /// <param name="xmlData">xmlData as string</param>
        /// <param name="xsltFileName">xsltFileName for View/Save mode</param>
        /// <returns name="resultHtml">resultHtml - Html as String</returns>
        public static string GetPageHTML(string xmlFile, bool bvtNo = true)
        {
            //Get Full path of XSLT File
            string fullXsltFilePath = string.Empty;
            string pp = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var dir = System.IO.Path.GetDirectoryName(pp);
            if (bvtNo)
            fullXsltFilePath = dir + @"\Transformer.xsl";
            else
                fullXsltFilePath = dir + @"\TransformerBvt.xsl";

            //Call LoadXMLToReader to load xml into Reader and process reader to fetch HTMl specific to XSLT
            XmlTextReader reader = new XmlTextReader(xmlFile);

            XslTransform xslt = new XslTransform();
            XsltArgumentList xslArgs = new XsltArgumentList();
            xslt.Load(fullXsltFilePath);
            XPathDocument xpath = new XPathDocument(xmlFile);
            XmlTextWriter xwriter = new XmlTextWriter(xmlFile.Replace(".xml", ".html"), Encoding.UTF8);
            xslt.Transform(xpath, xslArgs, xwriter, null);
            xwriter.Close();

            return xmlFile.Replace(".xml", ".html");
            
        }

        public static void SendReport(string htmlFile, bool IsBvt = false, string build ="")
        {
            string to = "sshaik7@slb.com";
            string from = "sshaik7@slb.com";
            string server = "gateway.mail.slb.com";

            string val = System.Configuration.ConfigurationManager.AppSettings["MailReceipents"];

            using (StreamReader reader = File.OpenText(htmlFile)) // Path to your 
            {
                MailMessage message = new MailMessage(from, val);
                if(IsBvt)
            message.Subject = "Avocet BVT Execution results summary (Build - " + build + ")";
                else
                    message.Subject = "Avocet Automation Regression Test Execution results summary for Build - " + build;

                message.IsBodyHtml = true;

                message.Body = reader.ReadToEnd();

                message.Body = message.Body + "</br> Thanks,</br>AvocetAutomation.";
                message.Attachments.Add(new Attachment(htmlFile));


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

          

           
        }
    }
}
