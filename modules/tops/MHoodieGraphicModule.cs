using Discord.Commands;
using System.Drawing;
using System.IO.Abstractions;
using System.Threading.Tasks;
using XLGraphicBot.services;

namespace XLGraphicBot.modules.tops
{
	public class MHoodieGraphicModule : BaseTopGraphicModule
    {
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