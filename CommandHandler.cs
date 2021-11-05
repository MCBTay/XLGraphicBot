using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO.Abstractions;
using System.Reflection;
using System.Threading.Tasks;
using XLGraphicBot.services;

namespace XLGraphicBot
{
	public class CommandHandler
    {
		private readonly DiscordSocketClient _client;
		private readonly CommandService _commandService;

		private readonly IServiceProvider _serviceProvider;

		public CommandHandler(
			DiscordSocketClient client,
			CommandService commandService)
		{
			_serviceProvider = new ServiceCollection()
				.AddHttpClient()
				.AddSingleton<IBitmapService, BitmapService>()
				.AddScoped<IDiscordService, DiscordService>()
				.AddScoped<IFileSystem, FileSystem>()
				.AddSingleton<IHelpService, HelpService>()
				.BuildServiceProvider();

			_client = client;
			_commandService = commandService;
		}

		public async Task InstallCommandsAsync()
		{
			_client.MessageReceived += HandleCommandAsync;

			await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
		}

		private async Task HandleCommandAsync(SocketMessage messageParam)
		{
			var message = messageParam as SocketUserMessage;
			if (message == null) return;

			int argPos = 0;

			if (!(message.HasCharPrefix('!', ref argPos)
				|| message.HasMentionPrefix(_client.CurrentUser, ref argPos))
				|| message.Author.IsBot)
			{
				return;
			}

			var context = new SocketCommandContext(_client, message);

			await _commandService.ExecuteAsync(context, argPos, _serviceProvider);
		}
    }
}