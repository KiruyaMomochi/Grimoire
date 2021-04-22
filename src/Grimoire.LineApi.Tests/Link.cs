using System.IO;
using System.Text.Json;
using Xunit;

// ReSharper disable StringLiteralTypo

namespace Grimoire.LineApi.Tests
{
    public class Link
    {
        [Fact]
        public void TestLink()
        {
            var json = File.ReadAllText("Examples/Link/Link.json");
            var actual = JsonSerializer.Deserialize<AccountLink.Link>(json, Json.SerializerOption);
            var expected = new AccountLink.Link
            {
                Result = "ok",
                Nonce = "xxxxxxxxxxxxxxx"
            };
            Assert.Equal(expected, actual);
        }
    }
}