using Discord.Commands;
using System.Drawing;
using System.IO.Abstractions;
using System.Threading.Tasks;
using XLGraphicBot.services;

namespace XLGraphicBot.modules.tops
{
	public class MShirtGraphicModule : BaseTopGraphicModule
    {
	    public MShirtGraphicModule(
		    IBitmapService bitmapService,
		    IFileSystem fileSystem)
		    : base(bitmapService, fileSystem)
		{
		    
	    }

        [Command("mshirt")]
        [Summary("Applies the image to the SkaterXL MShirt template.")]
        [Alias("shirt")]
        public async Task MShirtAsync(BaseTopGraphicModuleArguments arguments = null)
        {
            var rect = new Rectangle(420, 250, 330, 330);
	        await GenerateGraphicAsync("mshirt", rect, arguments);
        }
    }
}