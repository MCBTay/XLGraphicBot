using System;
using System.IO;
using System.Threading.Tasks;
using Discord.Commands;
using System.Drawing;
using System.Text;

namespace XLGraphicBot 
{
    [Group("graphicbot")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        [Summary("Spits out help info.")]
        [Alias("?", "halp", "rtfm")]
        public async Task HelpAsync()
        {
            var sb = new StringBuilder();

            sb.Append("**XLGraphicBot Help**" + Environment.NewLine);

            sb.Append("- `!deck` - Takes the most recent image not posted by XLGraphicBot and places it on a deck template." + Environment.NewLine);
            sb.Append("  - Has two optional parameters, which can be omitted entirely or used in any combination or order:" + Environment.NewLine);
            sb.Append("    - `includeWear` - Defaults to `false`.  Overlays a board wear pattern on the texture.  Example usage: `!deck includeWear: true`" + Environment.NewLine);
            sb.Append("    - `maintainAspectRatio` - Defaults to `true`.  Maintains the original images aspect ratio when scaling down.  Example usage: `!deck maintainAspectRatio: false`" + Environment.NewLine);
            sb.Append("- `!alphamshirt`/`!alpha` - " + Environment.NewLine);
            sb.Append("- `!bwhoodie`/`!bw` - " + Environment.NewLine);
            sb.Append("- `!esshirt`/`!es` - " + Environment.NewLine);
            sb.Append("- `!mhoodie`/`!hoodie` - " + Environment.NewLine);
            sb.Append("- `!mshirt`/`!shirt` - " + Environment.NewLine);
            sb.Append("- `!msweater`/`!sweater` - " + Environment.NewLine);
            sb.Append("- `!tashirt`/`!ta` - " + Environment.NewLine);
            sb.Append("- `!tlsweater`/`!tl` - " + Environment.NewLine);

            await ReplyAsync(sb.ToString());
        }
    }
}