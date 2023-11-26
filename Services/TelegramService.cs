using System.Threading.Tasks;
using Telegram.Bot;

namespace IAP.Services
{
    public class TelegramService
    {
        public async Task SendMessageAsync(string message)
        {
            string botToken = "6720093868:AAF-i_TcWt9EkSH5QDPTBX6K1D-Xe9dVeT4";
            var botClient = new TelegramBotClient(botToken);
            string chatId = "-4025383582";
            await botClient.SendTextMessageAsync(chatId, message);
        }
    }
}
