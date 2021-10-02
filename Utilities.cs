using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using System.Drawing;
using Discord.WebSocket;

namespace DeckGraphicBot 
{
    public class Utilities
    {
        public static async Task<(Bitmap Image, string fileName)> GetMostRecentImage(ISocketMessageChannel channel) 
        {
              Bitmap image = null;
              var fileName = string.Empty;

              var messages = await channel.GetMessagesAsync(20).FlattenAsync();
              if (messages == null || messages.Count() == 0) return (image, fileName);

              var attachmentMessage = messages.FirstOrDefault(x => x.Attachments.Any());
              var attachment = attachmentMessage?.Attachments?.FirstOrDefault();
              if (attachment == null) return (image, fileName);

              // these will be null according to spec if not an image
              if (attachment.Height == null || attachment.Width == null) return (image, fileName);

              fileName = attachment.Filename;

              using (var client = new HttpClient())
              {
                var bytes = await client.GetByteArrayAsync(attachment.Url);
                
                var filePath = $"./{fileName}";
                File.WriteAllBytes(filePath, bytes);
                image = new Bitmap(filePath);
              }

              return (image, fileName);
        }

        public static void DeleteFile(string filepath) 
        {
            if (string.IsNullOrEmpty(filepath)) return;
            if (!File.Exists(filepath)) return;

            File.Delete(filepath);
        }
    }
}