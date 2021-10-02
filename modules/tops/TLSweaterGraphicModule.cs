using System;
using System.IO;
using System.Threading.Tasks;
using Discord.Commands;
using System.Drawing;

namespace XLGraphicBot 
{
    public class TLSweaterGraphicModule : BaseTopGraphicModule
    {
        [Command("tlsweater")]
        [Summary("Applies the image to the SkaterXL TLSweater template.")]
        [Alias("tl")]
        public async Task TLSweaterAsync(BaseTopGraphicModuleArguments arguments = null)
        {
            var rect = new Rectangle(1340, 305, 400, 400);
	        await GenerateGraphicAsync("tlsweater", rect, arguments);
        }
    }
}