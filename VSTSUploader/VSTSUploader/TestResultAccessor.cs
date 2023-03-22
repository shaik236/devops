using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace VSTSUploader
{

    public class TestResultAccessor
    {


        public List<TestResult> ReadTests(string testResultPath)
        {
            List<TestResult> testresults = new List<TestResult>();
            
            

            try
            {
                if (testResultPath.Contains("xml"))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(testResultPath);
                    XmlNodeList elemList = doc.GetElementsByTagName("property");
                    string ID, Outcome, planname = "", suitename = "", buildname = "", versionname = "", configname = "";

                    for (int i = 0; i < elemList.Count; i++)
                    {
                        if (elemList[i].Attributes["name"].Value == "TFSTestPlan")
                            planname = elemList[i].Attributes["value"].Value;

                        if (elemList[i].Attributes["name"].Value == "TFSTestSuite")
                            suitename = elemList[i].Attributes["value"].Value;

                        if (elemList[i].Attributes["name"].Value == "TFSConfigName")
                            configname = elemList[i].Attributes["value"].Value;

                        if (elemList[i].Attributes["name"].Value == "Baseline")
                            buildname = elemList[i].Attributes["value"].Value;

                    }


                    elemList = doc.GetElementsByTagName("testcase");

                    for (int i = 0; i < elemList.Count; i++)
                    {
                        //result.Title = elemList[i].Attributes["name"].Value;
                        ID = elemList[i].Attributes["ID"].Value;
                        Outcome = elemList[i].Attributes["result"].Value;
                        //result.Duration = elemList[i].Attributes["time"].Value;
                        testresults.Add(new TestResult(ID, Outcome, planname, suitename, buildname,  versionname, configname));
                    }
                }



                if (testResultPath.Contains("xls") || testResultPath.Contains("xlsx") || testResultPath.Contains("csv"))
                {
                    Excel.Application xlApp;
                    Excel.Workbook xlWorkBook;
                    Excel.Worksheet xlWorkSheet;
                    Excel.Range range;

                    string str;
                    double db = 0;
                    int rCnt;
                    int cCnt;
                    int rw = 0;
                    int cl = 0;
                    string testCaseID;
                    string outCome = "";

                    xlApp = new Excel.Application();
                    xlWorkBook = xlApp.Workbooks.Open(testResultPath, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                    range = xlWorkSheet.UsedRange;
                    rw = range.Rows.Count;
                    cl = range.Columns.Count;


                    for (rCnt = 2; rCnt <= rw; rCnt++)
                    {

                        if (testResultPath.Contains("csv"))
                        {
                            string data = "";
                            if (((range.Cells[rCnt, 1] as Excel.Range).Value2) != null)
                                data = ((range.Cells[rCnt, 1] as Excel.Range).Value2).ToString();
                            else
                                continue;

                            string[] dataVals = data.Split(',');
                            testCaseID = dataVals[0];
                            outCome = dataVals[3];
                        }
                        else
                        {
                            if (((range.Cells[rCnt, 1] as Excel.Range).Value2) != null)
                                testCaseID = ((range.Cells[rCnt, 1] as Excel.Range).Value2).ToString();
                            else
                                continue;

                            if (((range.Cells[rCnt, 2] as Excel.Range).Value2) != null)
                                outCome = ((range.Cells[rCnt, 2] as Excel.Range).Value2).ToString();

                        }

                        outCome = outCome.ToLower();
                        

                        if (outCome == "fail")
                            outCome = "Failed";

                        if (outCome == "pass")
                            outCome = "Passed";

                        if (outCome == "ok")
                            outCome = "Passed";

                        if (rCnt == 120)
                            str = "test";

                        
                        testresults.Add(new TestResult(testCaseID, outCome, "","","","",""));

                    }

                    


                    xlWorkBook.Close(true, null, null);
                    xlApp.Quit();

                    Marshal.ReleaseComObject(xlWorkSheet);
                    Marshal.ReleaseComObject(xlWorkBook);
                    Marshal.ReleaseComObject(xlApp);

                    return testresults;
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            return testresults;
        }

    }
}
