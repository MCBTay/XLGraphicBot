using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace XLGraphicBot
{
    public class XLGraphicBot
    {
        private const string APP_TOKEN = "app_token_here";
        private DiscordSocketClient _client;

        public static void Main(string[] args)
        {
            new XLGraphicBot().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync() 
        {
            _client = new DiscordSocketClient();
            _client.Log += Log;

            await _client.LoginAsync(TokenType.Bot, APP_TOKEN);
            await _client.StartAsync();

            _client.Ready += OnReady;

            await Task.Delay(-1);
        }

        private async Task OnReady()
        {
          var commandHandler = new CommandHandler(_client, new Discord.Commands.CommandService());
          await commandHandler.InstallCommandsAsync();
        }

        private Task Log(LogMessage msg) 
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
