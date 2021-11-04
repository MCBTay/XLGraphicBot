using System;
using System.Text;
using XLGraphicBot.Localization;

namespace XLGraphicBot.services
{
	public interface IHelpService
	{
		string GetGeneralHelpText();
		string GetDeckHelpText();
		string GetTopsHelpText();
		string GetTopsColorsHelpText();
	}

	public class HelpService : IHelpService
	{
		public string GetGeneralHelpText()
		{
			var sb = new StringBuilder();

			sb.AppendLine(Strings.HelpHeader);
			sb.AppendLine(Strings.General_Description);
			sb.AppendLine(Strings.General_Deck);
			sb.AppendLine(Strings.General_Tops);
			sb.AppendLine(Strings.General_Colors);

			return sb.ToString();
		}

		public string GetDeckHelpText()
		{
			var sb = new StringBuilder();

			sb.AppendLine(Strings.HelpHeader);
			sb.AppendLine(Strings.Deck_Description);
			sb.AppendLine(Strings.Deck_Parameters);
			sb.AppendLine(Strings.Deck_IncludeWear);
			sb.AppendLine(Strings.Deck_MaintainAspectRatio);

			return sb.ToString();
		}

		public string GetTopsHelpText()
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
			
			return sb.ToString();
		}

		public string GetTopsColorsHelpText()
		{
			var sb = new StringBuilder();

			sb.AppendLine(Strings.HelpHeader);
			sb.AppendLine(Strings.TheFullListOfColors);
			sb.AppendLine(Strings.KnownColors);

			return sb.ToString();
		}
	}
}
