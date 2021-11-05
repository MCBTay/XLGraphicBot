using Discord.Commands;
using System.Drawing;
using System.IO.Abstractions;
using System.Threading.Tasks;
using XLGraphicBot.services;

namespace XLGraphicBot
{
	public class AlphaMShirtGraphicModule : BaseTopGraphicModule
    {
	    public AlphaMShirtGraphicModule(
			IBitmapService bitmapService,
			IFileSystem fileSystem)
		    : base(bitmapService, fileSystem)
		{
		    
	    }

        [Command("alphamshirt")]
        [Summary("Applies the image to the SkaterXL Alpha MShirt template.")]
        [Alias("alpha")]
        public async Task AlphaMShirtAsync(BaseTopGraphicModuleArguments arguments = null)
        {
            var rect = new Rectangle(265, 335, 310, 310);
	        await GenerateGraphicAsync("alphamshirt", rect, arguments);
        }
    }
}