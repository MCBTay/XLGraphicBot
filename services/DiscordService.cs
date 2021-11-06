using Discord;
using Discord.Commands;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace XLGraphicBot.services
{
	public interface IDiscordService
	{
		Task<(Bitmap Image, string fileName)> GetMostRecentImage(ICommandContext context);

		Task<IUserMessage> SendFileAsync(ICommandContext commandContext, Bitmap bitmap, string filePath);

		Task<IUserMessage> SendMessageAsync(
			ICommandContext commandContext, 
			string message,
			bool isTTS = false,
			Embed embed = null,
			RequestOptions options = null,
			AllowedMentions allowedMentions = null,
			MessageReference messageReference = null);
	}

	public class DiscordService : IDiscordService
	{
		private readonly IBitmapService _bitmapService;

		public DiscordService(IBitmapService bitmapService)
		{
			_bitmapService = bitmapService;
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

			var image = await _bitmapService.CreateBitmapFromUrl(attachment.Url, attachment.Filename);

			return (image, attachment.Filename);
		}

		public async Task<IUserMessage> SendFileAsync(ICommandContext commandContext, Bitmap bitmap, string filePath)
		{
			_bitmapService.WriteBitmap(bitmap, filePath, System.Drawing.Imaging.ImageFormat.Png);

			return await commandContext.Channel.SendFileAsync(filePath);
		}

		public async Task<IUserMessage> SendMessageAsync(
			ICommandContext commandContext, 
			string message,
			bool isTTS = false,
			Embed embed = null,
			RequestOptions options = null,
			AllowedMentions allowedMentions = null,
			MessageReference messageReference = null)
		{
			return await commandContext.Channel.SendMessageAsync(message, isTTS, embed, options, allowedMentions, messageReference);
		}
	}
}
