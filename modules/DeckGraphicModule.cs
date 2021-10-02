using System;
using System.Threading.Tasks;
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
              (attachmentImage, attachmentFileName) = await Utilities.GetMostRecentImage(Context.Channel);
              if (attachmentImage == null || string.IsNullOrEmpty(attachmentFileName)) return;

              template = new Bitmap("./img/templates/deck_template.png");
              if (template == null) return;

              wear = new Bitmap("./img/templates/deck_wear_template.png");
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

              Utilities.DeleteFile(attachmentFilePath);
              Utilities.DeleteFile(deckFilePath);
            }
        }
    }
}