using Quartz;
using Quartz.Spi;

namespace IAP.CronJobs
{
    public class QuartzHostedService : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly JobSchedule _jobSchedule;

        public QuartzHostedService(ISchedulerFactory schedulerFactory, IJobFactory jobFactory, JobSchedule jobSchedule)
        {
            _schedulerFactory = schedulerFactory;
            _jobSchedule = jobSchedule;
            _jobFactory = jobFactory;
        }

        public IScheduler Scheduler { get; set; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler = await _schedulerFactory.GetScheduler();
            Scheduler.JobFactory = _jobFactory;

          
            var job = CreateJob(_jobSchedule);
            var trigger = CreateTrigger(_jobSchedule);

            await Scheduler.ScheduleJob(job, trigger, cancellationToken);

            await Scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler?.Shutdown(cancellationToken);
        }

        private static IJobDetail CreateJob(JobSchedule schedule)
        {
            var jobType = schedule.JobType;
            return JobBuilder
                .Create(jobType)
                .WithIdentity(schedule.JobId.ToString())
                .WithDescription(schedule.JobName)
                .Build();
        }

        private static ITrigger CreateTrigger(JobSchedule schedule)
        {
            return TriggerBuilder
                .Create()
                .WithIdentity(schedule.JobId.ToString())
                .WithCronSchedule(schedule.CronExpression)
                .WithDescription(schedule.JobName)
                .Build();
        }
    }
}