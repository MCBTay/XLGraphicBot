using Discord.Commands;
using System.Drawing;
using System.Threading.Tasks;

namespace XLGraphicBot.modules.tops
{
	public class TAShirtGraphicModule : BaseTopGraphicModule
    {
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