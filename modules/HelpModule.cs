using System;
using System.Threading.Tasks;
using Discord.Commands;
using System.Text;

namespace XLGraphicBot
{
  [Group("graphicbot")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        [Group("help")]
        [Alias("?", "halp", "rtfm")]
        public class GeneralHelpModule : ModuleBase<SocketCommandContext>
        {
            [Command]
            public async Task GeneralHelpAsync() 
            {
                var sb = new StringBuilder();

                sb.Append("**XLGraphicBot Help**" + Environment.NewLine);
                sb.Append("XLGraphicBot allows you to take images posted in a channel and turn them into a deck or shirt immediately for SkaterXL." + Environment.NewLine);
                sb.Append("- `!deck` - Takes the most recent image and places it on a deck template.  For more info, run `!graphicbot help deck`." + Environment.NewLine);
                sb.Append("- `!alphamshirt`/`!bwhoodie`/`!esshirt`/`!mhoodie`/`!mshirt`/`!msweater`/`!tashirt`/`!tlsweater` - Takes the most recent image and places it on the appropriate shirt/sweater template.  For more info, run `!graphicbot help tops`" + Environment.NewLine);
                sb.Append("  - To see a list of all the colors that can be used with these run `!graphicbot help tops colors`.");

                await ReplyAsync(sb.ToString());
            }

            [Command("deck")]
            public async Task DeckHelpAsync()
            {
                var sb = new StringBuilder();

                sb.Append("**XLGraphicBot Help**" + Environment.NewLine);
                sb.Append("- `!deck` - Takes the most recent image not posted by XLGraphicBot and places it on a deck template." + Environment.NewLine);
                sb.Append("  - Has two optional parameters, which can be omitted entirely or used in any combination or order:" + Environment.NewLine);
                sb.Append("    - `includeWear` - Defaults to `false`.  Overlays a board wear pattern on the texture.  Example usage: `!deck includeWear: true`" + Environment.NewLine);
                sb.Append("    - `maintainAspectRatio` - Defaults to `true`.  Maintains the original images aspect ratio when scaling down.  Example usage: `!deck maintainAspectRatio: false`" + Environment.NewLine);

                await ReplyAsync(sb.ToString());
            }

            [Group("tops")]
            public class TopsHelpModule : ModuleBase<SocketCommandContext>
            {

              [Command]
              public async Task TopsHelpAsync()
              {
                  var sb = new StringBuilder();

                  sb.Append("**XLGraphicBot Help**" + Environment.NewLine);
                  sb.Append("XLGraphicBot supports multiple top types.  They all take the most recent image that was not posted by XLGraphicBot and place it on the appropriate template.  Most have aliases and shortcuts." + Environment.NewLine);
                  sb.Append("- `!alphamshirt`/`!alpha` - Alpha MShirt template." + Environment.NewLine);
                  sb.Append("- `!bwhoodie`/`!bw` - BWHoodie template." + Environment.NewLine);
                  sb.Append("- `!esshirt`/`!es` - ESShirt template." + Environment.NewLine);
                  sb.Append("- `!mhoodie`/`!hoodie` - MHoodie template." + Environment.NewLine);
                  sb.Append("- `!mshirt`/`!shirt` - MShirt template." + Environment.NewLine);
                  sb.Append("- `!msweater`/`!sweater` - MSweater template." + Environment.NewLine);
                  sb.Append("- `!tashirt`/`!ta` - TAShirt template." + Environment.NewLine);
                  sb.Append("- `!tlsweater`/`!tl` - TLSweater template." + Environment.NewLine);
                  sb.Append("  - Each of these commands have two optional parameters which can be omitted entirely or used in any combination or order:" + Environment.NewLine);
                  sb.Append("    - `color` - Defaults to `null`.  If not passed, the default white of the template will be used.  Can be passed in 3 formats:" + Environment.NewLine);
                  sb.Append("      - `Known colors` - English monikers for colors.  To see the full list run `!graphicbot help tops colors`." + Environment.NewLine);
                  sb.Append("      - `#AARRGGBB` - Example usage: `!esshirt color: #FFFF00FF`" + Environment.NewLine);
                  sb.Append("      - `#RRGGBB` - Example usage: `!tlsweater color: #FF00FF`" + Environment.NewLine);
                  sb.Append("    - `maintainAspectRatio` - Defaults to `true`.  Maintains the original images aspect ratio when scaling down.  Example usage: `!mshirt maintainAspectRatio: false`" + Environment.NewLine);

                  await ReplyAsync(sb.ToString());
                }

                [Command("colors")]
                public async Task ColorsHelpAsync()
                {
                    var sb = new StringBuilder();

                    sb.Append("The full list of colors that can be used with any of the tops commands:" + Environment.NewLine);
                    sb.Append("AliceBlue, AntiqueWhite, Aqua, Aquamarine, Azure, Beige, Bisque, Black, BlanchedAlmond, Blue, BlueViolet, Brown, BurlyWood, CadetBlue, Chartreuse, Chocolate, Coral, CornflowerBlue, Cornsilk, Crimson, Cyan, ");
                    sb.Append("DarkBlue, DarkCyan, DarkGoldenrod, DarkGray, DarkGreen, DarkKhaki, DarkMagenta, DarkOliveGreen, DarkOrange, DarkOrchid, DarkRed, DarkSalmon, DarkSeaGreen, DarkSlateBlue, DarkSlateGray, DarkTurquoise, DarkViolet, DeepPink, ");
                    sb.Append("DeepSkyBlue, DimGray, DodgerBlue, Firebrick, FloralWhite, ForestGreen, Fuchsia, Gainsboro, GhostWhite, Gold, Goldenrod, Gray, Green, GreenYellow, Honeydew, HotPink, IndianRed, Indigo, Ivory, Khaki, Lavender, LavenderBlush, ");
                    sb.Append("LawnGreen, LemonChiffon, LightBlue, LightCoral, LightCyan, LightGoldenrodYellow, LightGray, LightGreen, LightPink, LightSalmon, LightSeaGreen, LightSkyBlue, LightSlateGray, LightSteelBlue, LightYellow, Lime, LimeGreen, Linen, ");
                    sb.Append("Magenta, Maroon, MediumAquamarine, MediumBlue, MediumOrchid, MediumPurple, MediumSeaGreen, MediumSlateBlue, MediumSpringGreen, MediumTurquoise, MediumVioletRed, MidnightBlue, MintCream, MistyRose, Moccasin, NavajoWhite, Navy, ");
                    sb.Append("OldLace, Olive, OliveDrab, Orange, OrangeRed, Orchid, PaleGoldenrod, PaleGreen, PaleTurquoise, PaleVioletRed, PapayaWhip, PeachPuff, Peru, Pink, Plum, PowderBlue, Purple, Red, RosyBrown, RoyalBlue, SaddleBrown, Salmon, SandyBrown, ");
                    sb.Append("SeaGreen, SeaShell, Sienna, Silver, SkyBlue, SlateBlue, SlateGray, Snow, SpringGreen, SteelBlue, Tan, Teal, Thistle, Tomato, Transparent, Turquoise, Violet, Wheat, White, WhiteSmoke, Yellow, and YellowGreen" + Environment.NewLine);

                    await ReplyAsync(sb.ToString());
                }
            }
        }
    }
}