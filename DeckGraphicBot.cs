using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace DeckGraphicBot
{
    public class DeckGraphicBot
    {
        private const string APP_TOKEN = "app_token_here";
        private DiscordSocketClient _client;

        public static void Main(string[] args)
        {
            new DeckGraphicBot().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync() 
        {
            _client = new DiscordSocketClient();
            _client.Log += Log;

            await _client.LoginAsync(TokenType.Bot, APP_TOKEN);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg) 
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
