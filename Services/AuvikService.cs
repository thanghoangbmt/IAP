using IAP.Requests;
using IAP.Responses;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace IAP.Services
{
    public class AuvikService
    {
        public string GetSysLog(AuvikRequest auvikRequest)
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

            return response.Content;
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
