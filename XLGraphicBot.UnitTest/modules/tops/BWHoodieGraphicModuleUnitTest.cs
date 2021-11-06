using AutoFixture.Xunit2;
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
	public class BWHoodieGraphicModuleUnitTest
	{
		[Theory, AutoMoqData]
		public async Task BWHoodieAsync_HappyPath(
			[Frozen] Mock<IBitmapService> mockBitmapService,
			[Frozen] Mock<IFileSystem> mockFileSystem,
			[Frozen] Mock<IDiscordService> mockDiscordService,
			BWHoodieGraphicModule sut)
		{
			mockFileSystem.SetupFilesExist();
			mockDiscordService.SetupMockDiscordService(new Bitmap(100, 100), "image.png");

			await sut.BWHoodieAsync();

			mockBitmapService.VerifyGraphicApplied(new Rectangle(265, 225, 325, 325));
			mockDiscordService.VerifyFileSent();
			mockFileSystem.VerifyFilesDeleted("image.png", "bwhoodie_image.png.png");
		}
	}
}