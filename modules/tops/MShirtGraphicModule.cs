using Discord.Commands;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;

namespace XLGraphicBot
{
	public class MShirtGraphicModule : BaseTopGraphicModule
    {
	    public MShirtGraphicModule(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
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