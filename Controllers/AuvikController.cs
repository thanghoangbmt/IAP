using IAP.Services;
using Microsoft.AspNetCore.Mvc;
using IAP.Requests;
using Telegram.Bot;
using IAP.Responses;
using IAP.Enums;

namespace IAP.Controllers
{
    [Route("api/auvik")]
    [ApiController]
    public class AuvikController : ControllerBase
    {
        private AuvikService service = new AuvikService();

        [HttpPost("syslog")]
        public async Task<ActionResult> GetSyslog([FromBody] AuvikRequest request)
        {
            var syslog = service.GetSysLog(request);
            return Ok(syslog);
        }

        [HttpPost("distinctDevice")]
        public async Task<ActionResult> GetDistinctDevice([FromBody] AuvikRequest request)
        {
            var syslog = service.GetDistinctDevice(request);
            return Ok(syslog);
        }

        [HttpPost("tele-normal")]
        public async Task<ActionResult> SendNormalMessageToTelegram([FromBody] AuvikRequest request)
        {
            var auvikRequest = new AuvikRequest() { StartDate = request.StartDate.GetValueOrDefault(), EndDate = request.EndDate.GetValueOrDefault() };
            AuvikSyslogResponse auvikSyslogResponse = service.GetSysLogForSendMessage(auvikRequest);
            var warningLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Warning).Count();
            var noticeLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Notice).Count();
            var informationLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Informational).Count();
            var debugLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Debug).Count();

            if (warningLogCount > 0 || noticeLogCount > 0 || informationLogCount > 0 || debugLogCount > 0)
            {
                string message = "Auvik syslog's normal notification from " + request.StartDate.ToString() + " to " + request.EndDate.ToString() + ": \r\n";
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
            return Ok("Message sent!");
        }

        [HttpPost("tele-important")]
        public async Task<ActionResult> SendImportantMessageToTelegram([FromBody] AuvikRequest request)
        {
            var auvikRequest = new AuvikRequest() { StartDate = request.StartDate.GetValueOrDefault(), EndDate = request.EndDate.GetValueOrDefault() };
            AuvikSyslogResponse auvikSyslogResponse = service.GetSysLogForSendMessage(auvikRequest);

            var emergencyLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Emergency).Count();
            var alertLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Alert).Count();
            var criticalLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Critical).Count();
            var errorLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Error).Count();

            if (emergencyLogCount > 0 || alertLogCount > 0 || criticalLogCount > 0 || errorLogCount > 0)
            {
                string message = "Auvik syslog's important notification from " + request.StartDate.ToString() + " to " + request.EndDate.ToString() + ": \r\n";
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

            return Ok("Message sent!");
        }
    }
}
