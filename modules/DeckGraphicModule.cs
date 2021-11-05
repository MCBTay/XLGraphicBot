using Discord.Commands;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO.Abstractions;
using System.Threading.Tasks;
using XLGraphicBot.services;

namespace XLGraphicBot.modules
{
	[ExcludeFromCodeCoverage]
	[NamedArgumentType]
    public class DeckGraphicModuleArguments 
    {
        public bool IncludeWear { get; set; } = false;
        public bool MaintainAspectRatio { get; set; } = true;
    }
    
    public class DeckGraphicModule : BaseGraphicModule
    {
	    public DeckGraphicModule(
		    IDiscordService discordService,
		    IFileSystem fileSystem)
		    : base(discordService, fileSystem)
        {

		}

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

            string deckFilePath = string.Empty;

            try 
            {
                (attachmentImage, attachmentFileName) = await _discordService.GetMostRecentImage(Context);
                if (attachmentImage == null || string.IsNullOrEmpty(attachmentFileName)) return;

                attachmentFilePath = $"./img/download/{attachmentFileName}";

                template = new Bitmap("./img/templates/deck.png");
                if (template == null) return;

                deck = new Bitmap(template.Width, template.Height);

                using (Graphics g = Graphics.FromImage(deck)) 
                {
                    g.Clear(Color.Transparent);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    Rectangle scaledRect;
                    if (arguments != null && !arguments.MaintainAspectRatio)
                    {
                        scaledRect = new Rectangle(0, 694, template.Width, 482);
                    }
                    else 
                    {
                        scaledRect = ScaleImage(attachmentImage.Width, attachmentImage.Height, new Rectangle(0, 694, template.Width, 482));
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
                if (!_fileSystem.Directory.Exists(deckDirectory)) _fileSystem.Directory.CreateDirectory(deckDirectory);

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

                DeleteFile(attachmentFilePath);
                DeleteFile(deckFilePath);
            }
        }
    }
}