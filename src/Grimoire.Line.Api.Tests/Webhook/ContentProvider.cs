using System.IO;
using System.Text.Json;
using Grimoire.Line.Api.Webhook.ContentProvider;
using Xunit;

namespace Grimoire.Line.Api.Tests.Webhook
{
    public class ContentProvider
    {
        [Fact]
        public void TestExternalContentProvider()
        {
            var json = File.ReadAllText("Examples/ContentProvider/External.json");
            var actual = JsonSerializer.Deserialize<ExternalContentProvider>(json, Json.SerializerOption);
            var expected = new ExternalContentProvider
            {
                ContentType = ContentProviderType.External,
                OriginalContentUrl = "https://example.com/original.mp4",
                PreviewImageUrl = "https://example.com/preview.jpg"
            };
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void TestInternalContentProvider()
        {
            var json = File.ReadAllText("Examples/ContentProvider/Line.json");
            var actual = JsonSerializer.Deserialize<LineContentProvider>(json, Json.SerializerOption);
            var expected = new LineContentProvider
            {
                ContentType = ContentProviderType.Line
            };
            Assert.Equal(expected, actual);
        }
    }
}