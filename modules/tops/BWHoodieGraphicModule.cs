using Discord.Commands;
using System.Drawing;
using System.IO.Abstractions;
using System.Net.Http;
using System.Threading.Tasks;

namespace XLGraphicBot
{
    public class BWHoodieGraphicModule : BaseTopGraphicModule
    {
	    public BWHoodieGraphicModule(
		    IFileSystem fileSystem,
		    IHttpClientFactory httpClientFactory)
		    : base(fileSystem, httpClientFactory)
        {
		    
	    }

        [Command("bwhoodie")]
        [Summary("Applies the image to the SkaterXL BWHoodie template.")]
        [Alias("bw")]
        public async Task BWHoodieAsync(BaseTopGraphicModuleArguments arguments = null)
        {
            var rect = new Rectangle(265, 225, 325, 325);
	        await GenerateGraphicAsync("bwhoodie", rect, arguments);
        }
    }
}