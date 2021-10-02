using System;
using System.IO;
using System.Threading.Tasks;
using Discord.Commands;
using System.Drawing;

namespace XLGraphicBot 
{
    public class TAShirtGraphicModule : BaseTopGraphicModule
    {
        [Command("tashirt")]
        [Summary("Applies the image to the SkaterXL TAShirt template.")]
        [Alias("ta")]
        public async Task TAShirtAsync(
	        string color = null)
        {
            var rect = new Rectangle(420, 250, 330, 330);
	        await GenerateGraphicAsync("tashirt", color, rect);
        }
    }
}