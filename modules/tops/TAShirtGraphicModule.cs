using Discord.Commands;
using System.Drawing;
using System.IO.Abstractions;
using System.Threading.Tasks;
using XLGraphicBot.services;

namespace XLGraphicBot.modules.tops
{
	public class TAShirtGraphicModule : BaseTopGraphicModule
    {
	    public TAShirtGraphicModule(
		    IDiscordService discordService,
		    IFileSystem fileSystem)
		    : base(discordService, fileSystem)
        {
		    
	    }

        [Command("tashirt")]
        [Summary("Applies the image to the SkaterXL TAShirt template.")]
        [Alias("ta")]
        public async Task TAShirtAsync(BaseTopGraphicModuleArguments arguments = null)
        {
            var rect = new Rectangle(400, 240, 385, 385);
	        await GenerateGraphicAsync("tashirt", rect, arguments);
        }
    }
}