using System;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Abstractions;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Moq;
using XLGraphicBot.services;
using XLGraphicBot.UnitTest.Core;
using Xunit;

namespace XLGraphicBot.UnitTest.services
{
	[ExcludeFromCodeCoverage]
	public class BitmapServiceUnitTest
	{
		#region CreateBitmapFromUrl tests
		[Theory, AutoMoqData]
		public async Task CreateBitmapFromUrl_DirNotFound(
			Uri url,
			string filename,
			Mock<HttpMessageHandler> mockHttpMessageHandler,
			[Frozen] Mock<IFileSystem> mockFileSystem,
			[Frozen] Mock<IHttpClientFactory> mockHttpClientFactory,
			BitmapService sut)
		{
			mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient(mockHttpMessageHandler.Object));

			mockFileSystem.Setup(x => x.Directory.Exists("./img/download/")).Returns(false);

			var actual = await sut.CreateBitmapFromUrl(url.ToString(), filename);

			actual.Should().NotBeNull();

			mockFileSystem.Verify(x => x.Directory.CreateDirectory("./img/download/"));
			mockFileSystem.Verify(x => x.File.WriteAllBytesAsync($"./img/download/{filename}", It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));
		}

		[Theory, AutoMoqData]
		public async Task CreateBitmapFromUrl_DirFound(
			Uri url,
			string filename,
			Mock<HttpMessageHandler> mockHttpMessageHandler,
			[Frozen] Mock<IFileSystem> mockFileSystem,
			[Frozen] Mock<IHttpClientFactory> mockHttpClientFactory,
			BitmapService sut)
		{
			mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient(mockHttpMessageHandler.Object));

			mockFileSystem.Setup(x => x.Directory.Exists("./img/download/")).Returns(true);

			var actual = await sut.CreateBitmapFromUrl(url.ToString(), filename);

			actual.Should().NotBeNull();

			mockFileSystem.Verify(x => x.Directory.CreateDirectory("./img/download/"), Times.Never());
			mockFileSystem.Verify(x => x.File.WriteAllBytesAsync($"./img/download/{filename}", It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));
		}
		#endregion

		#region ApplyGraphicToTemplate tests

		[Theory, AutoMoqData]
		public void ApplyGraphicToTemplate_MaintainAspectRatio_NullColor(
			BitmapService sut)
		{
			var template = SetupTemplate(100, Brushes.White);
			var graphic = SetupGraphic(50, 50, Brushes.Red);
			var rectangle = new Rectangle(25, 25, 50, 50);

			var actual = sut.ApplyGraphicToTemplate(template, graphic, rectangle);

			actual.Should().NotBeNull();

			VerifyResult(actual, Color.White, Color.Red);
		}

		[Theory, AutoMoqData]
		public void ApplyGraphicToTemplate_UndersizedGraphic_LongerThanWide_MaintainAspectRatioTrue_NullColor(
			BitmapService sut)
		{
			var template = SetupTemplate(100, Brushes.White);
			var graphic = SetupGraphic(10, 40, Brushes.Red);
			var rectangle = new Rectangle(25, 25, 50, 50);

			var actual = sut.ApplyGraphicToTemplate(template, graphic, rectangle);

			actual.Should().NotBeNull();

			for (int i = 0; i < actual.Width; i++)
			{
				for (int j = 0; j < actual.Height; j++)
				{
					var pixel = actual.GetPixel(i, j);

					if (i >= 44 && i < 56 && j >= 25 && j < 75)
						// Because the image will stretch, it's hard to know exactly what color it will be.  So we can only assert that it isn't white.
						pixel.ToArgb().Should().NotBe(Color.White.ToArgb());
					else
						pixel.ToArgb().Should().Be(Color.White.ToArgb(), $"i = {i}, j = {j}");
				}
			}
		}

		[Theory, AutoMoqData]
		public void ApplyGraphicToTemplate_UndersizedGraphic_LongerThanWide_MaintainAspectRatioFalse_NullColor(
			BitmapService sut)
		{
			var template = SetupTemplate(100, Brushes.White);
			var graphic = SetupGraphic(10, 40, Brushes.Red);
			var rectangle = new Rectangle(25, 25, 50, 50);

			var actual = sut.ApplyGraphicToTemplate(template, graphic, rectangle, false);

			actual.Should().NotBeNull();

			for (int i = 0; i < actual.Width; i++)
			{
				for (int j = 0; j < actual.Height; j++)
				{
					var pixel = actual.GetPixel(i, j);

					if (i >= 25 && i < 75 && j >= 25 && j < 75)
						// Because the image will stretch, it's hard to know exactly what color it will be.  So we can only assert that it isn't white.
						pixel.ToArgb().Should().NotBe(Color.White.ToArgb());
					else
						pixel.ToArgb().Should().Be(Color.White.ToArgb());
				}
			}
		}

		[Theory, AutoMoqData]
		public void ApplyGraphicToTemplate_UndersizedGraphic_WiderThanLong_MaintainAspectRatioTrue_NullColor(
			BitmapService sut)
		{
			var template = SetupTemplate(100, Brushes.White);
			var graphic = SetupGraphic(40, 10, Brushes.Red);
			var rectangle = new Rectangle(25, 25, 50, 50);

			var actual = sut.ApplyGraphicToTemplate(template, graphic, rectangle);

			actual.Should().NotBeNull();

			for (int i = 0; i < actual.Width; i++)
			{
				for (int j = 0; j < actual.Height; j++)
				{
					var pixel = actual.GetPixel(i, j);

					if (i >= 25 && i < 75 && j >= 44 && j < 56)
						// Because the image will stretch, it's hard to know exactly what color it will be.  So we can only assert that it isn't white.
						pixel.ToArgb().Should().NotBe(Color.White.ToArgb());
					else
						pixel.ToArgb().Should().Be(Color.White.ToArgb(), $"i = {i}, j = {j}");
				}
			}
		}

		[Theory, AutoMoqData]
		public void ApplyGraphicToTemplate_UndersizedGraphic_WiderThanLong_MaintainAspectRatioFalse_NullColor(
			BitmapService sut)
		{
			var template = SetupTemplate(100, Brushes.White);
			var graphic = SetupGraphic(40, 10, Brushes.Red);
			var rectangle = new Rectangle(25, 25, 50, 50);

			var actual = sut.ApplyGraphicToTemplate(template, graphic, rectangle, false);

			actual.Should().NotBeNull();

			for (int i = 0; i < actual.Width; i++)
			{
				for (int j = 0; j < actual.Height; j++)
				{
					var pixel = actual.GetPixel(i, j);

					if (i >= 25 && i < 75 && j >= 25 && j < 75)
						// Because the image will stretch, it's hard to know exactly what color it will be.  So we can only assert that it isn't white.
						pixel.ToArgb().Should().NotBe(Color.White.ToArgb());
					else
						pixel.ToArgb().Should().Be(Color.White.ToArgb());
				}
			}
		}

		[Theory, AutoMoqData]
		public void ApplyGraphicToTemplate_MaintainAspectRatio_ValidColor(
			BitmapService sut)
		{
			var template = SetupTemplate(100, Brushes.White);
			var graphic = SetupGraphic(50, 50, Brushes.Red);
			var rectangle = new Rectangle(25, 25, 50, 50);

			var actual = sut.ApplyGraphicToTemplate(template, graphic, rectangle, false, "blue");

			actual.Should().NotBeNull();

			VerifyResult(actual, Color.Blue, Color.Red);
		}

		private static Bitmap SetupTemplate(int size, Brush brush)
		{
			var template = new Bitmap(size, size);
			using Graphics graph = Graphics.FromImage(template);
			graph.FillRectangle(brush, new Rectangle(0, 0, size, size));

			return template;
		}

		private static Bitmap SetupGraphic(int width, int height, Brush brush)
		{
			var graphic = new Bitmap(width, height);
			using Graphics graph = Graphics.FromImage(graphic);
			graph.FillRectangle(brush, new Rectangle(0, 0,  width, height));

			return graphic;
		}

		private static void VerifyResult(Bitmap result, Color templateColor, Color graphicColor)
		{
			for (int i = 0; i < result.Width; i++)
			{
				for (int j = 0; j < result.Height; j++)
				{
					var pixel = result.GetPixel(i, j);

					if (i >= 25 && i < 75 && j >= 25 && j < 75)
						pixel.ToArgb().Should().Be(graphicColor.ToArgb());
					else
						pixel.ToArgb().Should().Be(templateColor.ToArgb());
				}
			}
		}
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
