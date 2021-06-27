using System.IO;
using System.Text.Json;
using Grimoire.Line.Api.Webhook.Beacon;
using Xunit;

namespace Grimoire.Line.Api.Tests.Webhook
{
    public class Beacon
    {
        [Fact]
        public void TestBeacon()
        {
            var json = File.ReadAllText("Examples/Beacon/Beacon.json");
            var actual = JsonSerializer.Deserialize<Api.Webhook.Beacon.Beacon>(json, Json.SerializerOption);
            var expected = new Api.Webhook.Beacon.Beacon
            {
                Dm = null,
                Hwid = "d41d8cd98f",
                Type = BeaconType.Enter
            };
            Assert.Equal(expected, actual);
        }
    }
}