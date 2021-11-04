using AutoFixture.Xunit2;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace XLGraphicBot.UnitTest.Core
{
	[ExcludeFromCodeCoverage]
	public class InlineAutoMoqDataAttribute : CompositeDataAttribute
	{
		public InlineAutoMoqDataAttribute(params object[] values)
			: base(new InlineDataAttribute(values), new AutoMoqDataAttribute())
		{
		}
	}
}
