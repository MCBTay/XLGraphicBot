using System;
using System.Threading.Tasks;
using Discord.Commands;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace XLGraphicBot 
{
    [NamedArgumentType]
    public class DeckGraphicModuleArguments 
    {
        public bool IncludeWear { get; set; } = false;
        public bool MaintainAspectRatio { get; set; } = true;
    }
    
    public class DeckGraphicModule : ModuleBase<SocketCommandContext>
    {
        [Command("deck")]
        [Summary("Applies the SkaterXL deck template to the most recent image.")]
        public async Task DeckAsync(DeckGraphicModuleArguments arguments = null)
        {
            Bitmap attachmentImage = null;
            Bitmap template = null;
            Bitmap deck = null;
            Bitmap wearTemplate = null;

            string attachmentFileName = string.Empty;
            string attachmentFilePath = string.Empty;

            string deckFileName = string.Empty;
            string deckFilePath = string.Empty;

            try 
            {
                (attachmentImage, attachmentFileName) = await Utilities.GetMostRecentImage(Context);
                if (attachmentImage == null || string.IsNullOrEmpty(attachmentFileName)) return;

                attachmentFilePath = $"./img/download/{attachmentFileName}";

                template = new Bitmap("./img/templates/deck.png");
                if (template == null) return;

                deck = new Bitmap(template.Width, template.Height);

                using (Graphics g = Graphics.FromImage(deck)) 
                {
                    g.Clear(Color.Transparent);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    var scaledRect = new Rectangle();
                    if (arguments != null && !arguments.MaintainAspectRatio)
                    {
                        scaledRect = new Rectangle(0, 694, template.Width, 482);
                    }
                    else 
                    {
                        scaledRect = Utilities.ScaleImage(attachmentImage, new Rectangle(0, 694, template.Width, 482));
                    }
                    
                    g.DrawImage(attachmentImage, scaledRect);
                    g.DrawImage(template, 0, 0, template.Width, template.Height);

                    if (arguments != null && arguments.IncludeWear)
                    {
                        wearTemplate = new Bitmap("./img/templates/wear.png");
                        if (wearTemplate != null)
                        {
                            g.DrawImage(wearTemplate, 0, 0, template.Width, template.Height);
                        }
                    }
                }

                var deckDirectory = "./img/generated/";
                if (!Directory.Exists(deckDirectory)) Directory.CreateDirectory(deckDirectory);

                deckFilePath = $"{deckDirectory}Deck_{attachmentFileName}.png";
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
                wearTemplate?.Dispose();

                Utilities.DeleteFile(attachmentFilePath);
                Utilities.DeleteFile(deckFilePath);
            }
        }
    }
}