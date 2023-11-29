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
            var client = new RestClient("https://capstonefpt.au1.my.auvik.com/graphql");
            var username = "thanghoang.bmt@gmail.com";
            var password = "i3fjmcLY7KW/O5sYUHhs3qyhOpHdaQeqAdE6osf6qaVFUSjf";

            RestRequest restRequest = new RestRequest();
            restRequest.Authenticator = new HttpBasicAuthenticator(username, password);
            var sendRequest = new SendRequest(auvikRequest);
            restRequest.Method = Method.Post;
            restRequest.AddStringBody(sendRequest.ToString(), DataFormat.Json);
            var response = client.Post(restRequest);

            var result = JsonConvert.DeserializeObject<AuvikSyslogResponse>(response.Content);

            if (auvikRequest.DeviceName.Equals("All") || auvikRequest.DeviceName == null)
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
            var client = new RestClient("https://capstonefpt.au1.my.auvik.com/graphql");
            var username = "thanghoang.bmt@gmail.com";
            var password = "i3fjmcLY7KW/O5sYUHhs3qyhOpHdaQeqAdE6osf6qaVFUSjf";

            RestRequest restRequest = new RestRequest();
            restRequest.Authenticator = new HttpBasicAuthenticator(username, password);
            var sendRequest = new SendRequest(auvikRequest);
            restRequest.Method = Method.Post;
            restRequest.AddStringBody(sendRequest.ToString(), DataFormat.Json);
            var response = client.Post(restRequest);

            var result = JsonConvert.DeserializeObject<AuvikSyslogResponse>(response.Content);
            return result;
        }
    }
}
