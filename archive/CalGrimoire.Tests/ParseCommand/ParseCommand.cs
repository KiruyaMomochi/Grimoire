using Grimoire.Parser;
using Grimoire.Parser.Parameters;
using Xunit;

namespace Grimoire.Tests.ParseCommand
{
    public class GroupCommand
    {
        [Fact]
        public void ReserveCommand()
        {
            var everything = CommandParser.ParseGroupCommand("預約 1 3 補償 喵喵喵 ");
            Assert.Equal(new ReserveParameter {Comment = "喵喵喵", Lap = 1, Order = 3, IsRemain = true},
                everything);

            var noReserve = CommandParser.ParseGroupCommand("約 2 5 喵喵喵");
            Assert.Equal(new ReserveParameter {Comment = "喵喵喵", Lap = 2, Order = 5,},
                noReserve);

            var reserve = CommandParser.ParseGroupCommand("約 殘");
            Assert.Equal(new ReserveParameter {IsRemain = true}, reserve);
        }
        
        [Fact]
        public void HangCommand()
        {
            var actual = CommandParser.ParseGroupCommand("掛樹 114 514 喵喵喵");
            var expected = new ReportParameter
            {
                Comment = "喵喵喵",
                Lap = 114,
                Order = 514,
                IsHang = true,
                IsRemain = false
            };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RemainCommand()
        {
            var everything = CommandParser.ParseGroupCommand("補 1 2 喵喵喵");
            Assert.Equal(new ReportParameter
                    {Comment = "喵喵喵", Lap = 1, Order = 2, IsRemain = true},
                everything);

            var numberString = CommandParser.ParseGroupCommand("補償 5 測試");
            Assert.Equal(new ReportParameter
                    {Comment = "5 測試", IsRemain = true},
                numberString);
        }

        [Fact]
        public void ReportCommand()
        {
            var everything = CommandParser.ParseGroupCommand("報 14 2 掛 殘刀 喵喵喵");
            Assert.Equal(new ReportParameter
                    {Comment = "喵喵喵", Lap = 14, Order = 2, IsHang = true, IsRemain = true},
                everything);

            var reserve = CommandParser.ParseGroupCommand("報 3 1 補償 掛樹 喵喵喵");
            Assert.Equal(new ReportParameter
                    {Comment = "喵喵喵", Lap = 3, Order = 1, IsHang = true, IsRemain = true},
                reserve);

            var noComment = CommandParser.ParseGroupCommand("報刀 2 1 殘");
            Assert.Equal(new ReportParameter {Lap = 2, Order = 1, IsRemain = true},
                noComment);

            var numberString = CommandParser.ParseGroupCommand("報 2 測試");
            Assert.Equal(new ReportParameter {Comment = "2 測試"}, numberString);

            var number = CommandParser.ParseGroupCommand("報 1");
            Assert.Equal(new ReportParameter {Comment = "1",}, number);

            var comment = CommandParser.ParseGroupCommand("報 nya");
            Assert.Equal(new ReportParameter {Comment = "nya",}, comment);
        }

        [Fact]
        public void RollCommand()
        {
            var back1 = CommandParser.ParseGroupCommand("退");
            Assert.Equal(RollParameter.Back, back1);

            var back5 = CommandParser.ParseGroupCommand("退 5");
            Assert.Equal(new RollParameter {Delta = -5}, back5);

            var forward1 = CommandParser.ParseGroupCommand("倒");
            Assert.Equal(RollParameter.Forward, forward1);

            var forward8 = CommandParser.ParseGroupCommand("倒  8 ");
            Assert.Equal(new RollParameter {Delta = 8}, forward8);

            var back2 = CommandParser.ParseGroupCommand("倒  -2 ");
            Assert.Equal(new RollParameter {Delta = -2}, back2);
        }

        [Fact]
        public void SwitchCommand()
        {
            var full = CommandParser.ParseGroupCommand("切 3 1");
            Assert.Equal(new SwitchParameter {Lap = 3, Order = 1}, full);

            var orderOnly = CommandParser.ParseGroupCommand("切 1");
            Assert.Equal(new SwitchParameter {Order = 1}, orderOnly);
        }

        [Fact]
        public void ReserveListCommand()
        {
            var actual = CommandParser.ParseGroupCommand("預約清單");
            var expected = new ReserveListParameter();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CancelReserveCommand()
        {
            var none = CommandParser.ParseGroupCommand("取消預約");
            Assert.Equal(CancelReserveParameter.Empty, none);
            
            var full = CommandParser.ParseGroupCommand("取消預約 1 2");
            Assert.Equal(new CancelReserveParameter{Lap = 1, Order = 2}, full);
        }

        [Fact]
        public void ParseEmptyCommand()
        {
            var empty = CommandParser.ParseGroupCommand("");
            Assert.Equal(NoneParameter.Empty, empty);
            
            var invalid = CommandParser.ParseGroupCommand("what");
            Assert.Equal(NoneParameter.Empty, invalid);
        }
    }
}