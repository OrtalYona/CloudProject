using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using CloudProject.Models;

namespace CloudProject.Helpers
{
    public static class CouchDBConnect
    {
        private static string host = "https://1b9b2253-3a6e-4d23-82c9-3bc6ef256e41-bluemix:0a85d35a70b8c6d034ec1a0ded476c295d180d97ebf41561bb35fcbf67cab731@1b9b2253-3a6e-4d23-82c9-3bc6ef256e41-bluemix.cloudant.com/{0}";
        public static HttpClient GetClient(string db) {
            var hc = new HttpClient();
            hc.BaseAddress = new Uri(string.Format(host,db));
            hc.DefaultRequestHeaders.Clear();
            hc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Console.WriteLine(hc.DefaultRequestHeaders);
            return hc;
        }
    }
}