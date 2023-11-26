using System.Collections.Generic;

namespace IAP.Responses
{
    public class AuvikSyslogResponse
    {
        public AuvikData data { get; set; }
    }

    public class AuvikData
    {
        public AuvikLogs logs { get; set; }
    }

    public class AuvikLogs
    {
        public long total { get; set; }
        public string scrollId { get; set; }
        public List<Line> lines { get; set; }
        public string __typename { get; set; }

    }
    public class Line
    {
        public string deviceId { get; set; }
        public string deviceName { get; set; }
        public string eventTs { get; set; }
        public string tag { get; set; }
        public string message { get; set; }
        public int severity { get; set; }
        public int facility { get; set; }
        public string hostname { get; set; }
        public string exporterSrcAddr { get; set; }
        public string __typename { get; set; }
    }
}
