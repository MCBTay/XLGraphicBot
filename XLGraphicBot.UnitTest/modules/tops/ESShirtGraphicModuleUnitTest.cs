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
	public class ESShirtGraphicModuleUnitTest
	{
		[Theory, AutoMoqData]
		public async Task AlphaMShirtAsync_HappyPath(
			[Frozen] Mock<IBitmapService> mockBitmapService,
			[Frozen] Mock<IFileSystem> mockFileSystem,
			[Frozen] Mock<IDiscordService> mockDiscordService,
			ESShirtGraphicModule sut)
		{
			mockFileSystem.SetupFilesExist();
			mockDiscordService.SetupMockDiscordService(new Bitmap(100, 100), "image.png");

			await sut.ESShirtAsync();

			mockBitmapService.VerifyGraphicApplied(new Rectangle(460, 260, 320, 320));
			mockDiscordService.VerifyFileSent();
			mockFileSystem.VerifyFilesDeleted("image.png", "esshirt_image.png.png");
		}
	}
}