using AutoFixture.Xunit2;
using Discord.Commands;
using Moq;
using System.Drawing;
using System.IO.Abstractions;
using System.Threading.Tasks;
using XLGraphicBot.modules.tops;
using XLGraphicBot.services;
using XLGraphicBot.UnitTest.Core;
using Xunit;

namespace XLGraphicBot.UnitTest.modules.tops
{
	public class BaseTopGraphicModuleUnitTest
	{
		[Theory, AutoMoqData]
		public async Task GenerateGraphicAsync_NullImage(
			[Frozen] Mock<IBitmapService> mockBitmapService,
			[Frozen] Mock<IDiscordService> mockDiscordService,
			BaseTopGraphicModule sut)
		{
			mockDiscordService
				.Setup(x => x.GetMostRecentImage(It.IsAny<ICommandContext>()))
				.ReturnsAsync(((Bitmap)null, "image.jpg"));

			await sut.GenerateGraphicAsync("mshirt", new Rectangle(0, 0, 100, 100), null);

			mockBitmapService.Verify(x => x.ApplyGraphicToTemplate(It.IsAny<Bitmap>(), It.IsAny<Bitmap>(), It.IsAny<Rectangle>(), It.IsAny<bool>(), It.IsAny<string>()), Times.Never());
			mockDiscordService.Verify(x => x.SendFileAsync(It.IsAny<ICommandContext>(), It.IsAny<Bitmap>(), It.IsAny<string>()), Times.Never());
		}

		[Theory, AutoMoqData]
		public async Task GenerateGraphicAsync_EmptyFilename(
			[Frozen] Mock<IBitmapService> mockBitmapService,
			[Frozen] Mock<IDiscordService> mockDiscordService,
			BaseTopGraphicModule sut)
		{
			mockDiscordService
				.Setup(x => x.GetMostRecentImage(It.IsAny<ICommandContext>()))
				.ReturnsAsync((new Bitmap(100, 100), string.Empty));

			await sut.GenerateGraphicAsync("mshirt", new Rectangle(0, 0, 100, 100), null);

			mockBitmapService.Verify(x => x.ApplyGraphicToTemplate(It.IsAny<Bitmap>(), It.IsAny<Bitmap>(), It.IsAny<Rectangle>(), It.IsAny<bool>(), It.IsAny<string>()), Times.Never());
			mockDiscordService.Verify(x => x.SendFileAsync(It.IsAny<ICommandContext>(), It.IsAny<Bitmap>(), It.IsAny<string>()), Times.Never());
		}

		[Theory, AutoMoqData]
		public async Task GenerateGraphicAsync_ValidBitmap_ValidFilename(
			[Frozen] Mock<IBitmapService> mockBitmapService,
			[Frozen] Mock<IFileSystem> mockFileSystem,
			[Frozen] Mock<IDiscordService> mockDiscordService,
			BaseTopGraphicModule sut)
		{
			Mock.Get(mockFileSystem.Object.File)
				.Setup(x => x.Exists(It.IsAny<string>()))
				.Returns(true);

			mockDiscordService
				.Setup(x => x.GetMostRecentImage(It.IsAny<ICommandContext>()))
				.ReturnsAsync((new Bitmap(100, 100), "image.png"));

			await sut.GenerateGraphicAsync("mshirt", new Rectangle(0, 0, 100, 100), null);

			mockBitmapService.Verify(x => x.ApplyGraphicToTemplate(It.IsAny<Bitmap>(), It.IsAny<Bitmap>(), new Rectangle(0, 0, 100, 100), It.IsAny<bool>(), It.IsAny<string>()), Times.Once());

			mockDiscordService.Verify(x => x.SendFileAsync(It.IsAny<ICommandContext>(), It.IsAny<Bitmap>(), It.IsAny<string>()), Times.Once());

			mockFileSystem.Verify(x => x.File.Delete("./img/download/image.png"), Times.Once());
			mockFileSystem.Verify(x => x.File.Delete("./img/generated/mshirt_image.png.png"), Times.Once());
		}
	}
}
