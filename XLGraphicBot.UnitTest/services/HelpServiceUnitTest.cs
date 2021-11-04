using FluentAssertions;
using System.Collections.Generic;
using XLGraphicBot.Localization;
using XLGraphicBot.services;
using XLGraphicBot.UnitTest.Core;
using Xunit;

namespace XLGraphicBot.UnitTest.services
{
	public class HelpServiceUnitTest
	{
		[Theory, AutoMoqData]
		public void GetGeneralHelpTextTest(HelpService sut)
		{
			sut.GetGeneralHelpText().Should().ContainAll(new List<string>
			{
				Strings.HelpHeader,
				Strings.General_Description,
				Strings.General_Deck,
				Strings.General_Tops,
				Strings.General_Colors
			});
		}

		[Theory, AutoMoqData]
		public void GetDeckHelpText(HelpService sut)
		{
			sut.GetDeckHelpText().Should().ContainAll(new List<string>
			{
				Strings.HelpHeader,
				Strings.Deck_Description,
				Strings.Deck_Parameters,
				Strings.Deck_IncludeWear,
				Strings.Deck_MaintainAspectRatio
			});
		}

		[Theory, AutoMoqData]
		public void GetTopsHelpTextTest(HelpService sut)
		{
			sut.GetTopsHelpText().Should().ContainAll(new List<string>
			{
				Strings.HelpHeader,
				Strings.Tops_Description,
				Strings.Tops_AlphaMShirt,
				Strings.Tops_BWHoodie,
				Strings.Tops_ESShirt,
				Strings.Tops_MHoodie,
				Strings.Tops_MShirt,
				Strings.Tops_MSweater,
				Strings.Tops_TAShirt,
				Strings.Tops_TLSweater,
				Strings.Tops_Parameters,
				Strings.Tops_Color,
				Strings.Tops_KnownColors,
				Strings.Tops_AARRGGBB,
				Strings.Tops_RRGGBB,
				Strings.Tops_MaintainAspectRatio
			});
		}

		[Theory, AutoMoqData]
		public void GetTopsColorsHelpTextTest(HelpService sut)
		{
			sut.GetTopsColorsHelpText().Should().ContainAll(new List<string>
			{
				Strings.HelpHeader,
				Strings.TheFullListOfColors,
				Strings.KnownColors
			});
		}
	}
}
