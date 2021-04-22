using System;
using System.IO;
using System.Text.Json;
using Grimoire.LineApi.Postback;
using Xunit;

namespace Grimoire.LineApi.Tests
{
    public class Postback
    {
        [Fact]
        public void TestDateTimePostbackParam()
        {
            var json = File.ReadAllText("Examples/Postback/Datetime.json");
            var actual = JsonSerializer.Deserialize<BasePostbackParam>(json, Json.SerializerOption);
            var expected = new DateTimePostbackParam
            {
                DateTime = DateTime.Parse("2017-12-25T01:00")
            };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestDatePostbackParam()
        {
            var json = File.ReadAllText("Examples/Postback/Date.json");
            var actual = JsonSerializer.Deserialize<BasePostbackParam>(json, Json.SerializerOption);
            var expected = new DatePostbackParam()
            {
                Date = "2017-12-25"
            };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestTimePostbackParam()
        {
            var json = File.ReadAllText("Examples/Postback/Time.json");
            var actual = JsonSerializer.Deserialize<BasePostbackParam>(json, Json.SerializerOption);
            var expected = new TimePostbackParam()
            {
                Time = "01:00"
            };
            Assert.Equal(expected, actual);
        }
    }
}