using Discord.Commands;
using System.Drawing;
using System.IO.Abstractions;
using System.Threading.Tasks;
using XLGraphicBot.services;

namespace XLGraphicBot
{
	public class MSweaterGraphicModule : BaseTopGraphicModule
    {
	    public MSweaterGraphicModule(
		    IBitmapService bitmapService,
		    IFileSystem fileSystem)
		    : base(bitmapService, fileSystem)
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