using IAP.Enums;
using IAP.Requests;
using IAP.Responses;
using Quartz;
using Telegram.Bot;
using IAP.Services;

namespace IAP.CronJobs
{
    public class SendNotificationCronJob : IJob
    {
        private AuvikService service = new AuvikService();
        
        public async Task Execute(IJobExecutionContext context)
        {
            var endDate = DateTime.UtcNow;
            var startDate = endDate.AddSeconds(-30);
            var auvikRequest = new AuvikRequest() { StartDate = startDate, EndDate = endDate };
            AuvikSyslogResponse auvikSyslogResponse = service.GetSysLogForSendMessage(auvikRequest);
            var warningLogs = from line in auvikSyslogResponse.data.logs.lines
                              where line.severity == (int)AuvikSeverityEnum.Warning
                              select line;

            if (warningLogs.Count() > 0)
            {
                string message = "There are " + warningLogs.Count() + " warning logs from Auvik from " + startDate + " to " + endDate + "!";
                string botToken = "6720093868:AAF-i_TcWt9EkSH5QDPTBX6K1D-Xe9dVeT4";
                var botClient = new TelegramBotClient(botToken);
                string chatId = "-4025383582";
                await botClient.SendTextMessageAsync(chatId, message);
            }
        }
    }
}
