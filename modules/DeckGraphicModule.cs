using Discord.Commands;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Threading.Tasks;
using XLGraphicBot.Localization;

namespace XLGraphicBot.modules
{
	[ExcludeFromCodeCoverage]
	[NamedArgumentType]
    public class DeckGraphicModuleArguments 
    {
        public bool IncludeWear { get; set; } = false;
        public bool Stretch { get; set; } = false;
    }
    
    public class DeckGraphicModule : BaseGraphicModule
    {
	    private Bitmap WearTemplate;

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
                (Graphic, attachmentFileName) = await DiscordService.GetMostRecentImage(Context);
                if (Graphic == null || string.IsNullOrEmpty(attachmentFileName))
                {
	                await DiscordService.SendMessageAsync(Context, Strings.UnableToFindImageMessage);
	                return;
                }

                attachmentFilePath = $"./img/download/{attachmentFileName}";

                Template = new Bitmap("./img/templates/deck.png");

                ResultGraphic = BitmapService.ApplyTemplateToGraphic(Template, Graphic, new Rectangle(0, 694, Template.Width, 482), arguments.Stretch);

                if (arguments.IncludeWear)
                {
	                WearTemplate = new Bitmap("./img/templates/wear.png");

	                ResultGraphic = BitmapService.ApplyTemplateToGraphic(WearTemplate, ResultGraphic, new Rectangle(0, 0, Template.Width, Template.Height));
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