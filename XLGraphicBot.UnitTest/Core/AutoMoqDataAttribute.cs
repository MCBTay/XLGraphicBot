using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace XLGraphicBot.UnitTest.Core
{
	public class AutoMoqDataAttribute : AutoDataAttribute
	{
		public AutoMoqDataAttribute() : base(() => new Fixture().Customize(new AutoMoqCustomization()))
		{

		}
	}
}
