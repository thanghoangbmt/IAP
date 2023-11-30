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

            string message = "There are " + warningLogCount + " warning, "
                + noticeLogCount + " notice, "
                + informationLogCount + " information, "
                + debugLogCount + " debug "
                + "logs from Auvik from " + startDate.AddHours(7) + " to " + endDate.AddHours(7) + "!";
            string botToken = "6720093868:AAF-i_TcWt9EkSH5QDPTBX6K1D-Xe9dVeT4";
            var botClient = new TelegramBotClient(botToken);
            string chatId = "-4025383582";
            await botClient.SendTextMessageAsync(chatId, message);
        }
    }
}
