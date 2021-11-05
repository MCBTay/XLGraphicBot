using AutoFixture.Xunit2;
using Discord;
using Discord.Commands;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using XLGraphicBot.modules;
using XLGraphicBot.services;
using XLGraphicBot.UnitTest.Core;
using Xunit;

namespace XLGraphicBot.UnitTest.services
{
	[ExcludeFromCodeCoverage]
	public class DiscordServiceUnitTest
	{
		[Theory, AutoMoqData]
		public async Task GetMostRecentImage_NoMessages(
			Mock<ISelfUser> mockSelfUser,
			[Frozen] Mock<ICommandContext> mockCommandContext,
			DiscordService sut)
		{
			mockCommandContext.SetupCommandContext(mockSelfUser.Object);

			(Bitmap image, string filename) = await sut.GetMostRecentImage(mockCommandContext.Object);

			image.Should().BeNull();
			filename.Should().BeEmpty();
		}

		[Theory, AutoMoqData]
		public async Task GetMostRecentImage_MessagesMatchCurrentUser(
			List<List<IMessage>> messages,
			Mock<ISelfUser> mockSelfUser,
			ulong selfUserId,
			[Frozen] Mock<ICommandContext> mockCommandContext,
			DiscordService sut)
		{
			mockSelfUser.SetupGet(x => x.Id).Returns(selfUserId);

			messages.SetupMessages(mockSelfUser.Object.Id);

			mockCommandContext.SetupCommandContext(mockSelfUser.Object, messages);

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
			DiscordService sut)
		{
			mockSelfUser.SetupGet(x => x.Id).Returns(selfUserId);

			messages.SetupMessages(otherUserId);

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
			List<IAttachment> attachments,
			Mock<IMessageChannel> mockMessageChannel,
			[Frozen] Mock<ICommandContext> mockCommandContext,
			DiscordService sut)
		{
			mockSelfUser.SetupGet(x => x.Id).Returns(selfUserId);

			messages.SetupMessages(otherUserId, attachments);

			mockDiscordClient
				.SetupGet(x => x.CurrentUser)
				.Returns(mockSelfUser.Object);

			mockCommandContext
				.SetupGet(x => x.Client)
				.Returns(mockDiscordClient.Object);

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
		public async Task GetMostRecentImage_FoundAttachment_WrittenSuccessfully(
			List<List<IMessage>> messages,
			Mock<ISelfUser> mockSelfUser,
			ulong selfUserId,
			ulong otherUserId,
			List<IAttachment> attachments,
			[Frozen] Mock<IBitmapService> mockBitmapService,
			[Frozen] Mock<ICommandContext> mockCommandContext,
			DiscordService sut)
		{
			mockSelfUser.SetupGet(x => x.Id).Returns(selfUserId);

			attachments.SetupAttachments();
			messages.SetupMessages(otherUserId, attachments);

			mockCommandContext.SetupCommandContext(mockSelfUser.Object, messages);

			mockBitmapService
				.Setup(x => x.CreateBitmap(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(new Bitmap(100, 100));

			(Bitmap image, string filename) = await sut.GetMostRecentImage(mockCommandContext.Object);

			image.Should().NotBeNull();
			filename.Should().NotBeNullOrEmpty();
		}
	}
}
