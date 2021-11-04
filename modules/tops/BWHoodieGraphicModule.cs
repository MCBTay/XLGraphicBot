using Discord.Commands;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;

namespace XLGraphicBot
{
	public class BWHoodieGraphicModule : BaseTopGraphicModule
    {
	    public BWHoodieGraphicModule(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
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