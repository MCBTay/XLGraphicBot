using System;
using System.IO;
using System.Threading.Tasks;
using Discord.Commands;
using System.Drawing;

namespace XLGraphicBot 
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