using Discord.Commands;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Threading.Tasks;

namespace XLGraphicBot.modules.tops
{
	[ExcludeFromCodeCoverage]
	[NamedArgumentType]
    public class BaseTopGraphicModuleArguments 
    {
        public string Color { get; set; }
        public bool MaintainAspectRatio { get; set; } = true;
    }

    public class BaseTopGraphicModule : BaseGraphicModule
    {
        public async Task GenerateGraphicAsync(string templateName, Rectangle rectangle, BaseTopGraphicModuleArguments arguments)
        {
	        arguments ??= new BaseTopGraphicModuleArguments();

            string attachmentFileName = string.Empty;
            string attachmentFilePath = string.Empty;

            string shirtFilePath = string.Empty;

            try
            {
                (Graphic, attachmentFileName) = await DiscordService.GetMostRecentImage(Context);
                if (Graphic == null || string.IsNullOrEmpty(attachmentFileName)) return;

                attachmentFilePath = $"./img/download/{attachmentFileName}";

                Template = new Bitmap($"./img/templates/tops/{templateName}.png");

                ResultGraphic = BitmapService.ApplyGraphicToTemplate(Template, Graphic, rectangle, arguments.MaintainAspectRatio, arguments.Color);

                shirtFilePath = await WriteFileAndSend(ResultGraphic, $"{templateName}_{attachmentFileName}.png");
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

                Graphic = null;
                Template = null;
                ResultGraphic = null;

                DeleteFile(attachmentFilePath);
                DeleteFile(shirtFilePath);
            }
        }
    }
}