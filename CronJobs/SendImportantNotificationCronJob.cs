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
            var endDate = DateTime.UtcNow;
            var startDate = endDate.AddSeconds(-30);
            var auvikRequest = new AuvikRequest() { StartDate = startDate, EndDate = endDate };
            AuvikSyslogResponse auvikSyslogResponse = service.GetSysLogForSendMessage(auvikRequest);
            var emergencyLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Emergency).Count();
            var alertLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Alert).Count();
            var criticalLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Critical).Count();
            var errorLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Error).Count();

            string message = "There are " + emergencyLogCount + " emergency, " 
                + alertLogCount + " alert, "
                + criticalLogCount + " critical, "
                + errorLogCount + " error "
                + "logs from Auvik from " + startDate.AddHours(7) + " to " + endDate.AddHours(7) + "!";
            string botToken = "6720093868:AAF-i_TcWt9EkSH5QDPTBX6K1D-Xe9dVeT4";
            var botClient = new TelegramBotClient(botToken);
            string chatId = "-4025383582";
            await botClient.SendTextMessageAsync(chatId, message);

        }
    }
}
