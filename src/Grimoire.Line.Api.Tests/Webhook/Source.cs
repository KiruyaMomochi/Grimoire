using System.IO;
using System.Text.Json;
using Grimoire.Line.Api.Webhook.Source;
using Xunit;

namespace Grimoire.Line.Api.Tests.Webhook
{
    public class Source
    {
        [Fact]
        public void TestUserSource()
        {
            var json = File.ReadAllText("Examples/Source/UserSource.json");
            var actual = JsonSerializer.Deserialize<UserSource>(json, Json.SerializerOption);
            var expected = new UserSource
            {
                SourceType = SourceType.User,
                UserId = "U4af4980629..."
            };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestGroupSource()
        {
            var json = File.ReadAllText("Examples/Source/GroupSource.json");
            var actual = JsonSerializer.Deserialize<GroupSource>(json, Json.SerializerOption);
            var expected = new GroupSource
            {
                SourceType = SourceType.Group,
                GroupId = "Ca56f94637c...",
                UserId = "U4af4980629..."
            };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestRoomSource()
        {
            var json = File.ReadAllText("Examples/Source/RoomSource.json");
            var actual = JsonSerializer.Deserialize<RoomSource>(json, Json.SerializerOption);
            var expected = new RoomSource
            {
                SourceType = SourceType.Room,
                RoomId = "Ra8dbf4673c...",
                UserId = "U4af4980629..."
            };
        }

        [Fact]
        public void TestMemberList()
        {
            var json = File.ReadAllText("Examples/Source/MemberList.json");
            var actual = JsonSerializer.Deserialize<MemberList>(json, Json.SerializerOption);
            
            var user1 = new UserSource()
            {
                SourceType = SourceType.User,
                UserId = "U4af4980629..."
            };
            var user2 = new UserSource()
            {
                SourceType = SourceType.User,
                UserId = "U91eeaf62d9..."
            };

            Assert.NotNull(actual);
            Assert.NotNull(actual.Members);
            Assert.Equal(2, actual.Members.Count);
            Assert.Equal(user1, actual.Members[0]);
            Assert.Equal(user2, actual.Members[1]);
        }
    }
}