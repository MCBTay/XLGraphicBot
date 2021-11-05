using Discord.Commands;
using System.Drawing;
using System.IO.Abstractions;
using System.Threading.Tasks;
using XLGraphicBot.services;

namespace XLGraphicBot.modules.tops
{
	public class MSweaterGraphicModule : BaseTopGraphicModule
    {
	    public MSweaterGraphicModule(
		    IDiscordService discordService,
		    IFileSystem fileSystem)
		    : base(discordService, fileSystem)
        {
		    
	    }

        [Command("msweater")]
        [Summary("Applies the image to the SkaterXL MSweater template.")]
        [Alias("sweater")]
        public async Task MSweaterAsync(BaseTopGraphicModuleArguments arguments = null)
        {
            var rect = new Rectangle(400, 1120, 350, 350);
	        await GenerateGraphicAsync("msweater", rect, arguments);
        }
    }
}