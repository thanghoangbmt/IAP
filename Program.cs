using IAP.CronJobs;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IJobFactory, JobFactory>();
builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

// Add our job
builder.Services.AddSingleton<SendNotificationCronJob>();

JobSchedule jobSchedule = new JobSchedule(
    Guid.NewGuid(),
    jobType: typeof(SendNotificationCronJob),
    "NotificationJob",
    cronExpression: "0 0 * ? * * *");

builder.Services.AddSingleton(jobSchedule);

builder.Services.AddHostedService<QuartzHostedService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
