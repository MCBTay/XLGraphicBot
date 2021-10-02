using System;
using System.IO;
using System.Threading.Tasks;
using Discord.Commands;
using System.Drawing;

namespace XLGraphicBot 
{
    public class AlphaMShirtGraphicModule : BaseTopGraphicModule
    {
        [Command("alphamshirt")]
        [Summary("Applies the image to the SkaterXL Alpha MShirt template.")]
        [Alias("alpha")]
        public async Task AlphaMShirtAsync(string color = null)
        {
            var rect = new Rectangle(265, 335, 310, 310);
	        await GenerateGraphicAsync("alphamshirt", color, rect);
        }
    }
}