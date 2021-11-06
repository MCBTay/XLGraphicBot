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
	public class MShirtGraphicModuleUnitTest : BaseTopGraphicModuleUnitTest
	{
		[Theory, AutoMoqData]
		public async Task MShirtAsync_HappyPath(
			[Frozen] Mock<IBitmapService> mockBitmapService,
			[Frozen] Mock<IFileSystem> mockFileSystem,
			[Frozen] Mock<IDiscordService> mockDiscordService,
			MShirtGraphicModule sut)
		{
			SetupFilesExist(mockFileSystem);
			SetupMockDiscordService(mockDiscordService, new Bitmap(100, 100), "image.png");

			await sut.MShirtAsync();

			VerifyGraphicApplied(mockBitmapService, new Rectangle(420, 250, 330, 330));
			VerifyFileSent(mockDiscordService);
			VerifyFilesDeleted(mockFileSystem, "image.png", "mshirt_image.png.png");
		}
	}
}
