using Discord.Commands;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;

namespace XLGraphicBot
{
	public class MSweaterGraphicModule : BaseTopGraphicModule
    {
	    public MSweaterGraphicModule(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
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