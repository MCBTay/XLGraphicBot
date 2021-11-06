using AutoFixture.Xunit2;
using Discord.Commands;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using XLGraphicBot.Localization;
using XLGraphicBot.modules;
using XLGraphicBot.services;
using XLGraphicBot.UnitTest.Core;
using Xunit;

namespace XLGraphicBot.UnitTest.modules
{
	[ExcludeFromCodeCoverage]
	public class HelpModuleUnitTest
	{
		[Theory, AutoMoqData]
		public async Task GetGeneralHelpTextTest(
			[Frozen] Mock<IDiscordService> mockDiscordService,
			HelpModule.GeneralHelpModule sut)
		{
			await sut.GeneralHelpAsync();

			mockDiscordService
				.Verify(x => x.SendMessageAsync(
					It.IsAny<ICommandContext>(),
					It.Is<string>(y =>
						y.Contains(Strings.HelpHeader) &&
						y.Contains(Strings.General_Description) &&
						y.Contains(Strings.General_Deck) &&
						y.Contains(Strings.General_Tops) &&
						y.Contains(Strings.General_Colors)),
					false,
					null,
					null,
					null,
					null));
		}

		[Theory, AutoMoqData]
		public async Task GetDeckHelpText(
			[Frozen] Mock<IDiscordService> mockDiscordService,
			HelpModule.GeneralHelpModule sut)
		{
			await sut.DeckHelpAsync();

			mockDiscordService
				.Verify(x => x.SendMessageAsync(
					It.IsAny<ICommandContext>(),
					It.Is<string>(y =>
						y.Contains(Strings.HelpHeader) &&
						y.Contains(Strings.Deck_Description) &&
						y.Contains(Strings.Deck_Parameters) &&
						y.Contains(Strings.Deck_IncludeWear) &&
						y.Contains(Strings.Deck_MaintainAspectRatio)),
					false,
					null,
					null,
					null,
					null));
		}

		[Theory, AutoMoqData]
		public async Task GetTopsHelpTextTest(
			[Frozen] Mock<IDiscordService> mockDiscordService,
			HelpModule.GeneralHelpModule.TopsHelpModule sut)
		{
			await sut.TopsHelpAsync();

			mockDiscordService
				.Verify(x => x.SendMessageAsync(
					It.IsAny<ICommandContext>(),
					It.Is<string>(y =>
						y.Contains(Strings.HelpHeader) &&
						y.Contains(Strings.Tops_Description) &&
						y.Contains(Strings.Tops_AlphaMShirt) &&
						y.Contains(Strings.Tops_BWHoodie) &&
						y.Contains(Strings.Tops_ESShirt) &&
						y.Contains(Strings.Tops_MHoodie) &&
						y.Contains(Strings.Tops_MShirt) &&
						y.Contains(Strings.Tops_MSweater) &&
						y.Contains(Strings.Tops_TAShirt) &&
						y.Contains(Strings.Tops_TLSweater) &&
						y.Contains(Strings.Tops_Parameters) &&
						y.Contains(Strings.Tops_Color) &&
						y.Contains(Strings.Tops_KnownColors) &&
						y.Contains(Strings.Tops_AARRGGBB) &&
						y.Contains(Strings.Tops_RRGGBB) &&
						y.Contains(Strings.Tops_MaintainAspectRatio)),
					false,
					null,
					null,
					null,
					null));
		}

		[Theory, AutoMoqData]
		public async Task GetTopsColorsHelpTextTest(
			[Frozen] Mock<IDiscordService> mockDiscordService,
			HelpModule.GeneralHelpModule.TopsHelpModule sut)
		{
			await sut.ColorsHelpAsync();

			mockDiscordService
				.Verify(x => x.SendMessageAsync(
					It.IsAny<ICommandContext>(),
					It.Is<string>(y =>
						y.Contains(Strings.HelpHeader) &&
						y.Contains(Strings.TheFullListOfColors) &&
						y.Contains(Strings.KnownColors)),
					false,
					null,
					null,
					null,
					null));
		}
	}
}
