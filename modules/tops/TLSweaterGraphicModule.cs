using Discord.Commands;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;

namespace XLGraphicBot
{
	public class TLSweaterGraphicModule : BaseTopGraphicModule
    {
	    public TLSweaterGraphicModule(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
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