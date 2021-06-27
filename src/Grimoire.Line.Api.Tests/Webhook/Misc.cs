using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Grimoire.Line.Api.Webhook;
using Grimoire.Line.Api.Webhook.Event;
using Grimoire.Line.Api.Webhook.Message;
using Grimoire.Line.Api.Webhook.Source;
using Xunit;
using Xunit.Abstractions;

namespace Grimoire.Line.Api.Tests.Webhook
{
    public class Misc
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Misc(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestMentionNotNull()
        {
            var json = File.ReadAllText("Examples/Mention/Mention.json");
            var actual = JsonSerializer.Deserialize<Mention>(json, Json.SerializerOption);
            Assert.NotNull(actual);
        }

        [Fact]
        public void TestTextMessageNotNull()
        {
            var json = File.ReadAllText("Examples/Message/TextMessage.json");
            var actual = JsonSerializer.Deserialize<BaseMessage>(json, Json.SerializerOption);
            Assert.NotNull(actual);
        }


        [Fact]
        public void TestTextEventNotNull()
        {
            var json = File.ReadAllText("Examples/Event/TextMessage.json");
            var actual = JsonSerializer.Deserialize<BaseEvent>(json, Json.SerializerOption);
            Assert.NotNull(actual);
            _testOutputHelper.WriteLine(actual.ToString());
        }

        [Fact]
        public void TestFullTextMessage()
        {
            var json = File.ReadAllText("Examples/TextMessage.json");
            var actual = JsonSerializer.Deserialize<WebhookEvent>(json, Json.SerializerOption);
            Assert.NotNull(actual);
            _testOutputHelper.WriteLine(actual.ToString());

            var expected = new WebhookEvent
            {
                Destination = "xxxxxxxxxx",
                Events = new List<BaseEvent>()
                {
                    new MessageEvent()
                    {
                        ReplyToken = "nHuyWiB7yP5Zw52FIkcQobQuGDXCTA",
                        EventType = "message",
                        Mode = Mode.Active,
                        Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(1462629479859),
                        Source = new UserSource()
                        {
                            SourceType = SourceType.User,
                            UserId = "U4af4980629..."
                        },
                        Message = new TextMessage
                        {
                            Id = "325078",
                            MessageType = MessageType.Text,
                            Text = "@example Hello, world! (love)",
                            Emojis = new List<Emoji>()
                            {
                                new Emoji()
                                {
                                    Index = 23,
                                    Length = 6,
                                    ProductId = "5ac1bfd5040ab15980c9b435",
                                    EmojiId = "001"
                                }
                            },
                            Mention = new Mention()
                            {
                                Mentionees = new List<Mentionee>()
                                {
                                    new Mentionee()
                                    {
                                        Index = 0,
                                        Length = 0,
                                        UserId = "U850014438e..."
                                    }
                                }
                            }
                        }
                    }
                }
            };
            
            
        }
    }
}