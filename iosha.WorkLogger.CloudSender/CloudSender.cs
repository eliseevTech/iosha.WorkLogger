using System;
using System.Threading.Tasks;
using iosha.WorkLogger.Data;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization;

namespace iosha.WorkLogger.CloudSender
{
    public class CloudSender : ICloudSender
    {
        private readonly string _sendPath = "http://localhost:5000";

        public string Send(WorkLog workLog)
        {
            var request = new RestRequest("/WorkLogger", Method.POST);
            request.AddJsonBody(workLog);
            request.AddHeader("Content-type", "application/json");

            var _client = new RestClient(_sendPath);
            var result = _client.Execute<string>(request);
            return result.Data;
        }


    }
}
