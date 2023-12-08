using IAP.Enums;
using IAP.Requests;
using IAP.Responses;
using Quartz;
using Telegram.Bot;
using IAP.Services;

namespace IAP.CronJobs
{
    public class SendImportantNotificationCronJob : IJob
    {
        private AuvikService service = new AuvikService();

        public async Task Execute(IJobExecutionContext context)
        {
            var endDate = DateTime.UtcNow.AddHours(7).AddMinutes(-2);
            var startDate = endDate.AddMinutes(-5);
            var auvikRequest = new AuvikRequest() { StartDate = startDate, EndDate = endDate };
            AuvikSyslogResponse auvikSyslogResponse = service.GetSysLogForSendMessage(auvikRequest);
            var emergencyLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Emergency).Count();
            var alertLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Alert).Count();
            var criticalLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Critical).Count();
            var errorLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Error).Count();

            if (emergencyLogCount > 0 || alertLogCount > 0 || criticalLogCount > 0 || errorLogCount > 0)
            {
                string message = "Auvik syslog's important notification from " + startDate + " to " + endDate + ": \r\n";
                if (emergencyLogCount > 0)
                    message += "- Emergency log: " + emergencyLogCount + ".\r\n";
                if (alertLogCount > 0)
                    message += "- Alert log: " + alertLogCount + ".\r\n";
                if (criticalLogCount > 0)
                    message += "- Critical log: " + criticalLogCount + ".\r\n";
                if (errorLogCount > 0)
                    message += "- Error log: " + errorLogCount + ".";
                string botToken = "6757127481:AAEFs5S0tw9sv4AlPt3P2wKkvjrADPhaS2A";
                var botClient = new TelegramBotClient(botToken);
                string chatId = "-4071366636";
                await botClient.SendTextMessageAsync(chatId, message);
            }
        }
    }
}
