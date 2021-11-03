# XLGraphicBot
A simple Discord bot utilizing [Discord.Net](https://github.com/discord-net/Discord.Net) to make deck and shirt graphics for SkaterXL based on recently posted images.

XLGraphicBot allows you to take images posted in a channel and turn them into a deck or shirt immediately for SkaterXL.
- `!deck` - Takes the most recent image and places it on a deck template.  For more info, run `!graphicbot help deck`.
- `!alphamshirt`/`!bwhoodie`/`!esshirt`/`!mhoodie`/`!mshirt`/`!msweater`/`!tashirt`/`!tlsweater` - Takes the most recent image and places it on the appropriate shirt/sweater template.  For more info, run `!graphicbot help tops`
  - To see a list of all the colors that can be used with these run `!graphicbot help tops colors`.

# Usage
- `!deck` - Takes the most recent image not posted by XLGraphicBot and places it on a deck template.
  - Has two optional parameters, which can be omitted entirely or used in any combination or order:
    -  `includeWear` - Defaults to `false`.  Overlays a board wear pattern on the texture.  Example usage: `!deck includeWear: true`
    -  `maintainAspectRatio` - Defaults to `true`.  Maintains the original images aspect ratio when scaling down.  Example usage: `!deck maintainAspectRatio: false`

XLGraphicBot supports multiple top types.  They all take the most recent image that was not posted by XLGraphicBot and place it on the appropriate template.  Most have aliases and shortcuts.
- `!alphamshirt`/`!alpha` - Alpha MShirt template.
- `!bwhoodie`/`!bw` - BWHoodie template.
- `!esshirt`/`!es` - ESShirt template.
- `!mhoodie`/`!hoodie` - MHoodie template.
- `!mshirt`/`!shirt` - MShirt template.
- `!msweater`/`!sweater` - MSweater template.
- `!tashirt`/`!ta` - TAShirt template.
- `!tlsweater`/`!tl` - TLSweater template.
- Each of these commands have two optional parameters which can be omitted entirely or used in any combination or order:
  - `color` - Defaults to `null`.  If not passed, the default white of the template will be used.  Can be passed in 3 formats:
    - `Known colors` - English monikers for colors.
      - AliceBlue, AntiqueWhite, Aqua, Aquamarine, Azure, Beige, Bisque, Black, BlanchedAlmond, Blue, BlueViolet, Brown, BurlyWood, CadetBlue, Chartreuse, Chocolate, Coral, CornflowerBlue, Cornsilk, Crimson, Cyan, DarkBlue, DarkCyan, DarkGoldenrod, DarkGray, DarkGreen, DarkKhaki, DarkMagenta, DarkOliveGreen, DarkOrange, DarkOrchid, DarkRed, DarkSalmon, DarkSeaGreen, DarkSlateBlue, DarkSlateGray, DarkTurquoise, DarkViolet, DeepPink, DeepSkyBlue, DimGray, DodgerBlue, Firebrick, FloralWhite, ForestGreen, Fuchsia, Gainsboro, GhostWhite, Gold, Goldenrod, Gray, Green, GreenYellow, Honeydew, HotPink, IndianRed, Indigo, Ivory, Khaki, Lavender, LavenderBlush, LawnGreen, LemonChiffon, LightBlue, LightCoral, LightCyan, LightGoldenrodYellow, LightGray, LightGreen, LightPink, LightSalmon, LightSeaGreen, LightSkyBlue, LightSlateGray, LightSteelBlue, LightYellow, Lime, LimeGreen, Linen, Magenta, Maroon, MediumAquamarine, MediumBlue, MediumOrchid, MediumPurple, MediumSeaGreen, MediumSlateBlue, MediumSpringGreen, MediumTurquoise, MediumVioletRed, MidnightBlue, MintCream, MistyRose, Moccasin, NavajoWhite, Navy, OldLace, Olive, OliveDrab, Orange, OrangeRed, Orchid, PaleGoldenrod, PaleGreen, PaleTurquoise, PaleVioletRed, PapayaWhip, PeachPuff, Peru, Pink, Plum, PowderBlue, Purple, Red, RosyBrown, RoyalBlue, SaddleBrown, Salmon, SandyBrown, SeaGreen, SeaShell, Sienna, Silver, SkyBlue, SlateBlue, SlateGray, Snow, SpringGreen, SteelBlue, Tan, Teal, Thistle, Tomato, Transparent, Turquoise, Violet, Wheat, White, WhiteSmoke, Yellow, and YellowGreen
    - `#AARRGGBB` - Example usage: `!esshirt color: #FFFF00FF`
    - `#RRGGBB` - Example usage: `!tlsweater color: #FF00FF`
  - `maintainAspectRatio` - Defaults to `true`.  Maintains the original images aspect ratio when scaling down.  Example usage: `!mshirt maintainAspectRatio: false`
