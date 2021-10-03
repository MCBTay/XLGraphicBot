﻿using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

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

            var token = File.ReadAllText("./token.txt");

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
