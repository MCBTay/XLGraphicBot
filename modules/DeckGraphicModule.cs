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
	    private Bitmap WearTemplate;
        
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
            
            string attachmentFileName = string.Empty;
            string attachmentFilePath = string.Empty;

            string deckFilePath = string.Empty;

            try 
            {
                (Graphic, attachmentFileName) = await _discordService.GetMostRecentImage(Context);
                if (Graphic == null || string.IsNullOrEmpty(attachmentFileName)) return;

                attachmentFilePath = $"./img/download/{attachmentFileName}";

                Template = new Bitmap("./img/templates/deck.png");

                ResultGraphic = _bitmapService.ApplyGraphicToTemplate(Template, Graphic, new Rectangle(0, 694, Template.Width, 482), arguments.MaintainAspectRatio);

                if (arguments.IncludeWear)
                {
	                WearTemplate = new Bitmap("./img/templates/wear.png");

	                ResultGraphic = _bitmapService.ApplyGraphicToTemplate(ResultGraphic, WearTemplate, new Rectangle(0, 0, Template.Width, Template.Height));
                }

                deckFilePath = await WriteFileAndSend(ResultGraphic, $"Deck_{attachmentFileName}.png");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally 
            {
                Graphic?.Dispose();
                Template?.Dispose();
                ResultGraphic?.Dispose();
                WearTemplate?.Dispose();

                Graphic = null;
                Template = null;
                ResultGraphic = null;
                WearTemplate = null;

                DeleteFile(attachmentFilePath);
                DeleteFile(deckFilePath);
            }
        }
    }
}