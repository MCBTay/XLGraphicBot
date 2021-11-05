using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Discord;
using Discord.Commands;
using Moq;

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
  }
}