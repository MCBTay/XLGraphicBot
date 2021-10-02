using System;
using System.IO;
using System.Threading.Tasks;
using Discord.Commands;
using System.Drawing;

namespace DeckGraphicBot 
{
    public class ShirtGraphicModule : ModuleBase<SocketCommandContext>
    {
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
              (attachmentImage, attachmentFileName) = await Utilities.GetMostRecentImage(Context.Channel);
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

              Utilities.DeleteFile(attachmentFilePath);
              Utilities.DeleteFile(shirtFilePath);
            }
        }
    }
}