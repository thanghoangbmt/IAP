using System;

namespace IAP.CronJobs
{
    public class JobSchedule
    {
        public JobSchedule(Guid Id, Type jobType, string jobName, string cronExpression)
        {
            JobId = Id;
            JobType = jobType;
            JobName = jobName;
            CronExpression = cronExpression;
        }
        public Guid JobId { get; set; }
        public Type JobType { get; set; }
        public string JobName { get; set; }
        public string CronExpression { get; set; }
    }
}
