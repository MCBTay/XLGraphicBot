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
	public class AlphaMShirtGraphicModuleUnitTest : BaseTopGraphicModuleUnitTest
	{
		[Theory, AutoMoqData]
		public async Task AlphaMShirtAsync_HappyPath(
			[Frozen] Mock<IBitmapService> mockBitmapService,
			[Frozen] Mock<IFileSystem> mockFileSystem,
			[Frozen] Mock<IDiscordService> mockDiscordService,
			AlphaMShirtGraphicModule sut)
		{
			SetupFilesExist(mockFileSystem);
			SetupMockDiscordService(mockDiscordService, new Bitmap(100, 100), "image.png");

			await sut.AlphaMShirtAsync();

			VerifyGraphicApplied(mockBitmapService, new Rectangle(265, 335, 310, 310));
			VerifyFileSent(mockDiscordService);
			VerifyFilesDeleted(mockFileSystem, "image.png", "alphamshirt_image.png.png");
		}
	}
}
