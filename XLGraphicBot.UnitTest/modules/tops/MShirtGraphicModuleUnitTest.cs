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
	public class MShirtGraphicModuleUnitTest
	{
		[Theory, AutoMoqData]
		public async Task MShirtAsync_HappyPath(
			[Frozen] Mock<IBitmapService> mockBitmapService,
			[Frozen] Mock<IFileSystem> mockFileSystem,
			[Frozen] Mock<IDiscordService> mockDiscordService,
			MShirtGraphicModule sut)
		{
			mockFileSystem.SetupFilesExist();
			mockDiscordService.SetupMockDiscordService(new Bitmap(100, 100), "image.png");

			await sut.MShirtAsync();

			mockBitmapService.VerifyGraphicApplied(new Rectangle(420, 250, 330, 330));
			mockDiscordService.VerifyFileSent();
			mockFileSystem.VerifyFilesDeleted("image.png", "mshirt_image.png.png");
		}
	}
}
