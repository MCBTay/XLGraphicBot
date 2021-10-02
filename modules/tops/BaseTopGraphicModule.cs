using System;
using System.IO;
using System.Threading.Tasks;
using Discord.Commands;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace XLGraphicBot
{
    public class BaseTopGraphicModule : ModuleBase<SocketCommandContext>
    {
        private Bitmap attachmentImage;
        protected Bitmap template;
        private Bitmap shirt;

	    public async Task GenerateGraphicAsync(string templateName, string color, Rectangle rectangle)
        {
            string attachmentFileName = string.Empty;
            string attachmentFilePath = string.Empty;

            string shirtFileName = string.Empty;
            string shirtFilePath = string.Empty;

            try
            {
                (attachmentImage, attachmentFileName) = await Utilities.GetMostRecentImage(Context.Channel);
                if (attachmentImage == null || string.IsNullOrEmpty(attachmentFileName)) return;

                template = new Bitmap($"./img/templates/tops/{templateName}.png");
                if (template == null) return;

                shirt = new Bitmap(template.Width, template.Height);

                using (Graphics g = Graphics.FromImage(shirt))
                {
	                g.Clear(Color.Transparent);

                    g.DrawImage(template, 0, 0, template.Width, template.Height);

                    ReplaceTemplateColor(color);

                    if (attachmentImage.Width > attachmentImage.Height) 
                    {
                        float ratio = (float)rectangle.Width / (float)attachmentImage.Width;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        var newWidth = (int)(attachmentImage.Width * ratio);
                        var newHeight = (int)(attachmentImage.Height * ratio);

                        var newY = rectangle.Y + ((rectangle.Height - newHeight) / 2);

                        var newRect = new Rectangle(rectangle.X, newY, newWidth, newHeight);
                        g.DrawImage(attachmentImage, newRect);
                    }
                    else if (attachmentImage.Height > attachmentImage.Width) 
                    {
                        float ratio = (float)rectangle.Height / (float)attachmentImage.Height;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        var newWidth = (int)(attachmentImage.Width * ratio);
                        var newHeight = (int)(attachmentImage.Height * ratio);

                        var newX = rectangle.X + ((rectangle.Width - newWidth) / 2);

                        var newRect = new Rectangle(newX, rectangle.Y, newWidth, newHeight);
                        g.DrawImage(attachmentImage, newRect);
                    }
                    else
                    {
                        g.DrawImage(attachmentImage, rectangle);
                    }
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