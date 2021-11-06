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
	public class TAShirtGraphicModuleUnitTest : BaseTopGraphicModuleUnitTest
	{
		[Theory, AutoMoqData]
		public async Task TAShirtAsync_HappyPath(
			[Frozen] Mock<IBitmapService> mockBitmapService,
			[Frozen] Mock<IFileSystem> mockFileSystem,
			[Frozen] Mock<IDiscordService> mockDiscordService,
			TAShirtGraphicModule sut)
		{
			SetupFilesExist(mockFileSystem);
			SetupMockDiscordService(mockDiscordService, new Bitmap(100, 100), "image.png");

			await sut.TAShirtAsync();

			VerifyGraphicApplied(mockBitmapService, new Rectangle(400, 240, 385, 385));
			VerifyFileSent(mockDiscordService);
			VerifyFilesDeleted(mockFileSystem, "image.png", "tashirt_image.png.png");
		}
	}
}