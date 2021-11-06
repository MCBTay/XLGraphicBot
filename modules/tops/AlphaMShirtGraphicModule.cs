using Discord.Commands;
using System.Drawing;
using System.Threading.Tasks;

namespace XLGraphicBot.modules.tops
{
	public class AlphaMShirtGraphicModule : BaseTopGraphicModule
    {
        [Command("alphamshirt")]
        [Summary("Applies the image to the SkaterXL Alpha MShirt template.")]
        [Alias("alpha")]
        public async Task AlphaMShirtAsync(BaseTopGraphicModuleArguments arguments = null)
        {
            var rect = new Rectangle(265, 335, 310, 310);
	        await GenerateGraphicAsync("alphamshirt", rect, arguments);
        }
    }
}