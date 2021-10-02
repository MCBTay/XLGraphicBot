using System;
using System.IO;
using System.Threading.Tasks;
using Discord.Commands;
using System.Drawing;

namespace XLGraphicBot 
{
    public class MSweaterGraphicModule : BaseTopGraphicModule
    {
        [Command("msweater")]
        [Summary("Applies the image to the SkaterXL MSweater template.")]
        [Alias("sweater")]
        public async Task MSweaterAsync(BaseTopGraphicModuleArguments arguments = null)
        {
            var rect = new Rectangle(400, 1120, 350, 350);
	        await GenerateGraphicAsync("msweater", rect, arguments);
        }
    }
}