using Discord;
using Discord.Commands;
using System.Drawing;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using XLGraphicBot.services;

namespace XLGraphicBot.modules
{
	public class BaseGraphicModule : ModuleBase<SocketCommandContext>
	{
		protected readonly IBitmapService _bitmapService;
		protected readonly IFileSystem _fileSystem;

		public BaseGraphicModule(
			IBitmapService bitmapService,
			IFileSystem fileSystem)
		{
			_bitmapService = bitmapService;
			_fileSystem = fileSystem;
		}

		public async Task<(Bitmap Image, string fileName)> GetMostRecentImage(ICommandContext context)
		{
			var messages = await context.Channel.GetMessagesAsync(20).FlattenAsync();
			if (messages == null || !messages.Any()) return (null, string.Empty);

			var attachmentMessage = messages.FirstOrDefault(x => x.Attachments.Any() && x.Author.Id != context.Client.CurrentUser.Id);
			var attachment = attachmentMessage?.Attachments?.FirstOrDefault();
			if (attachment == null) return (null, string.Empty);

			// these will be null according to spec if not an image
			if (attachment.Height == null || attachment.Width == null) return (null, string.Empty);

			var image = await _bitmapService.CreateBitmap(attachment.Url, attachment.Filename);

			return (image, attachment.Filename);
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
