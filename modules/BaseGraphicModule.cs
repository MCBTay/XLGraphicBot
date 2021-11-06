using Discord.Commands;
using System.Drawing;
using System.IO.Abstractions;
using System.Threading.Tasks;
using XLGraphicBot.services;

namespace XLGraphicBot.modules
{
	public class BaseGraphicModule : ModuleBase<SocketCommandContext>
	{
		protected readonly IBitmapService _bitmapService;
		protected readonly IDiscordService _discordService;
		protected readonly IFileSystem _fileSystem;

		protected Bitmap Graphic;
		protected Bitmap Template;
		protected Bitmap ResultGraphic;

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

		protected async Task<string> WriteFileAndSend(Bitmap bitmap, string filename)
		{
			var generatedDirectory = "./img/generated/";
			if (!_fileSystem.Directory.Exists(generatedDirectory)) _fileSystem.Directory.CreateDirectory(generatedDirectory);

			var filePath = $"{generatedDirectory}{filename}";

			await _discordService.SendFileAsync(Context, bitmap, filePath);

			return filePath;
		}
	}
}
