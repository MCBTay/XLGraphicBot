using System.Drawing;
using System.IO.Abstractions;
using System.Net.Http;
using System.Threading.Tasks;

namespace XLGraphicBot.services
{
	public interface IBitmapService
	{
		Task<Bitmap> CreateBitmap(string url, string filename);
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

		public async Task<Bitmap> CreateBitmap(string url, string filename)
		{
			Bitmap bitmap = null;

			using (var client = _httpClientFactory.CreateClient())
			{
				var bytes = await client.GetByteArrayAsync(url);

				var directory = "./img/download/";
				if (!_fileSystem.Directory.Exists(directory)) _fileSystem.Directory.CreateDirectory(directory);

				var filePath = $"{directory}{filename}";
				await _fileSystem.File.WriteAllBytesAsync(filePath, bytes);
				bitmap = new Bitmap(filePath);
			}

			return bitmap;
		}
	}
}
