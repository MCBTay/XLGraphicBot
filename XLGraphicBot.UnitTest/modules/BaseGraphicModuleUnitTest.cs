using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO.Abstractions;
using XLGraphicBot.modules;
using XLGraphicBot.UnitTest.Core;
using Xunit;

namespace XLGraphicBot.UnitTest.modules
{
	[ExcludeFromCodeCoverage]
	public class BaseGraphicModuleUnitTest
	{
		#region DeleteFile tests
		[Theory]
		[InlineAutoMoqData(null)]
		[InlineAutoMoqData("")]
		public void DeleteFile_InvalidFilePath(
			string filepath,
			[Frozen] Mock<IFileSystem> mockFileSystem,
			BaseGraphicModule sut)
		{
			sut.DeleteFile(filepath);

			mockFileSystem.Verify(x => x.File.Delete(filepath), Times.Never());
		}

		[Theory, AutoMoqData]
		public void DeleteFile_FileDoesNotExist(
			string filepath,
			[Frozen] Mock<IFileSystem> mockFileSystem,
			BaseGraphicModule sut)
		{
			mockFileSystem.Setup(x => x.File.Exists(filepath)).Returns(false);

			sut.DeleteFile(filepath);

			mockFileSystem.Verify(x => x.File.Delete(filepath), Times.Never());
		}

		[Theory, AutoMoqData]
		public void DeleteFile_FileExists(
			string filepath,
			[Frozen] Mock<IFileSystem> mockFileSystem,
			BaseGraphicModule sut)
		{
			mockFileSystem.Setup(x => x.File.Exists(filepath)).Returns(true);

			sut.DeleteFile(filepath);

			mockFileSystem.Verify(x => x.File.Delete(filepath));
		}
		#endregion

		#region ScaleImage tests
		[Theory, AutoMoqData]
		public void ScaleImage_WidthAndHeightMatch(
			BaseGraphicModule sut)
		{
			var rectangle = new Rectangle(0, 0, 500, 500);

			var actual = sut.ScaleImage(100, 100, rectangle);

			actual.Should().Be(rectangle);
		}

		[Theory, AutoMoqData]
		public void ScaleImage_WidthLarger(
			BaseGraphicModule sut)
		{
			var rectangle = new Rectangle(0, 0, 500, 500);

			var actual = sut.ScaleImage(200, 100, rectangle);

			actual.Should().Be(new Rectangle(0, 125, 500, 250));
		}

		[Theory, AutoMoqData]
		public void ScaleImage_HeightLarger(
			BaseGraphicModule sut)
		{
			var rectangle = new Rectangle(0, 0, 500, 500);

			var actual = sut.ScaleImage(100, 200, rectangle);

			actual.Should().Be(new Rectangle(125, 0, 250, 500));
		}
		#endregion
	}
}
