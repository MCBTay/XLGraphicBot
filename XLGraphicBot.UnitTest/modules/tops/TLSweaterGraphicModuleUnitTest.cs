using AutoFixture.Xunit2;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO.Abstractions;
using System.Threading.Tasks;
using XLGraphicBot.modules.tops;
using XLGraphicBot.services;
using XLGraphicBot.UnitTest.Core;
using Xunit;

namespace XLGraphicBot.UnitTest.modules.tops
{
	[ExcludeFromCodeCoverage]
	public class TLSweaterGraphicModuleUnitTest
	{
		[Theory, AutoMoqData]
		public async Task TLSweaterAsync_HappyPath(
			[Frozen] Mock<IBitmapService> mockBitmapService,
			[Frozen] Mock<IFileSystem> mockFileSystem,
			[Frozen] Mock<IDiscordService> mockDiscordService,
			TLSweaterGraphicModule sut)
		{
			mockFileSystem.SetupFilesExist();
			mockDiscordService.SetupMockDiscordService(new Bitmap(100, 100), "image.png");

			await sut.TLSweaterAsync();

			mockBitmapService.VerifyGraphicApplied(new Rectangle(1340, 305, 400, 400));
			mockDiscordService.VerifyFileSent();
			mockFileSystem.VerifyFilesDeleted("image.png", "tlsweater_image.png.png");
		}
	}
}