namespace Grimoire.Line.Api.Flex
{
    public record BubbleFlex : BaseFlex
    {
        public BubbleFlex()
        {
            FlexType = FlexType.Bubble;
        }

        public BubbleSize Size { get; set; } = BubbleSize.Mega;
        public Direction Direction { get; set; } = Direction.Ltr;
        // TODO
    }
}