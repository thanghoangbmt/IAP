using IAP.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IAP.Requests;
using Telegram.Bot;
using System;
using IAP.Responses;
using System.Linq;
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

        [HttpPost("tele")]
        public async Task<ActionResult> SendMessageToTelegram([FromBody] AuvikRequest request)
        {
            var auvikRequest = new AuvikRequest() { StartDate = request.StartDate, EndDate = request.EndDate };
            AuvikSyslogResponse auvikSyslogResponse = service.GetSysLogForSendMessage(auvikRequest);
            var warningLogs = from line in auvikSyslogResponse.data.logs.lines
                              where line.severity == (int) AuvikSeverityEnum.Warning
                              select line;

            if (warningLogs.Count() > 0)
            {
                string message = "There are " + warningLogs.Count() + " warning logs from Auvik from " + request.StartDate.ToString() + " to " + request.EndDate.ToString() + "!";
                string botToken = "6720093868:AAF-i_TcWt9EkSH5QDPTBX6K1D-Xe9dVeT4";
                var botClient = new TelegramBotClient(botToken);
                string chatId = "-4025383582";
                await botClient.SendTextMessageAsync(chatId, message);
            }

            return Ok("Message sent!");
        }
    }
}
