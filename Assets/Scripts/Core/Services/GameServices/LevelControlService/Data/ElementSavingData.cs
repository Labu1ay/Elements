using Elements.Game.Elements;

namespace Elements.Core.Services.GameServices.Data
{
    public struct ElementSavingData
    {
        public ElementType Type;
        public (int, int) Position;

        public ElementSavingData(ElementType type, (int, int) position)
        {
            Type = type;
            Position = position;
        }
    }
}