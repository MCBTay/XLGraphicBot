using Discord.Commands;
using System.Drawing;
using System.IO.Abstractions;
using System.Net.Http;
using System.Threading.Tasks;

namespace XLGraphicBot
{
	public class TLSweaterGraphicModule : BaseTopGraphicModule
    {
	    public TLSweaterGraphicModule(
		    IFileSystem fileSystem,
		    IHttpClientFactory httpClientFactory)
		    : base(fileSystem, httpClientFactory)
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