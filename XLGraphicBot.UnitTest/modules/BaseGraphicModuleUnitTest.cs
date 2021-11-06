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
	}
}
