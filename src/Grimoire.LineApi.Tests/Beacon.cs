using System.IO;
using System.Text.Json;
using Grimoire.LineApi.Beacon;
using Xunit;

namespace Grimoire.LineApi.Tests
{
    public class Beacon
    {
        [Fact]
        public void TestBeacon()
        {
            var json = File.ReadAllText("Examples/Beacon/Beacon.json");
            var actual = JsonSerializer.Deserialize<LineApi.Beacon.Beacon>(json, Json.SerializerOption);
            var expected = new LineApi.Beacon.Beacon
            {
                Dm = null,
                Hwid = "d41d8cd98f",
                Type = BeaconType.Enter
            };
            Assert.Equal(expected, actual);
        }
    }
}