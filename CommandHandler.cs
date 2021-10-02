using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DeckGraphicBot
{
  public class CommandHandler
  {
      private readonly DiscordSocketClient _client;
      private readonly CommandService _commandService;

      public CommandHandler(
        DiscordSocketClient client,
        CommandService commandService)
      {
        _client = client;
        _commandService = commandService;
      }

      public async Task InstallCommandsAsync()
      {
          _client.MessageReceived += HandleCommandAsync;

          await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), null);
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

          await _commandService.ExecuteAsync(context, argPos, null);
      }
  }
}