using Discord;
using Discord.WebSocket;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace XLGraphicBot
{
	public class XLGraphicBot
    {
        private DiscordSocketClient _client;

        public static void Main(string[] args)
        {
            new XLGraphicBot().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync() 
        {
            _client = new DiscordSocketClient();
            _client.Log += Log;

            var tokenPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "token.txt");

            var token = string.Empty;

            if (!File.Exists(tokenPath))
            {
	            Console.WriteLine("Couldn't find token file.");
	            return;
            }

            using (var sr = new StreamReader(tokenPath))
            {
	            token = await sr.ReadToEndAsync();
            }

            if (string.IsNullOrEmpty(token)) return;

            await _client.LoginAsync(TokenType.Bot, token);
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
