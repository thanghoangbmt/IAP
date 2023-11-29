using System;

namespace IAP.Requests
{
    public class SendRequestVariable
    {
        public SendRequestVariable(AuvikRequest auvikRequest)
        {
            clientId = "1032914113525458685";
            endDate = auvikRequest.EndDate.GetValueOrDefault().AddHours(-7);
            exporterId = "";
            mspId = "1012609622137906942";
            scrollId = null;
            sortings = new Sorting { event_ts = "DESC" };
            size = "10000";
            startDate = auvikRequest.StartDate.GetValueOrDefault().AddHours(-7);
            targetDomainPrefix = "capstonefpt";
        }

        public string clientId { get; set; }
        public DateTime? endDate { get; set; }
        public string exporterId { get; set; }
        public string mspId { get; set; }
        public string scrollId { get; set; }
        public Sorting sortings { get; set; }
        public string size { get; set; }
        public DateTime? startDate { get; set; }
        public string targetDomainPrefix { get; set; }
    }
}
