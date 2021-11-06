using System;
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
		#region CreateBitmapFromUrl tests

		#endregion

		#region ApplyGraphicToTemplate tests

		#endregion

		#region ReplaceTemplateColor tests
		[Theory]
		[InlineAutoMoqData("black")]
		[InlineAutoMoqData("red")]
		[InlineAutoMoqData("lime")]
		[InlineAutoMoqData("blue")]
		public void ReplaceTemplateColor(string color)
		{
			Bitmap bitmap = new Bitmap(100, 100);

			using Graphics graph = Graphics.FromImage(bitmap);
			Rectangle ImageSize = new Rectangle(0, 0, 100, 100);
			graph.FillRectangle(Brushes.White, ImageSize);

			BitmapService.ReplaceTemplateColor(bitmap, color);

			for (int i = 0; i < 100; i++)
			{
				for (int j = 0; j < 100; j++)
				{
					var pixel = bitmap.GetPixel(i, j);

					Enum.TryParse<KnownColor>(color, true, out var knownColor);

					pixel.ToArgb().Should().Be(Color.FromKnownColor(knownColor).ToArgb());
				}
			}
		}
		#endregion

		#region ParseColor tests
		[Theory]
		[InlineAutoMoqData("black",     255,   0,   0,   0)]
		[InlineAutoMoqData("#000000",   255,   0,   0,   0)]
		[InlineAutoMoqData("#FF000000", 255,   0,   0,   0)]
		[InlineAutoMoqData("red",       255, 255,   0,   0)]
		[InlineAutoMoqData("#FF0000",   255, 255,   0,   0)]
		[InlineAutoMoqData("#FFFF0000", 255, 255,   0,   0)]
		[InlineAutoMoqData("lime",      255,   0, 255,   0)]
		[InlineAutoMoqData("#00FF00",   255,   0, 255,   0)]
		[InlineAutoMoqData("#FF00FF00", 255,   0, 255,   0)]
		[InlineAutoMoqData("blue",      255,   0,   0, 255)]
		[InlineAutoMoqData("#0000FF",   255,   0,   0, 255)]
		[InlineAutoMoqData("#FF0000FF", 255,   0,   0, 255)]
		public void ParseColor(string color, int alpha, int red, int green, int blue)
		{
			BitmapService.ParseColor(color).ToArgb().Should().Be(Color.FromArgb(alpha, red, green, blue).ToArgb());
		}
		#endregion

		#region ScaleImage tests
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
		#endregion
	}
}
