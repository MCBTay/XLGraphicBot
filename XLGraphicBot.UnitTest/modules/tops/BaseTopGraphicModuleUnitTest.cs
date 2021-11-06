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
			SetupMockDiscordService(mockDiscordService, (Bitmap)null, "image.png");

			await sut.GenerateGraphicAsync("mshirt", new Rectangle(0, 0, 100, 100), null);

			VerifyGraphicApplied(mockBitmapService, new Rectangle(0, 0, 100, 100), Times.Never());
			VerifyFileSent(mockDiscordService, Times.Never());
		}

		[Theory, AutoMoqData]
		public async Task GenerateGraphicAsync_EmptyFilename(
			[Frozen] Mock<IBitmapService> mockBitmapService,
			[Frozen] Mock<IDiscordService> mockDiscordService,
			BaseTopGraphicModule sut)
		{
			SetupMockDiscordService(mockDiscordService, new Bitmap(100, 100), string.Empty);

			await sut.GenerateGraphicAsync("mshirt", new Rectangle(0, 0, 100, 100), null);

			VerifyGraphicApplied(mockBitmapService, new Rectangle(0, 0, 100, 100), Times.Never());
			VerifyFileSent(mockDiscordService, Times.Never());
		}

		[Theory, AutoMoqData]
		public async Task GenerateGraphicAsync_ValidBitmap_ValidFilename(
			[Frozen] Mock<IBitmapService> mockBitmapService,
			[Frozen] Mock<IFileSystem> mockFileSystem,
			[Frozen] Mock<IDiscordService> mockDiscordService,
			BaseTopGraphicModule sut)
		{
			SetupFilesExist(mockFileSystem);
			SetupMockDiscordService(mockDiscordService, new Bitmap(100, 100), "image.png");

			await sut.GenerateGraphicAsync("mshirt", new Rectangle(0, 0, 100, 100), null);

			VerifyGraphicApplied(mockBitmapService, new Rectangle(0, 0, 100, 100));
			VerifyFileSent(mockDiscordService);
			VerifyFilesDeleted(mockFileSystem, "image.png", "mshirt_image.png.png");
		}

		protected void SetupMockDiscordService(Mock<IDiscordService> mockDiscordService, Bitmap bitmap, string filename)
		{
			mockDiscordService
				.Setup(x => x.GetMostRecentImage(It.IsAny<ICommandContext>()))
				.ReturnsAsync((bitmap, filename));
		}

		protected void SetupFilesExist(Mock<IFileSystem> mockFileSystem, bool exist = true)
		{
			Mock.Get(mockFileSystem.Object.File)
				.Setup(x => x.Exists(It.IsAny<string>()))
				.Returns(exist);
		}

		protected void VerifyGraphicApplied(Mock<IBitmapService> mockBitmapService, Rectangle rectangle, Times? times = null)
		{
			times ??= Times.Once();
			mockBitmapService.Verify(x => x.ApplyGraphicToTemplate(It.IsAny<Bitmap>(), It.IsAny<Bitmap>(), rectangle, It.IsAny<bool>(), It.IsAny<string>()), times.Value);
		}

		protected void VerifyFileSent(Mock<IDiscordService> mockDiscordService, Times? times = null)
		{
			times ??= Times.Once();
			mockDiscordService.Verify(x => x.SendFileAsync(It.IsAny<ICommandContext>(), It.IsAny<Bitmap>(), It.IsAny<string>()), times.Value);
		}

		protected void VerifyFilesDeleted(Mock<IFileSystem> mockFileSystem, string filename, string generatedFilename)
		{
			mockFileSystem.Verify(x => x.File.Delete($"./img/download/{filename}"), Times.Once());
			mockFileSystem.Verify(x => x.File.Delete($"./img/generated/{generatedFilename}"), Times.Once());
		}
	}
}
