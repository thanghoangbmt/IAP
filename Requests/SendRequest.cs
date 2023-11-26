namespace IAP.Requests
{
    public class SendRequest
    {
        public SendRequest(AuvikRequest auvikRequest)
        {
            operationName = "loglist";
            variables = new SendRequestVariable(auvikRequest);
            query = "query loglist($clientId: ID!, $endDate: String!, $exporterId: ID, $mspId: ID!, " +
            "$scrollId: ID, $size: String, $startDate: String!, $targetDomainPrefix: TenantDomainPrefix!, $searchesMulti: SearchingsMultiType, " +
            "$tenantFilter: [String], $sortings: SortingsType) {\n  logs(clientId: $clientId, endDate: $endDate, exporterId: $exporterId, mspId: $mspId, scrollId: " +
            "$scrollId, searchesMulti: $searchesMulti, size: $size, startDate: $startDate, targetDomainPrefix: $targetDomainPrefix, tenantFilter: $tenantFilter, sortings: $sortings) " +
            "{\n    total\n    scrollId\n    lines {\n      deviceId\n      deviceName\n      eventTs\n      tag\n      message\n      severity\n      facility\n      hostname\n      " +
            "exporterSrcAddr\n      __typename\n    }\n    __typename\n  }\n}\n";
        }
        public string operationName { get; set; } 
        public SendRequestVariable variables { get; set; }
        public string query { get; set; }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
