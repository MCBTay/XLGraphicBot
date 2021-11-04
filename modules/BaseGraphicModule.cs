using System.Drawing;
using System.IO.Abstractions;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace XLGraphicBot.modules
{
	public class BaseGraphicModule : ModuleBase<SocketCommandContext>
	{
		protected readonly IFileSystem _fileSystem;
		protected readonly IHttpClientFactory _httpClientFactory;

		public BaseGraphicModule(
			IFileSystem fileSystem, 
			IHttpClientFactory httpClientFactory)
		{
			_fileSystem = fileSystem;
			_httpClientFactory = httpClientFactory;
		}

		public async Task<(Bitmap Image, string fileName)> GetMostRecentImage(ICommandContext context)
		{
			Bitmap image = null;
			var fileName = string.Empty;

			var messages = await context.Channel.GetMessagesAsync(20).FlattenAsync();
			if (messages == null || !messages.Any()) return (image, fileName);

			var attachmentMessage = messages.FirstOrDefault(x => x.Attachments.Any() && x.Author.Id != context.Client.CurrentUser.Id);
			var attachment = attachmentMessage?.Attachments?.FirstOrDefault();
			if (attachment == null) return (image, fileName);

			// these will be null according to spec if not an image
			if (attachment.Height == null || attachment.Width == null) return (image, fileName);

			fileName = attachment.Filename;

			using (var client = _httpClientFactory.CreateClient())
			{
				var bytes = await client.GetByteArrayAsync(attachment.Url);

				var directory = "./img/download/";
				if (!_fileSystem.Directory.Exists(directory)) _fileSystem.Directory.CreateDirectory(directory);

				var filePath = $"{directory}{fileName}";
				await _fileSystem.File.WriteAllBytesAsync(filePath, bytes);
				image = new Bitmap(filePath);
			}

			return (image, fileName);
		}

		public void DeleteFile(string filepath)
		{
			if (string.IsNullOrEmpty(filepath)) return;
			if (!_fileSystem.File.Exists(filepath)) return;

			_fileSystem.File.Delete(filepath);
		}

		public Rectangle ScaleImage(Bitmap image, Rectangle rectangle)
		{
			if (image.Width == image.Height) return rectangle;
			
			if (image.Width > image.Height)
			{
				float ratio = (float)rectangle.Width / (float)image.Width;

				var newWidth = (int)(image.Width * ratio);
				var newHeight = (int)(image.Height * ratio);

				var newY = rectangle.Y + ((rectangle.Height - newHeight) / 2);

				return new Rectangle(rectangle.X, newY, newWidth, newHeight);
			}
			
			if (image.Height > image.Width)
			{
				float ratio = (float)rectangle.Height / (float)image.Height;

				var newWidth = (int)(image.Width * ratio);
				var newHeight = (int)(image.Height * ratio);

				var newX = rectangle.X + ((rectangle.Width - newWidth) / 2);

				return new Rectangle(newX, rectangle.Y, newWidth, newHeight);
			}
			
			return rectangle;
		}
	}
}
