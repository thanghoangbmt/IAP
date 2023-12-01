using IAP.Enums;
using IAP.Requests;
using IAP.Responses;
using IAP.Services;
using Quartz;
using Telegram.Bot;

namespace IAP.CronJobs
{
    public class SendNormalNotificationCronJob : IJob
    {
        private AuvikService service = new AuvikService();

        public async Task Execute(IJobExecutionContext context)
        {
            var endDate = DateTime.UtcNow;
            var startDate = endDate.AddMinutes(-30);
            var auvikRequest = new AuvikRequest() { StartDate = startDate, EndDate = endDate };
            AuvikSyslogResponse auvikSyslogResponse = service.GetSysLogForSendMessage(auvikRequest);
            var warningLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Warning).Count();
            var noticeLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Notice).Count();
            var informationLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Informational).Count();
            var debugLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Debug).Count();

            if (warningLogCount > 0 || noticeLogCount > 0 || informationLogCount > 0 || debugLogCount > 0)
            {
                string message = "Auvik syslog's normal notification from " + startDate.AddHours(7) + " to " + endDate.AddHours(7) + ": \r\n";
                if (warningLogCount > 0)
                    message += "- Warning log: " + warningLogCount + ".\r\n";
                if (noticeLogCount > 0)
                    message += "- Notice log: " + noticeLogCount + ".\r\n";
                if (informationLogCount > 0)
                    message += "- Information log: " + informationLogCount + ".\r\n";
                if (debugLogCount > 0)
                    message += "- Debug log: " + debugLogCount + ".";
                string botToken = "6757127481:AAEFs5S0tw9sv4AlPt3P2wKkvjrADPhaS2A";
                var botClient = new TelegramBotClient(botToken);
                string chatId = "-4071366636";
                await botClient.SendTextMessageAsync(chatId, message);
            }
        }
    }
}
