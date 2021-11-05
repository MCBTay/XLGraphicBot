using Discord.Commands;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using XLGraphicBot.modules;
using XLGraphicBot.services;

namespace XLGraphicBot
{
	[NamedArgumentType]
    public class BaseTopGraphicModuleArguments 
    {
        public string Color { get; set; }
        public bool MaintainAspectRatio { get; set; } = true;
    }

    public class BaseTopGraphicModule : BaseGraphicModule
    {
        private Bitmap attachmentImage;
        protected Bitmap template;
        private Bitmap shirt;

        public BaseTopGraphicModule(
            IBitmapService bitmapService,
	        IFileSystem fileSystem)
	        : base(bitmapService, fileSystem)
        {

        }

        public async Task GenerateGraphicAsync(string templateName, Rectangle rectangle, BaseTopGraphicModuleArguments arguments)
        {
            string attachmentFileName = string.Empty;
            string attachmentFilePath = string.Empty;

            string shirtFileName = string.Empty;
            string shirtFilePath = string.Empty;

            try
            {
                (attachmentImage, attachmentFileName) = await GetMostRecentImage(Context);
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
                        scaledRect = ScaleImage(attachmentImage.Width, attachmentImage.Height, rectangle);
                    }

                    g.DrawImage(attachmentImage, scaledRect);
                }

                var shirtDirectory = "./img/generated/";
                if (!Directory.Exists(shirtDirectory)) Directory.CreateDirectory(shirtDirectory);

                shirtFilePath = $"{shirtDirectory}{templateName}_{attachmentFileName}.png";
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

                DeleteFile(attachmentFilePath);
                DeleteFile(shirtFilePath);
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