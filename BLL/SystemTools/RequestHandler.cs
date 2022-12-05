using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SystemTools
{
    public static class RequestHandler
    {
        public static string GetSecureHttpData(string url)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.DefaultConnectionLimit = 9999;

            HttpWebRequest request = (HttpWebRequest)
                WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json"; 

            //Get certificate
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection collection = store.Certificates.Find(X509FindType.FindBySubjectName, "mycertsubjectname", false);

            //Associate the collection of certificates to the request
            request.ClientCertificates = collection;

            //Add headers
            request.Headers.Add("CUSTOM-HEADER-1", "headerValue1");
            request.Headers.Add("CUSTOM-UNIX-EPOCH-TIMESTAMP-HEADER", "1557515741"); // unixEpochTimestamp);
            request.Headers.Add("CUSTOM-HEADER-3", "headerValue3");

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);

            String receivedData = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            return receivedData;
        }
        public static string GetHttpData(string url, string method)
        {


            HttpWebRequest request = (HttpWebRequest)
                WebRequest.Create(url);
            request.Method = method;
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);

            String receivedData = reader.ReadToEnd();

            reader.Close();
            dataStream.Close();
            return receivedData;
        }
    }
}
