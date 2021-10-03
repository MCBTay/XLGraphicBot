using System;
using System.IO;
using System.Threading.Tasks;
using Discord.Commands;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace XLGraphicBot
{
    [NamedArgumentType]
    public class BaseTopGraphicModuleArguments 
    {
        public string Color { get; set; }
        public bool MaintainAspectRatio { get; set; } = true;
    }

    public class BaseTopGraphicModule : ModuleBase<SocketCommandContext>
    {
        private Bitmap attachmentImage;
        protected Bitmap template;
        private Bitmap shirt;

	    public async Task GenerateGraphicAsync(string templateName, Rectangle rectangle, BaseTopGraphicModuleArguments arguments)
        {
            string attachmentFileName = string.Empty;
            string attachmentFilePath = string.Empty;

            string shirtFileName = string.Empty;
            string shirtFilePath = string.Empty;

            try
            {
                (attachmentImage, attachmentFileName) = await Utilities.GetMostRecentImage(Context);
                if (attachmentImage == null || string.IsNullOrEmpty(attachmentFileName)) return;

                attachmentFilePath = $"./img/download/{attachmentFileName}";

                template = new Bitmap($"./img/templates/tops/{templateName}.png");
                if (template == null) return;

                shirt = new Bitmap(template.Width, template.Height);

                using (Graphics g = Graphics.FromImage(shirt))
                {
	                g.Clear(Color.Transparent);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    g.DrawImage(template, 0, 0, template.Width, template.Height);

                    ReplaceTemplateColor(arguments?.Color);

                    var scaledRect = new Rectangle();
                    if (arguments != null && !arguments.MaintainAspectRatio)
                    {
                        scaledRect = rectangle;
                    }
                    else 
                    {
                        scaledRect = Utilities.ScaleImage(attachmentImage, rectangle);
                    }

                    g.DrawImage(attachmentImage, scaledRect);
                }

                shirtFilePath = $"./img/generated/{templateName}_{attachmentFileName}.png";
                shirt.Save(shirtFilePath, ImageFormat.Png);

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

        private void ReplaceTemplateColor(string color)
        {
            if (string.IsNullOrEmpty(color)) return;
	                
            Color parsedColor = Color.White;

            if (color.StartsWith('#')) 
            {
                parsedColor = ColorTranslator.FromHtml(color);
            }
            else 
            {
                parsedColor = Color.FromName(color);
            }		                

            for (int i = 0; i < shirt.Width; i++) 
            {
                for (int j = 0; j < shirt.Height; j++) 
                {
                    var pixel = shirt.GetPixel(i, j);

                    if (pixel.R == 255 && pixel.G == 255 && pixel.B == 255) 
                    {
                        shirt.SetPixel(i, j, parsedColor);
                    }
                }
            }
        }
    }
}