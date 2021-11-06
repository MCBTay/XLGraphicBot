using System.Text;
using Discord.Commands;
using System.Threading.Tasks;
using XLGraphicBot.Localization;
using XLGraphicBot.services;

namespace XLGraphicBot.modules
{
	[Group("graphicbot")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        [Group("help")]
        [Alias("?", "halp", "rtfm")]
        public class GeneralHelpModule : ModuleBase<SocketCommandContext>
        {
	        private readonly IDiscordService _discordService;

	        public GeneralHelpModule(IDiscordService discordService)
	        {
		        _discordService = discordService;
	        }

            [Command]
            public async Task GeneralHelpAsync() 
            {
	            var sb = new StringBuilder();

	            sb.AppendLine(Strings.HelpHeader);
	            sb.AppendLine(Strings.General_Description);
	            sb.AppendLine(Strings.General_Deck);
	            sb.AppendLine(Strings.General_Tops);
	            sb.AppendLine(Strings.General_Colors);

	            await _discordService.SendMessageAsync(Context, sb.ToString());
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

				await _discordService.SendMessageAsync(Context, sb.ToString());
			}

            [Group("tops")]
            public class TopsHelpModule : ModuleBase<SocketCommandContext>
            {
				private readonly IDiscordService _discordService;

				public TopsHelpModule(IDiscordService discordService)
	            {
					_discordService = discordService;
				}

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

					await _discordService.SendMessageAsync(Context, sb.ToString());
				}

                [Command("colors")]
                public async Task ColorsHelpAsync()
                {
	                var sb = new StringBuilder();

	                sb.AppendLine(Strings.HelpHeader);
	                sb.AppendLine(Strings.TheFullListOfColors);
	                sb.AppendLine(Strings.KnownColors);

					await _discordService.SendMessageAsync(Context, sb.ToString());
				}
            }
        }
    }
}