using AutoFixture.Xunit2;
using Discord.Commands;
using Moq;
using System.Drawing;
using System.IO.Abstractions;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Frameworks;
using XLGraphicBot.modules;
using XLGraphicBot.services;
using XLGraphicBot.UnitTest.Core;
using Xunit;

namespace XLGraphicBot.UnitTest.modules
{
	public class DeckGraphicModuleUnitTest
	{
		[Theory, AutoMoqData]
		public async Task DeckAsync_NullImage(
			[Frozen] Mock<IBitmapService> mockBitmapService,
			[Frozen] Mock<IDiscordService> mockDiscordService,
			DeckGraphicModule sut)
		{
			mockDiscordService
				.Setup(x => x.GetMostRecentImage(It.IsAny<ICommandContext>()))
				.ReturnsAsync(((Bitmap)null, "image.jpg"));

			await sut.DeckAsync(null);

			mockBitmapService.Verify(x => x.ApplyGraphicToTemplate(It.IsAny<Bitmap>(), It.IsAny<Bitmap>(), It.IsAny<Rectangle>(), It.IsAny<bool>()), Times.Never());
			mockDiscordService.Verify(x => x.SendFileAsync(It.IsAny<ICommandContext>(), It.IsAny<Bitmap>(), It.IsAny<string>()), Times.Never());
		}

		[Theory, AutoMoqData]
		public async Task DeckAsync_EmptyFilename(
			[Frozen] Mock<IBitmapService> mockBitmapService,
			[Frozen] Mock<IDiscordService> mockDiscordService,
			DeckGraphicModule sut)
		{
			mockDiscordService
				.Setup(x => x.GetMostRecentImage(It.IsAny<ICommandContext>()))
				.ReturnsAsync((new Bitmap(100, 100), string.Empty));

			await sut.DeckAsync(null);

			mockBitmapService.Verify(x => x.ApplyGraphicToTemplate(It.IsAny<Bitmap>(), It.IsAny<Bitmap>(), It.IsAny<Rectangle>(), It.IsAny<bool>()), Times.Never());
			mockDiscordService.Verify(x => x.SendFileAsync(It.IsAny<ICommandContext>(), It.IsAny<Bitmap>(), It.IsAny<string>()), Times.Never());
		}

		[Theory]
		[InlineAutoMoqData(false)]
		[InlineAutoMoqData(true)]
		public async Task DeckAsync_ValidBitmap_ValidFilename(
			bool includeWear,
			[Frozen] Mock<IBitmapService> mockBitmapService,
			[Frozen] Mock<IFileSystem> mockFileSystem,
			[Frozen] Mock<IDiscordService> mockDiscordService,
			DeckGraphicModule sut)
		{
			Mock.Get(mockFileSystem.Object.File)
				.Setup(x => x.Exists(It.IsAny<string>()))
				.Returns(true);

			mockDiscordService
				.Setup(x => x.GetMostRecentImage(It.IsAny<ICommandContext>()))
				.ReturnsAsync((new Bitmap(100, 100), "image.png"));

			await sut.DeckAsync(new DeckGraphicModuleArguments { IncludeWear = includeWear });

			mockBitmapService.Verify(x => x.ApplyGraphicToTemplate(It.IsAny<Bitmap>(), It.IsAny<Bitmap>(), new Rectangle(0, 694, 2048, 482), It.IsAny<bool>()), Times.Once());

			if (includeWear)
			{
				mockBitmapService.Verify(x => x.ApplyGraphicToTemplate(It.IsAny<Bitmap>(), It.IsAny<Bitmap>(), new Rectangle(0, 0, 2048, 2048), It.IsAny<bool>()), Times.Once());
			}

			mockDiscordService.Verify(x => x.SendFileAsync(It.IsAny<ICommandContext>(), It.IsAny<Bitmap>(), It.IsAny<string>()), Times.Once());

			mockFileSystem.Verify(x => x.File.Delete("./img/download/image.png"), Times.Once());
			mockFileSystem.Verify(x => x.File.Delete("./img/generated/Deck_image.png.png"), Times.Once());
		}
	}
}
