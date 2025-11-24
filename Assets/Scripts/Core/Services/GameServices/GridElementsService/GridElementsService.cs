using System.Collections.Generic;
using System.Linq;
using Elements.Game.Elements;
using UnityEngine;

namespace Elements.Core.Services.GameServices
{
    public class GridElementsService : IGridElementsService
    {
        private List<Element> _elements = new List<Element>();

        public void AddElement(Element element) => _elements.Add(element);

        public void TryMoveSecondElement(Vector2Int currentRoundPosition, Vector2Int newRoundPosition)
        {
            if (_elements.Any(e => e.RoundPosition == newRoundPosition))
            {
                var element = _elements.First(e => e.RoundPosition == newRoundPosition);
                element.Move(currentRoundPosition);
            }
        }
    }
}