using Discord.Commands;
using System.Drawing;
using System.IO.Abstractions;
using System.Threading.Tasks;
using XLGraphicBot.services;

namespace XLGraphicBot.modules
{
	public class BaseGraphicModule : ModuleBase<SocketCommandContext>
	{
		public IBitmapService BitmapService { get; set; }
		public IDiscordService DiscordService { get; set; }
		public IFileSystem FileSystem { get; set; }

		protected Bitmap Graphic;
		protected Bitmap Template;
		protected Bitmap ResultGraphic;

		public void DeleteFile(string filepath)
		{
			if (string.IsNullOrEmpty(filepath)) return;
			if (!FileSystem.File.Exists(filepath)) return;

			FileSystem.File.Delete(filepath);
		}

		protected async Task<string> WriteFileAndSend(Bitmap bitmap, string filename)
		{
			var generatedDirectory = "./img/generated/";
			if (!FileSystem.Directory.Exists(generatedDirectory)) FileSystem.Directory.CreateDirectory(generatedDirectory);

			var filePath = $"{generatedDirectory}{filename}";

			await DiscordService.SendFileAsync(Context, bitmap, filePath);

			return filePath;
		}
	}
}
