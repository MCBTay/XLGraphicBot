using System;
using System.IO;
using System.Threading.Tasks;
using Discord.Commands;
using System.Drawing;

namespace XLGraphicBot 
{
    public class MShirtGraphicModule : BaseTopGraphicModule
    {
        [Command("mshirt")]
        [Summary("Applies the image to the SkaterXL MShirt template.")]
        [Alias("shirt")]
        public async Task MShirtAsync(string color = null)
        {
            var rect = new Rectangle(420, 250, 330, 330);
	        await GenerateGraphicAsync("mshirt", color, rect);
        }
    }
}