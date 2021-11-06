using Discord;
using Discord.WebSocket;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using XLGraphicBot.services;

namespace XLGraphicBot
{
	public class XLGraphicBot
    {
	    private static IServiceProvider _serviceProvider;
        private DiscordSocketClient _client;

        public static void Main(string[] args)
        {
	        _serviceProvider = new ServiceCollection()
		        .AddHttpClient()
		        .AddSingleton<IBitmapService, BitmapService>()
		        .AddScoped<IDiscordService, DiscordService>()
		        .AddScoped<IFileSystem, FileSystem>()
		        .BuildServiceProvider();

            new XLGraphicBot().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync() 
        {
            _client = new DiscordSocketClient();
            _client.Log += Log;

            var token = await GetToken();
            if (string.IsNullOrEmpty(token)) return;

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            _client.Ready += OnReady;

            await Task.Delay(-1);
        }

        public async Task<string> GetToken()
        {
	        var tokenPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "token.txt");

	        if (!File.Exists(tokenPath))
	        {
		        Console.WriteLine("Couldn't find token file.");
		        return string.Empty;
	        }

	        using var sr = new StreamReader(tokenPath);
	        return await sr.ReadToEndAsync();
        }

        private async Task OnReady()
        {
			var commandHandler = new CommandHandler(_serviceProvider, _client, new Discord.Commands.CommandService());
			await commandHandler.InstallCommandsAsync();
        }

        private Task Log(LogMessage msg) 
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
