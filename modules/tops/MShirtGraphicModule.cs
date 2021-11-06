using Discord.Commands;
using System.Drawing;
using System.Threading.Tasks;

namespace XLGraphicBot.modules.tops
{
	public class MShirtGraphicModule : BaseTopGraphicModule
    {
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