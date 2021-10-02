using System;
using System.IO;
using System.Threading.Tasks;
using Discord.Commands;
using System.Drawing;

namespace DeckGraphicBot 
{
    // 350, 156 is rough upper left boundary
    // 820, 390 is rough right boundary
    // 584, 624 is rough center point of where we want the graphic to land on front
    public class MShirtGraphicModule : BaseTopGraphicModule
    {
        [Command("mshirt")]
        [Summary("Applies the image to the SkaterXL MShirt template.")]
        public async Task MShirtAsync(
	        string color = null)
        {
	        var rect = new Rectangle(420, 250, 330, 330);
	        await GenerateGraphicAsync("mshirt", color, rect);
        }
    }
}