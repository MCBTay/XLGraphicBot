using Discord.Commands;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;

namespace XLGraphicBot
{
	public class MHoodieGraphicModule : BaseTopGraphicModule
    {
	    public MHoodieGraphicModule(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
		    
	    }

        [Command("mhoodie")]
        [Summary("Applies the image to the SkaterXL MHoodie template.")]
        [Alias("hoodie")]
        public async Task MHoodieAsync(BaseTopGraphicModuleArguments arguments = null)
        {
            var rect = new Rectangle(1350, 400, 330, 330);
	        await GenerateGraphicAsync("mhoodie", rect, arguments);
        }
    }
}