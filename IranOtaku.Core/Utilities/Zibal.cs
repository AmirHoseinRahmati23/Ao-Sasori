using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace ZibalIPGRequests
{
    public class Zibal
    {
        public static HttpWebResponse HttpRequestToZibal(string url, string data)
        {
#pragma warning disable SYSLIB0014 // Type or member is obsolete
            var httpWebRequest = (HttpWebRequest)WebRequest.CreateHttp(url); // make request
#pragma warning restore SYSLIB0014 // Type or member is obsolete
            httpWebRequest.ContentType = "application/json; charset=utf-8"; // content of request -> must be JSON
            httpWebRequest.Method = "POST"; // method of request -> must be POST
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(data); // send request
                streamWriter.Flush(); // flush stream
            }
            return (HttpWebResponse)httpWebRequest.GetResponse(); // get Response
        }

        public class makeRequest
        {
            public string merchant { get; set; }
            public string orderId { get; set; }
            public int amount { get; set; }
            public string callbackUrl { get; set; }
            public string description { get; set; }
            public string mobile { get; set; }
        }

        public class makeRequest_response
        {
            public string trackId { get; set; }
            public string result { get; set; }
            public string message { get; set; }
        }

        public class verifyRequest
        {
            public string merchant { get; set; }
            public string trackId { get; set; }
        }

        public class verifyResponse
        {
            public string paidAt { get; set; }
            public string status { get; set; }
            public string amount { get; set; }
            public string result { get; set; }
            public string message { get; set; }
        }


    }
}
