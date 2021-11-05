using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO.Abstractions;
using System.Net.Http;
using System.Threading.Tasks;

namespace XLGraphicBot.services
{
	public interface IBitmapService
	{
		Task<Bitmap> CreateBitmapFromUrl(string url, string filename);

		void WriteBitmap(Bitmap bitmap, string filePath, ImageFormat imageFormat);

		Bitmap ApplyGraphicToTemplate(Bitmap template, Bitmap graphic, Rectangle rectangle, bool maintainAspectRatio = true);

		Rectangle ScaleImage(int imageWidth, int imageHeight, Rectangle rectangle);
	}

	public class BitmapService : IBitmapService
	{
		private readonly IFileSystem _fileSystem;
		private readonly IHttpClientFactory _httpClientFactory;

		public BitmapService(
			IFileSystem fileSystem, 
			IHttpClientFactory httpClientFactory)
		{
			_fileSystem = fileSystem;
			_httpClientFactory = httpClientFactory;
		}

		public async Task<Bitmap> CreateBitmapFromUrl(string url, string filename)
		{
			using var client = _httpClientFactory.CreateClient();

			var bytes = await client.GetByteArrayAsync(url);

			var directory = "./img/download/";
			if (!_fileSystem.Directory.Exists(directory)) _fileSystem.Directory.CreateDirectory(directory);

			var filePath = $"{directory}{filename}";
			await _fileSystem.File.WriteAllBytesAsync(filePath, bytes);
			
			return new Bitmap(filePath);
		}

		public void WriteBitmap(Bitmap bitmap, string filePath, ImageFormat imageFormat)
		{
			bitmap.Save(filePath, imageFormat);
		}

		public Bitmap ApplyGraphicToTemplate(Bitmap template, Bitmap graphic, Rectangle rectangle, bool maintainAspectRatio = true)
		{
			var result = new Bitmap(template.Width, template.Height);

			using Graphics g = Graphics.FromImage(result);
			g.Clear(Color.Transparent);
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;

			if (template.Size == graphic.Size)
			{
				g.DrawImage(graphic, 0, 0, template.Width, template.Height);
				return result;
			}

			Rectangle scaledRect = rectangle;
			if (maintainAspectRatio)
			{
				scaledRect = ScaleImage(graphic.Width, graphic.Height, rectangle);
			}

			g.DrawImage(graphic, scaledRect);
			g.DrawImage(template, 0, 0, template.Width, template.Height);

			return result;
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
