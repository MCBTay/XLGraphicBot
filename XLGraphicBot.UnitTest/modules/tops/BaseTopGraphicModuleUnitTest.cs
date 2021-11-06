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
			mockDiscordService.SetupMockDiscordService((Bitmap) null, "image.png");

			await sut.GenerateGraphicAsync("mshirt", new Rectangle(0, 0, 100, 100), null);

			mockBitmapService.VerifyGraphicApplied(new Rectangle(0, 0, 100, 100), Times.Never());
			mockDiscordService.VerifyFileSent(Times.Never());
		}

		[Theory, AutoMoqData]
		public async Task GenerateGraphicAsync_EmptyFilename(
			[Frozen] Mock<IBitmapService> mockBitmapService,
			[Frozen] Mock<IDiscordService> mockDiscordService,
			BaseTopGraphicModule sut)
		{
			mockDiscordService.SetupMockDiscordService(new Bitmap(100, 100), string.Empty);

			await sut.GenerateGraphicAsync("mshirt", new Rectangle(0, 0, 100, 100), null);

			mockBitmapService.VerifyGraphicApplied(new Rectangle(0, 0, 100, 100), Times.Never());
			mockDiscordService.VerifyFileSent(Times.Never());
		}

		[Theory, AutoMoqData]
		public async Task GenerateGraphicAsync_ValidBitmap_ValidFilename(
			[Frozen] Mock<IBitmapService> mockBitmapService,
			[Frozen] Mock<IFileSystem> mockFileSystem,
			[Frozen] Mock<IDiscordService> mockDiscordService,
			BaseTopGraphicModule sut)
		{
			mockFileSystem.SetupFilesExist();
			mockDiscordService.SetupMockDiscordService(new Bitmap(100, 100), "image.png");

			await sut.GenerateGraphicAsync("mshirt", new Rectangle(0, 0, 100, 100), null);

			mockBitmapService.VerifyGraphicApplied(new Rectangle(0, 0, 100, 100));
			mockDiscordService.VerifyFileSent();
			mockFileSystem.VerifyFilesDeleted("image.png", "mshirt_image.png.png");
		}
	}
}
