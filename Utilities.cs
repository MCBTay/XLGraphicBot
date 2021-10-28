using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using System.Drawing;
using Discord.Commands;

namespace XLGraphicBot
{
  public class Utilities
    {
        public static async Task<(Bitmap Image, string fileName)> GetMostRecentImage(SocketCommandContext context) 
        {
              Bitmap image = null;
              var fileName = string.Empty;

              var messages = await context.Channel.GetMessagesAsync(20).FlattenAsync();
              if (messages == null || messages.Count() == 0) return (image, fileName);

              var attachmentMessage = messages.FirstOrDefault(x => x.Attachments.Any() && x.Author.Id != context.Client.CurrentUser.Id);
              var attachment = attachmentMessage?.Attachments?.FirstOrDefault();
              if (attachment == null) return (image, fileName);

              // these will be null according to spec if not an image
              if (attachment.Height == null || attachment.Width == null) return (image, fileName);

              fileName = attachment.Filename;

              using (var client = new HttpClient())
              {
                var bytes = await client.GetByteArrayAsync(attachment.Url);
                
                var filePath = $"./img/download/{fileName}";
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

        public static Rectangle ScaleImage(Bitmap image, Rectangle rectangle)
        {
	        if (image.Width == image.Height) return rectangle;

            if (image.Width > image.Height) 
            {
                float ratio = (float)rectangle.Width / (float)image.Width;

                var newWidth = (int)(image.Width * ratio);
                var newHeight = (int)(image.Height * ratio);

                var newY = rectangle.Y + ((rectangle.Height - newHeight) / 2);

                return new Rectangle(rectangle.X, newY, newWidth, newHeight);
            }
            else if (image.Height > image.Width) 
            {
                float ratio = (float)rectangle.Height / (float)image.Height;
                
                var newWidth = (int)(image.Width * ratio);
                var newHeight = (int)(image.Height * ratio);

                var newX = rectangle.X + ((rectangle.Width - newWidth) / 2);

                return new Rectangle(newX, rectangle.Y, newWidth, newHeight);
            }
        }
    }
}