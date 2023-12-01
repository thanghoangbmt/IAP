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
builder.Services.AddSingleton<SendImportantNotificationCronJob>();
builder.Services.AddSingleton<SendNormalNotificationCronJob>();

//Cron expression:
// - "0 0/15 * ? * * *": 15m
// - "0 0/30 * ? * * *": 30m
// - "0/30 * * ? * * *": 30s
builder.Services.AddSingleton(new JobSchedule(
    Guid.Parse("0ea7fd03-8465-4be6-846f-f9758e329098"),
    jobType: typeof(SendImportantNotificationCronJob),
    "ImportantNotificationJob",
    cronExpression: "0/30 * * ? * * *")); //30s

builder.Services.AddSingleton(new JobSchedule(
    Guid.Parse("3504147b-b835-4866-a8c9-b648e2b1e431"),
    jobType: typeof(SendNormalNotificationCronJob),
    "NormalNotificationJob",
    cronExpression: "0 0/30 * ? * * *")); //30m

builder.Services.AddHostedService<QuartzHostedService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
