using IAP.Services;
using Microsoft.AspNetCore.Mvc;
using IAP.Requests;
using Telegram.Bot;
using System;
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
            var auvikRequest = new AuvikRequest() { StartDate = request.StartDate.GetValueOrDefault().AddHours(-7), EndDate = request.EndDate.GetValueOrDefault().AddHours(-7) };
            AuvikSyslogResponse auvikSyslogResponse = service.GetSysLogForSendMessage(auvikRequest);
            var warningLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Warning).Count();
            var noticeLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Notice).Count();
            var informationLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Informational).Count();
            var debugLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Debug).Count();

            string message = "There are " + warningLogCount + " warning, "
                + noticeLogCount + " notice, "
                + informationLogCount + " information, "
                + debugLogCount + " debug "
                + "logs from Auvik from " + request.StartDate.ToString() + " to " + request.EndDate.ToString() + "!";
            string botToken = "6720093868:AAF-i_TcWt9EkSH5QDPTBX6K1D-Xe9dVeT4";
            var botClient = new TelegramBotClient(botToken);
            string chatId = "-4025383582";
            await botClient.SendTextMessageAsync(chatId, message);

            return Ok("Message sent!");
        }

        [HttpPost("tele-important")]
        public async Task<ActionResult> SendImportantMessageToTelegram([FromBody] AuvikRequest request)
        {
            var auvikRequest = new AuvikRequest() { StartDate = request.StartDate.GetValueOrDefault().AddHours(-7), EndDate = request.EndDate.GetValueOrDefault().AddHours(-7) };
            AuvikSyslogResponse auvikSyslogResponse = service.GetSysLogForSendMessage(auvikRequest);
            
            var emergencyLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Emergency).Count();
            var alertLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Alert).Count();
            var criticalLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Critical).Count();
            var errorLogCount = auvikSyslogResponse.data.logs.lines.Where(line => line.severity == (int)AuvikSeverityEnum.Error).Count();

            string message = "There are " + emergencyLogCount + " emergency, "
                + alertLogCount + " alert, "
                + criticalLogCount + " critical, "
                + errorLogCount + " error "
                + "logs from Auvik from " + request.StartDate.ToString() + " to " + request.EndDate.ToString() + "!";
            string botToken = "6720093868:AAF-i_TcWt9EkSH5QDPTBX6K1D-Xe9dVeT4";
            var botClient = new TelegramBotClient(botToken);
            string chatId = "-4025383582";
            await botClient.SendTextMessageAsync(chatId, message);

            return Ok("Message sent!");
        }
    }
}
