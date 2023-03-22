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

namespace TestExecutionPlanner
{
    class RequestExecutor
    {
        public string MakeRequest(string requestType, string apiparam, string bodyparam = "")
        {
            HttpWebResponse response;
            string responseText = "";
            bool responseType = false;


            string apiParam = "";
            if(apiparam.Contains("tfvc"))                
                apiParam = ConfigurationManager.AppSettings["VSTSUrl"].Replace("InterACT%20Core/", "") + apiparam;
            else if (apiparam.Contains("distributedtask"))
                apiParam = ConfigurationManager.AppSettings["VSTSUrl"].Replace("InterACT%20Core/", "") + apiparam;
            else
                apiParam = ConfigurationManager.AppSettings["VSTSUrl"] + apiparam;

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

        private bool ProcessRequestWithBody(string requestType, string apiUri, string bodyparam, out HttpWebResponse response)
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
