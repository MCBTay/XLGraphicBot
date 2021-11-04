using Discord.Commands;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;

namespace XLGraphicBot
{
	public class TAShirtGraphicModule : BaseTopGraphicModule
    {
	    public TAShirtGraphicModule(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
		    
	    }

        [Command("tashirt")]
        [Summary("Applies the image to the SkaterXL TAShirt template.")]
        [Alias("ta")]
        public async Task TAShirtAsync(BaseTopGraphicModuleArguments arguments = null)
        {
            var rect = new Rectangle(400, 240, 385, 385);
	        await GenerateGraphicAsync("tashirt", rect, arguments);
        }
    }
}