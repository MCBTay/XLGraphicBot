using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DeckGraphicBot 
{
    public class DeckGraphicModule : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Summary("Echos a message.")]
        public Task SayAsync(
          [Remainder] 
          [Summary("The text to echo")] 
          string echo) => ReplyAsync(echo);

        [Command("deck")]
        [Summary("Applies the SkaterXL deck template to the most recent image.")]
        public async Task DeckAsync()
        {
          Console.WriteLine("Running deck");

          var messages = await Context.Channel.GetMessagesAsync(20).FlattenAsync();
          if (messages == null || messages.Count() == 0) return;

          Console.WriteLine("Got some messages!");

          var attachmentMessage = messages.FirstOrDefault(x => x.Attachments.Any());
          if (attachmentMessage?.Attachments == null) return;

          var attachment = attachmentMessage.Attachments.FirstOrDefault();
          if (attachmentMessage == null) return;

          Console.WriteLine("Found an image message!");

          await Context.Channel.SendMessageAsync(attachment.Url);

          // Context.Channel.SendFileAsync()
        }
    }
}