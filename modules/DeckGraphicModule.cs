using Discord.Commands;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
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
            IBitmapService bitmapService,
		    IDiscordService discordService,
		    IFileSystem fileSystem)
		    : base(bitmapService, discordService, fileSystem)
	    {

	    }

		[Command("deck")]
        [Summary("Applies the SkaterXL deck template to the most recent image.")]
        public async Task DeckAsync(DeckGraphicModuleArguments arguments = null)
        {
	        arguments ??= new DeckGraphicModuleArguments();

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

                deck = _bitmapService.ApplyGraphicToTemplate(template, attachmentImage, new Rectangle(0, 694, template.Width, 482), arguments.MaintainAspectRatio);

                if (arguments.IncludeWear)
                {
	                wearTemplate = new Bitmap("./img/templates/wear.png");

	                deck = _bitmapService.ApplyGraphicToTemplate(deck, wearTemplate, new Rectangle(0, 0, template.Width, template.Height));
                }

                var deckDirectory = "./img/generated/";
                if (!_fileSystem.Directory.Exists(deckDirectory)) _fileSystem.Directory.CreateDirectory(deckDirectory);

                deckFilePath = $"{deckDirectory}Deck_{attachmentFileName}.png";

                await _discordService.SendFileAsync(Context, deck, deckFilePath);
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