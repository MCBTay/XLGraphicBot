using System;
using System.Threading.Tasks;
using Discord.Commands;
using System.Text;
using XLGraphicBot.Localization;

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

                sb.AppendLine(Strings.HelpHeader);
                sb.AppendLine(Strings.General_Description);
                sb.AppendLine(Strings.General_Deck);
                sb.AppendLine(Strings.General_Tops);
                sb.AppendLine(Strings.General_Colors);

                await ReplyAsync(sb.ToString());
            }

            [Command("deck")]
            public async Task DeckHelpAsync()
            {
                var sb = new StringBuilder();

                sb.AppendLine(Strings.HelpHeader);
                sb.AppendLine(Strings.Deck_Description);
                sb.AppendLine(Strings.Deck_Parameters);
                sb.AppendLine(Strings.Deck_IncludeWear);
                sb.AppendLine(Strings.Deck_MaintainAspectRatio);

                await ReplyAsync(sb.ToString());
            }

            [Group("tops")]
            public class TopsHelpModule : ModuleBase<SocketCommandContext>
            {
	            [Command]
				public async Task TopsHelpAsync()
				{
					var sb = new StringBuilder();

					sb.AppendLine(Strings.HelpHeader);
					sb.AppendLine(Strings.Tops_Description);
					sb.AppendLine(Strings.Tops_AlphaMShirt);
					sb.AppendLine(Strings.Tops_BWHoodie);
					sb.AppendLine(Strings.Tops_ESShirt);
					sb.AppendLine(Strings.Tops_MHoodie);
					sb.AppendLine(Strings.Tops_MShirt);
					sb.AppendLine(Strings.Tops_MSweater);
					sb.AppendLine(Strings.Tops_TAShirt);
					sb.AppendLine(Strings.Tops_TLSweater);
					sb.AppendLine(Strings.Tops_Parameters);
					sb.AppendLine(Strings.Tops_Color);
					sb.AppendLine(Strings.Tops_KnownColors);
					sb.AppendLine(Strings.Tops_AARRGGBB);
					sb.AppendLine(Strings.Tops_RRGGBB);
					sb.AppendLine(Strings.Tops_MaintainAspectRatio);

					await ReplyAsync(sb.ToString());
				}

                [Command("colors")]
                public async Task ColorsHelpAsync()
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(Strings.HelpHeader);
                    sb.AppendLine(Strings.TheFullListOfColors);
                    sb.AppendLine(Strings.KnownColors);

                    await ReplyAsync(sb.ToString());
                }
            }
        }
    }
}