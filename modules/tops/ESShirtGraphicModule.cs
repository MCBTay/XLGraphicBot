using Discord.Commands;
using System.Drawing;
using System.Threading.Tasks;

namespace XLGraphicBot.modules.tops
{
	public class ESShirtGraphicModule : BaseTopGraphicModule
    {
        [Command("esshirt")]
        [Summary("Applies the image to the SkaterXL ESShirt template.")]
        [Alias("es")]
        public async Task ESShirtAsync(BaseTopGraphicModuleArguments arguments = null)
        {
            var rect = new Rectangle(460, 260, 320, 320);
	        await GenerateGraphicAsync("esshirt", rect, arguments);
        }
    }
}