using Discord.Commands;
using System.Drawing;
using System.IO.Abstractions;
using System.Threading.Tasks;
using XLGraphicBot.services;

namespace XLGraphicBot.modules.tops
{
	public class TLSweaterGraphicModule : BaseTopGraphicModule
    {
	    public TLSweaterGraphicModule(
		    IDiscordService discordService,
		    IFileSystem fileSystem)
		    : base(discordService, fileSystem)
        {
		    
	    }

        [Command("tlsweater")]
        [Summary("Applies the image to the SkaterXL TLSweater template.")]
        [Alias("tl")]
        public async Task TLSweaterAsync(BaseTopGraphicModuleArguments arguments = null)
        {
            var rect = new Rectangle(1340, 305, 400, 400);
	        await GenerateGraphicAsync("tlsweater", rect, arguments);
        }
    }
}