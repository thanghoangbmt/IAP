using IAP.Requests;
using IAP.Responses;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace IAP.Services
{
    public class AuvikService
    {
        public AuvikSyslogResponse GetSysLog(AuvikRequest auvikRequest)
        {
            var response = SendRequestToAuvik(auvikRequest);
            var result = JsonConvert.DeserializeObject<AuvikSyslogResponse>(response.Content);

            if (auvikRequest.DeviceName == null || auvikRequest.DeviceName.Equals("All"))
            {
                return result;
            }
            else
            {
                
                var lines = result.data.logs.lines.Where(x => x.deviceName.Equals(auvikRequest.DeviceName)).ToList();
                var auvikLog = new AuvikLogs
                {
                    total = lines.Count,
                    lines = lines,
                    scrollId = result.data.logs.scrollId,
                    __typename = result.data.logs.__typename,
                };

                return result = new AuvikSyslogResponse
                {
                    data = new AuvikData
                    {
                        logs = auvikLog
                    },
                };
            }

        }

        public AuvikSyslogResponse GetSysLogForSendMessage(AuvikRequest auvikRequest)
        {
            var response = SendRequestToAuvik(auvikRequest);
            var result = JsonConvert.DeserializeObject<AuvikSyslogResponse>(response.Content);
            return result;
        }

        public List<string> GetDistinctDevice(AuvikRequest auvikRequest)
        {
            var response = SendRequestToAuvik(auvikRequest);
            var auvikSyslogResponse = JsonConvert.DeserializeObject<AuvikSyslogResponse>(response.Content);
            var result = auvikSyslogResponse?.data?.logs?.lines.Select(line => line.deviceName).Distinct().ToList();

            return (result != null && result.Any()) ? result : new List<string>();
        }

        private RestResponse? SendRequestToAuvik(AuvikRequest auvikRequest)
        {
            var client = new RestClient("https://capstonefpt.au1.my.auvik.com/graphql");
            var username = "trungntse161075@fpt.edu.vn";
            var password = "pA03qQ16LXq3J/M9RIn+ecMA5SaMw9tUDe0/oT09x1Cxhvli";

            RestRequest restRequest = new RestRequest();
            restRequest.Authenticator = new HttpBasicAuthenticator(username, password);
            var sendRequest = new SendRequest(auvikRequest);
            restRequest.Method = Method.Post;
            restRequest.AddStringBody(sendRequest.ToString(), DataFormat.Json);
            var response = client.Post(restRequest);
            return response;
        }
    }
}
