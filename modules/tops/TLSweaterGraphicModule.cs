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
        [Alias("sweater")]
        public async Task TLSweaterAsync(
	        string color = null)
        {
            var rect = new Rectangle(420, 250, 330, 330);
	        await GenerateGraphicAsync("tlsweater", color, rect);
        }
    }
}