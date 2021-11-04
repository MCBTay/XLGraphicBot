﻿using AutoFixture.Xunit2;
using Discord;
using Discord.Commands;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Abstractions;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using XLGraphicBot.modules;
using XLGraphicBot.UnitTest.Core;
using Xunit;

namespace XLGraphicBot.UnitTest.modules
{
	public class BaseGraphicModuleUnitTest
	{
		#region GetMostRecentImage tests
		[Theory, AutoMoqData]
		public async Task GetMostRecentImage_NoMessages(
			Mock<IMessageChannel> mockMessageChannel,
			[Frozen] Mock<ICommandContext> mockCommandContext,
			BaseGraphicModule sut)
		{
			mockMessageChannel
				.Setup(x => x.GetMessagesAsync(
					It.IsAny<int>(),
					It.IsAny<CacheMode>(),
					It.IsAny<RequestOptions>()))
				.Returns(new List<List<IMessage>>().ToAsyncEnumerable());

			mockCommandContext
				.SetupGet(x => x.Channel)
				.Returns(mockMessageChannel.Object);

			(Bitmap image, string filename) = await sut.GetMostRecentImage(mockCommandContext.Object);

			image.Should().BeNull();
			filename.Should().BeEmpty();
		}

		[Theory, AutoMoqData]
		public async Task GetMostRecentImage_MessagesMatchCurrentUser(
			List<List<IMessage>> messages,
			Mock<IDiscordClient> mockDiscordClient,
			Mock<ISelfUser> mockSelfUser,
			ulong selfUserId,
			Mock<IMessageChannel> mockMessageChannel,
			[Frozen] Mock<ICommandContext> mockCommandContext,
			BaseGraphicModule sut)
		{
			mockSelfUser.SetupGet(x => x.Id).Returns(selfUserId);

			foreach (var messageList in messages)
			{
				foreach (var message in messageList)
				{
					Mock.Get(message.Author)
						.SetupGet(x => x.Id)
						.Returns(mockSelfUser.Object.Id);
				}
			}

			mockDiscordClient
				.SetupGet(x => x.CurrentUser)
				.Returns(mockSelfUser.Object);

			mockCommandContext
				.SetupGet(x => x.Client).Returns(mockDiscordClient.Object);

			mockMessageChannel
				.Setup(x => x.GetMessagesAsync(
					It.IsAny<int>(),
					It.IsAny<CacheMode>(),
					It.IsAny<RequestOptions>()))
				.Returns(messages.ToAsyncEnumerable());

			mockCommandContext
				.SetupGet(x => x.Channel)
				.Returns(mockMessageChannel.Object);

			(Bitmap image, string filename) = await sut.GetMostRecentImage(mockCommandContext.Object);

			image.Should().BeNull();
			filename.Should().BeEmpty();
		}

		[Theory, AutoMoqData]
		public async Task GetMostRecentImage_MessagesDoNotMatchCurrentUser_NoAttachments(
			List<List<IMessage>> messages,
			Mock<IDiscordClient> mockDiscordClient,
			Mock<ISelfUser> mockSelfUser,
			ulong selfUserId,
			ulong otherUserId,
			Mock<IMessageChannel> mockMessageChannel,
			[Frozen] Mock<ICommandContext> mockCommandContext,
			BaseGraphicModule sut)
		{
			mockSelfUser.SetupGet(x => x.Id).Returns(selfUserId);

			foreach (var messageList in messages)
			{
				foreach (var message in messageList)
				{
					Mock.Get(message.Author)
						.SetupGet(x => x.Id)
						.Returns(otherUserId);
				}
			}

			mockDiscordClient
				.SetupGet(x => x.CurrentUser)
				.Returns(mockSelfUser.Object);

			mockCommandContext
				.SetupGet(x => x.Client).Returns(mockDiscordClient.Object);

			mockMessageChannel
				.Setup(x => x.GetMessagesAsync(
					It.IsAny<int>(),
					It.IsAny<CacheMode>(),
					It.IsAny<RequestOptions>()))
				.Returns(messages.ToAsyncEnumerable());

			mockCommandContext
				.SetupGet(x => x.Channel)
				.Returns(mockMessageChannel.Object);

			(Bitmap image, string filename) = await sut.GetMostRecentImage(mockCommandContext.Object);

			image.Should().BeNull();
			filename.Should().BeEmpty();
		}

		[Theory, AutoMoqData]
		public async Task GetMostRecentImage_FoundAttachment_NullHeightAndWidth(
			List<List<IMessage>> messages,
			Mock<IDiscordClient> mockDiscordClient,
			Mock<ISelfUser> mockSelfUser,
			ulong selfUserId,
			ulong otherUserId,
			IReadOnlyList<IAttachment> attachments,
			Mock<IMessageChannel> mockMessageChannel,
			[Frozen] Mock<ICommandContext> mockCommandContext,
			BaseGraphicModule sut)
		{
			mockSelfUser.SetupGet(x => x.Id).Returns(selfUserId);

			foreach (var messageList in messages)
			{
				foreach (var message in messageList)
				{
					Mock.Get(message.Author)
						.SetupGet(x => x.Id)
						.Returns(otherUserId);

					Mock.Get(message)
						.SetupGet(x => x.Attachments)
						.Returns(attachments);
				}
			}

			mockDiscordClient
				.SetupGet(x => x.CurrentUser)
				.Returns(mockSelfUser.Object);

			mockCommandContext
				.SetupGet(x => x.Client).Returns(mockDiscordClient.Object);

			mockMessageChannel
				.Setup(x => x.GetMessagesAsync(
					It.IsAny<int>(),
					It.IsAny<CacheMode>(),
					It.IsAny<RequestOptions>()))
				.Returns(messages.ToAsyncEnumerable());

			mockCommandContext
				.SetupGet(x => x.Channel)
				.Returns(mockMessageChannel.Object);

			(Bitmap image, string filename) = await sut.GetMostRecentImage(mockCommandContext.Object);

			image.Should().BeNull();
			filename.Should().BeEmpty();
		}

		[Theory, AutoMoqData]
		public async Task GetMostRecentImage_FoundAttachment_WrittenSuccesfully(
			List<List<IMessage>> messages,
			Mock<IDiscordClient> mockDiscordClient,
			Mock<ISelfUser> mockSelfUser,
			ulong selfUserId,
			ulong otherUserId,
			IReadOnlyList<IAttachment> attachments,
			Mock<IMessageChannel> mockMessageChannel,
			[Frozen] Mock<HttpClient> mockHttpClient,
			[Frozen] Mock<IFileSystem> mockFileSystem,
			[Frozen] Mock<ICommandContext> mockCommandContext,
			BaseGraphicModule sut)
		{
			mockSelfUser.SetupGet(x => x.Id).Returns(selfUserId);

			foreach (var messageList in messages)
			{
				foreach (var message in messageList)
				{
					Mock.Get(message.Author)
						.SetupGet(x => x.Id)
						.Returns(otherUserId);

					Mock.Get(message)
						.SetupGet(x => x.Attachments)
						.Returns(attachments);
				}
			}

			foreach (var attachment in attachments)
			{
				Mock.Get(attachment).SetupGet(x => x.Height).Returns(100);
				Mock.Get(attachment).SetupGet(x => x.Width).Returns(100);
				Mock.Get(attachment).SetupGet(x => x.Url).Returns("www.google.com");
			}

			mockDiscordClient
				.SetupGet(x => x.CurrentUser)
				.Returns(mockSelfUser.Object);

			mockCommandContext
				.SetupGet(x => x.Client).Returns(mockDiscordClient.Object);

			mockMessageChannel
				.Setup(x => x.GetMessagesAsync(
					It.IsAny<int>(),
					It.IsAny<CacheMode>(),
					It.IsAny<RequestOptions>()))
				.Returns(messages.ToAsyncEnumerable());

			mockCommandContext
				.SetupGet(x => x.Channel)
				.Returns(mockMessageChannel.Object);

			var test = new HttpClient(new HttpMessageHandlerStub());

			//mockHttpClient
			//	.Setup(x => x.GetByteArrayAsync(It.IsAny<string>()))
			//	.ReturnsAsync(new byte[1]);

			(Bitmap image, string filename) = await sut.GetMostRecentImage(mockCommandContext.Object);

			image.Should().BeNull();
			filename.Should().BeEmpty();

			mockFileSystem
				.Verify(x => x.File.WriteAllBytesAsync(
					It.IsAny<string>(),
					It.IsAny<byte[]>(),
					It.IsAny<CancellationToken>()));
		}
		#endregion

		#region DeleteFile tests
		[Theory]
		[InlineAutoMoqData(null)]
		[InlineAutoMoqData("")]
		public async Task DeleteFile_InvalidFilePath(
			string filepath,
			[Frozen] Mock<IFileSystem> mockFileSystem,
			BaseGraphicModule sut)
		{
			sut.DeleteFile(filepath);

			mockFileSystem.Verify(x => x.File.Delete(filepath), Times.Never());
		}

		[Theory, AutoMoqData]
		public async Task DeleteFile_FileDoesNotExist(
			string filepath,
			[Frozen] Mock<IFileSystem> mockFileSystem,
			BaseGraphicModule sut)
		{
			mockFileSystem.Setup(x => x.File.Exists(filepath)).Returns(false);

			sut.DeleteFile(filepath);

			mockFileSystem.Verify(x => x.File.Delete(filepath), Times.Never());
		}

		[Theory, AutoMoqData]
		public async Task DeleteFile_FileExists(
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

		public class HttpMessageHandlerStub : HttpMessageHandler
		{
			protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
			{
				var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
				{
					Content = new StringContent("This is a reply")
				};

				return await Task.FromResult(responseMessage);
			}
		}
	}
}
