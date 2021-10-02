using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Drawing;

namespace DeckGraphicBot 
{
    public class DeckGraphicModule : ModuleBase<SocketCommandContext>
    {
        [Command("deck")]
        [Summary("Applies the SkaterXL deck template to the most recent image.")]
        public async Task DeckAsync()
        {
          Bitmap attachmentImage = null;
          Bitmap template = null;
          Bitmap deck = null;
          Bitmap wear = null;

          string attachmentFileName = string.Empty;
          string attachmentFilePath = string.Empty;

          string deckFileName = string.Empty;
          string deckFilePath = string.Empty;

          try 
          {
              (attachmentImage, attachmentFileName) = await GetMostRecentImage();
              if (attachmentImage == null || string.IsNullOrEmpty(attachmentFileName)) return;

              template = new Bitmap("./img/deck_template.png");
              if (template == null) return;

              wear = new Bitmap("./img/deck_wear_template.png");
              if (wear == null) return;

              deck = new Bitmap(template.Width, template.Height);

              using (Graphics g = Graphics.FromImage(deck)) 
              {
                  g.Clear(System.Drawing.Color.Transparent);

                  g.DrawImage(attachmentImage, new Rectangle(0, 694, template.Width, 482));
                  g.DrawImage(template, 0, 0, template.Width, template.Height);
                  g.DrawImage(wear, 0, 0, template.Width, template.Height);
              }

              deckFilePath = $"./Deck_{attachmentFileName}.png";
              deck.Save(deckFilePath, System.Drawing.Imaging.ImageFormat.Png);

              await Context.Channel.SendFileAsync(deckFilePath);
          }
          catch (Exception ex)
          {
            Console.WriteLine(ex.Message);
            throw;
          }
          finally 
          {
              attachmentImage?.Dispose();
              template?.Dispose();
              deck?.Dispose();
              wear?.Dispose();

              if (!string.IsNullOrEmpty(attachmentFilePath)
                  && File.Exists(attachmentFilePath)) 
              {
                  File.Delete(attachmentFilePath);
              }

              if (!string.IsNullOrEmpty(deckFilePath)
                  && File.Exists(deckFilePath))
              {
                  File.Delete(deckFilePath);
              }
            }
        }
    
        [Command("shirt")]
        [Summary("Applies the image to the SkaterXL shirt template.")]
        public async Task ShirtAsync() 
        {
          Bitmap attachmentImage = null;
          Bitmap template = null;
          Bitmap shirt = null;

          string attachmentFileName = string.Empty;
          string attachmentFilePath = string.Empty;

          string shirtFileName = string.Empty;
          string shirtFilePath = string.Empty;

          try 
          {
              (attachmentImage, attachmentFileName) = await GetMostRecentImage();
              if (attachmentImage == null || string.IsNullOrEmpty(attachmentFileName)) return;

              template = new Bitmap("./img/shirt_template.png");
              if (template == null) return;

              shirt = new Bitmap(template.Width, template.Height);

              using (Graphics g = Graphics.FromImage(shirt)) 
              {
                  g.Clear(System.Drawing.Color.Transparent);

                  g.DrawImage(template, 0, 0, template.Width, template.Height);
                  g.DrawImage(attachmentImage, new Rectangle(0, 694, template.Width, 482));  
              }

              shirtFilePath = $"./MShirt_{attachmentFileName}.png";
              shirt.Save(shirtFilePath, System.Drawing.Imaging.ImageFormat.Png);

              await Context.Channel.SendFileAsync(shirtFilePath);
          }
          catch (Exception ex)
          {
            Console.WriteLine(ex.Message);
            throw;
          }
          finally 
          {
              attachmentImage?.Dispose();
              template?.Dispose();
              shirt?.Dispose();

              if (!string.IsNullOrEmpty(attachmentFilePath)
                  && File.Exists(attachmentFilePath)) 
              {
                  File.Delete(attachmentFilePath);
              }

              if (!string.IsNullOrEmpty(shirtFilePath)
                  && File.Exists(shirtFilePath))
              {
                  File.Delete(shirtFilePath);
              }
            }
        }

        private async Task<(Bitmap Image, string fileName)> GetMostRecentImage() 
        {
              Bitmap image = null;
              var fileName = string.Empty;

              var messages = await Context.Channel.GetMessagesAsync(20).FlattenAsync();
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
    }
}