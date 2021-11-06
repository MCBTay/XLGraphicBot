using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using XLGraphicBot.services;
using XLGraphicBot.UnitTest.Core;
using Xunit;

namespace XLGraphicBot.UnitTest.services
{
	[ExcludeFromCodeCoverage]
	public class BitmapServiceUnitTest
	{
		[Theory, AutoMoqData]
		public void ScaleImage_WidthAndHeightMatch(
			BitmapService sut)
		{
			var rectangle = new Rectangle(0, 0, 500, 500);

			var actual = sut.ScaleImage(100, 100, rectangle);

			actual.Should().Be(rectangle);
		}

		[Theory, AutoMoqData]
		public void ScaleImage_WidthLarger(
			BitmapService sut)
		{
			var rectangle = new Rectangle(0, 0, 500, 500);

			var actual = sut.ScaleImage(200, 100, rectangle);

			actual.Should().Be(new Rectangle(0, 125, 500, 250));
		}

		[Theory, AutoMoqData]
		public void ScaleImage_HeightLarger(
			BitmapService sut)
		{
			var rectangle = new Rectangle(0, 0, 500, 500);

			var actual = sut.ScaleImage(100, 200, rectangle);

			actual.Should().Be(new Rectangle(125, 0, 250, 500));
		}
	}
}
