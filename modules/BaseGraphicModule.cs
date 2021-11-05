using Discord.Commands;
using System.Drawing;
using System.IO.Abstractions;
using XLGraphicBot.services;

namespace XLGraphicBot.modules
{
	public class BaseGraphicModule : ModuleBase<SocketCommandContext>
	{
		protected readonly IBitmapService _bitmapService;
		protected readonly IDiscordService _discordService;
		protected readonly IFileSystem _fileSystem;

		public BaseGraphicModule(
			IBitmapService bitmapService,
			IDiscordService discordService,
			IFileSystem fileSystem)
		{
			_bitmapService = bitmapService;
			_discordService = discordService;
			_fileSystem = fileSystem;
		}

		public void DeleteFile(string filepath)
		{
			if (string.IsNullOrEmpty(filepath)) return;
			if (!_fileSystem.File.Exists(filepath)) return;

			_fileSystem.File.Delete(filepath);
		}

		public Rectangle ScaleImage(int imageWidth, int imageHeight, Rectangle rectangle)
		{
			if (imageWidth > imageHeight)
			{
				float ratio = (float)rectangle.Width / (float)imageWidth;

				var newWidth = (int)(imageWidth * ratio);
				var newHeight = (int)(imageHeight * ratio);

				var newY = rectangle.Y + ((rectangle.Height - newHeight) / 2);

				return new Rectangle(rectangle.X, newY, newWidth, newHeight);
			}
			
			if (imageHeight > imageWidth)
			{
				float ratio = (float)rectangle.Height / (float)imageHeight;

				var newWidth = (int)(imageWidth * ratio);
				var newHeight = (int)(imageHeight * ratio);

				var newX = rectangle.X + ((rectangle.Width - newWidth) / 2);

				return new Rectangle(newX, rectangle.Y, newWidth, newHeight);
			}
			
			//TODO: if neither of the above two conditions are true, we can scale the iamge using the same ratio in both directions
			return rectangle;
		}
	}
}
