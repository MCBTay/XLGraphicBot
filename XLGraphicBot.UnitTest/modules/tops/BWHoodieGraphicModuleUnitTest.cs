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
	public class BWHoodieGraphicModuleUnitTest : BaseTopGraphicModuleUnitTest
	{
		[Theory, AutoMoqData]
		public async Task BWHoodieAsync_HappyPath(
			[Frozen] Mock<IBitmapService> mockBitmapService,
			[Frozen] Mock<IFileSystem> mockFileSystem,
			[Frozen] Mock<IDiscordService> mockDiscordService,
			BWHoodieGraphicModule sut)
		{
			SetupFilesExist(mockFileSystem);
			SetupMockDiscordService(mockDiscordService, new Bitmap(100, 100), "image.png");

			await sut.BWHoodieAsync();

			VerifyGraphicApplied(mockBitmapService, new Rectangle(265, 225, 325, 325));
			VerifyFileSent(mockDiscordService);
			VerifyFilesDeleted(mockFileSystem, "image.png", "bwhoodie_image.png.png");
		}
	}
}