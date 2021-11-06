using Discord;
using Discord.Commands;
using Moq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO.Abstractions;
using System.Linq;
using XLGraphicBot.services;

namespace XLGraphicBot.UnitTest.Core
{
	[ExcludeFromCodeCoverage]
	public static class GraphicModuleExtensions
	{
		public static void SetupCommandContext(
			this Mock<ICommandContext> mockCommandContext,
			ISelfUser selfUser,
			List<List<IMessage>> messages = null)
		{
			mockCommandContext.SetupMessageChannel(messages);
			mockCommandContext.SetupDiscordClient(selfUser);
		}

		private static void SetupMessageChannel(
			this Mock<ICommandContext> mockCommandContext,
			List<List<IMessage>> messages = null)
		{
			var mockMessageChannel = new Mock<IMessageChannel>();

			mockMessageChannel
					  .Setup(x => x.GetMessagesAsync(
						  It.IsAny<int>(),
						  It.IsAny<CacheMode>(),
						  It.IsAny<RequestOptions>()))
					  .Returns((messages ?? new List<List<IMessage>>()).ToAsyncEnumerable());

			mockCommandContext
					  .SetupGet(x => x.Channel)
					  .Returns(mockMessageChannel.Object);
		}

		private static void SetupDiscordClient(
			this Mock<ICommandContext> mockCommandContext,
			ISelfUser selfUser)
		{
			var mockDiscordClient = new Mock<IDiscordClient>();

			mockDiscordClient
					  .SetupGet(x => x.CurrentUser)
					  .Returns(selfUser);

			mockCommandContext
			  .SetupGet(x => x.Client)
			  .Returns(mockDiscordClient.Object);
		}

		public static void SetupAttachments(
			this List<IAttachment> attachments,
			int width = 100,
			int height = 100,
			string url = "http://www.google.com",
			string filename = "image.png")
		{
			foreach (var attachment in attachments)
			{
				var mockAttachment = Mock.Get(attachment);

				mockAttachment.SetupGet(x => x.Width).Returns(width);
				mockAttachment.SetupGet(x => x.Height).Returns(height);
				mockAttachment.SetupGet(x => x.Url).Returns(url);
				mockAttachment.SetupGet(x => x.Filename).Returns(filename);
			}
		}

		public static void SetupMessages(
			this List<List<IMessage>> messages,
			ulong id,
			List<IAttachment> attachments = null)
		{
			foreach (var messageList in messages)
			{
				foreach (var message in messageList)
				{
					Mock.Get(message.Author)
						.SetupGet(x => x.Id)
						.Returns(id);

					Mock.Get(message)
						.SetupGet(x => x.Attachments)
						.Returns(attachments ?? new List<IAttachment>());
				}
			}
		}

		public static void SetupMockDiscordService(this Mock<IDiscordService> mockDiscordService, Bitmap bitmap, string filename)
		{
			mockDiscordService
				.Setup(x => x.GetMostRecentImage(It.IsAny<ICommandContext>()))
				.ReturnsAsync((bitmap, filename));
		}

		public static void SetupFilesExist(this Mock<IFileSystem> mockFileSystem, bool exist = true)
		{
			Mock.Get(mockFileSystem.Object.File)
				.Setup(x => x.Exists(It.IsAny<string>()))
				.Returns(exist);
		}

		public static void VerifyGraphicApplied(this Mock<IBitmapService> mockBitmapService, Rectangle rectangle, Times? times = null)
		{
			times ??= Times.Once();
			mockBitmapService.Verify(x => x.ApplyGraphicToTemplate(It.IsAny<Bitmap>(), It.IsAny<Bitmap>(), rectangle, It.IsAny<bool>(), It.IsAny<string>()), times.Value);
		}

		public static void VerifyFileSent(this Mock<IDiscordService> mockDiscordService, Times? times = null)
		{
			times ??= Times.Once();
			mockDiscordService.Verify(x => x.SendFileAsync(It.IsAny<ICommandContext>(), It.IsAny<Bitmap>(), It.IsAny<string>()), times.Value);
		}

		public static void VerifyFilesDeleted(this Mock<IFileSystem> mockFileSystem, string filename, string generatedFilename)
		{
			mockFileSystem.Verify(x => x.File.Delete($"./img/download/{filename}"), Times.Once());
			mockFileSystem.Verify(x => x.File.Delete($"./img/generated/{generatedFilename}"), Times.Once());
		}
	}
}