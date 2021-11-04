using Discord.Commands;
using System.Threading.Tasks;
using XLGraphicBot.services;

namespace XLGraphicBot
{
	[Group("graphicbot")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        [Group("help")]
        [Alias("?", "halp", "rtfm")]
        public class GeneralHelpModule : ModuleBase<SocketCommandContext>
        {
	        private readonly IHelpService _helpService;

	        public GeneralHelpModule(IHelpService helpService)
	        {
		        _helpService = helpService;
	        }

            [Command]
            public async Task GeneralHelpAsync() 
            {
	            await ReplyAsync(_helpService.GetGeneralHelpText());
            }

            [Command("deck")]
            public async Task DeckHelpAsync()
            {
                await ReplyAsync(_helpService.GetDeckHelpText());
            }

            [Group("tops")]
            public class TopsHelpModule : ModuleBase<SocketCommandContext>
            {
	            private readonly IHelpService _helpService;

	            public TopsHelpModule(IHelpService helpService)
	            {
		            _helpService = helpService;
	            }

                [Command]
				public async Task TopsHelpAsync()
				{
					await ReplyAsync(_helpService.GetTopsHelpText());
				}

                [Command("colors")]
                public async Task ColorsHelpAsync()
                {
                    

                    await ReplyAsync(_helpService.GetTopsColorsHelpText());
                }
            }
        }
    }
}