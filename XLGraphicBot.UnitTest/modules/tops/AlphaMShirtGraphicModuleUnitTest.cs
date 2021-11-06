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
	public class AlphaMShirtGraphicModuleUnitTest
	{
		[Theory, AutoMoqData]
		public async Task AlphaMShirtAsync_HappyPath(
			[Frozen] Mock<IBitmapService> mockBitmapService,
			[Frozen] Mock<IFileSystem> mockFileSystem,
			[Frozen] Mock<IDiscordService> mockDiscordService,
			AlphaMShirtGraphicModule sut)
		{
			mockFileSystem.SetupFilesExist();
			mockDiscordService.SetupMockDiscordService(new Bitmap(100, 100), "image.png");

			await sut.AlphaMShirtAsync();

			mockBitmapService.VerifyGraphicApplied(new Rectangle(265, 335, 310, 310));
			mockDiscordService.VerifyFileSent();
			mockFileSystem.VerifyFilesDeleted("image.png", "alphamshirt_image.png.png");
		}
	}
}
