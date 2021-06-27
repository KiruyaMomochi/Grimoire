using System.IO;
using System.Text.Json;
using Grimoire.Line.Api.Webhook.ContentProvider;
using Grimoire.Line.Api.Webhook.Message;
using Xunit;

namespace Grimoire.Line.Api.Tests.Webhook
{
    public class Message
    {
        [Fact]
        public void TestAudioMessage()
        {
            var json = File.ReadAllText("Examples/Message/AudioMessage.json");
            var actual = JsonSerializer.Deserialize<AudioMessage>(json, Json.SerializerOption);
            var expected = new AudioMessage
            {
                Id = "325708",
                MessageType = MessageType.Audio,
                Duration = 60000,
                ContentProvider = new LineContentProvider {ContentType = ContentProviderType.Line}
            };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestEmoji()
        {
            var json = File.ReadAllText("Examples/Message/Emoji.json");
            var actual = JsonSerializer.Deserialize<Emoji>(json, Json.SerializerOption);
            var expected = new Emoji
            {
                Index = 23,
                Length = 6,
                ProductId = "5ac1bfd5040ab15980c9b435",
                EmojiId = "001"
            };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestFileMessage()
        {
            var json = File.ReadAllText("Examples/Message/FileMessage.json");
            var actual = JsonSerializer.Deserialize<FileMessage>(json, Json.SerializerOption);
            var expected = new FileMessage
            {
                Id = "325708",
                FileName = "file.txt",
                FileSize = 2138,
                MessageType = MessageType.File
            };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestLocationMessage()
        {
            var json = File.ReadAllText("Examples/Message/LocationMessage.json");
            var actual = JsonSerializer.Deserialize<LocationMessage>(json, Json.SerializerOption);
            var expected = new LocationMessage
            {
                Id = "325708",
                MessageType = MessageType.Location,
                Title = "my location",
                Address = "〒150-0002 東京都渋谷区渋谷２丁目２１−１",
                Latitude = 35.65910807942215,
                Longitude = 139.70372892916203
            };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestMentionee()
        {
            var json = File.ReadAllText("Examples/Message/Mentionee.json");
            var actual = JsonSerializer.Deserialize<Mentionee>(json, Json.SerializerOption);
            var expected = new Mentionee
            {
                Index = 0,
                Length = 8,
                UserId = "U850014438e..."
            };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestMention()
        {
            var json = File.ReadAllText("Examples/Message/Mention.json");
            var actual = JsonSerializer.Deserialize<Mention>(json, Json.SerializerOption);
            var mentionee = new Mentionee
            {
                Index = 0,
                Length = 8,
                UserId = "U850014438e..."
            };

            Assert.NotNull(actual);
            Assert.NotNull(actual.Mentionees);
            Assert.Single(actual.Mentionees);
            Assert.Equal(mentionee, actual.Mentionees[0]);
        }

        [Fact]
        public void TestStickerMessage()
        {
            var json = File.ReadAllText("Examples/Message/StickerMessage.json");
            var actual = JsonSerializer.Deserialize<StickerMessage>(json, Json.SerializerOption);

            Assert.NotNull(actual);
            Assert.Equal(MessageType.Sticker, actual.MessageType);
            Assert.Equal("1501597916", actual.Id);
            Assert.Equal("52002738", actual.StickerId);
            Assert.Equal("11537", actual.PackageId);
            Assert.Equal("ANIMATION", actual.StickerResourceType);
            Assert.Equal(
                new string[]
                {
                    "cony", "sally", "Staring", "hi", "whatsup", "line", "howdy", "HEY", "Peeking", "wave", "peek",
                    "Hello", "yo", "greetings"
                }, actual.Keywords);
        }

        [Fact]
        public void TestTextMessage()
        {
            var json = File.ReadAllText("Examples/Message/TextMessage.json");
            var actual = JsonSerializer.Deserialize<TextMessage>(json, Json.SerializerOption);
            
            Assert.NotNull(actual);
            Assert.Equal("325708", actual.Id);
            Assert.Equal(MessageType.Text, actual.MessageType);
            Assert.Equal("@example Hello, world! (love)", actual.Text);
            
            Assert.Single(actual.Emojis);
            Assert.NotNull(actual.Mention);
            Assert.Single(actual.Mention.Mentionees);
        }

        [Fact]
        public void TestVideoMessage()
        {
            var json = File.ReadAllText("Examples/Message/VideoMessage.json");
            var actual = JsonSerializer.Deserialize<VideoMessage>(json, Json.SerializerOption);
            var expected = new VideoMessage
            {
                Id = "325708",
                MessageType = MessageType.Video,
                Duration = 60000,
                ContentProvider = new ExternalContentProvider
                {
                    ContentType = ContentProviderType.External,
                    OriginalContentUrl = "https://example.com/original.mp4",
                    PreviewImageUrl = "https://example.com/preview.jpg"
                }
            };
            Assert.Equal(expected, actual);
        }
    }
}