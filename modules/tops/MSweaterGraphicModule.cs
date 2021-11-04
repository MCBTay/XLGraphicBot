using Discord.Commands;
using System.Drawing;
using System.IO.Abstractions;
using System.Net.Http;
using System.Threading.Tasks;

namespace XLGraphicBot
{
	public class MSweaterGraphicModule : BaseTopGraphicModule
    {
	    public MSweaterGraphicModule(
		    IFileSystem fileSystem,
		    IHttpClientFactory httpClientFactory)
		    : base(fileSystem, httpClientFactory)
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