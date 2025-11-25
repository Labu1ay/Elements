using Cysharp.Threading.Tasks;
using Elements.Game.Elements;
using UnityEngine;

namespace Elements.Core.Services.GameServices
{
    public interface IGridElementsService
    {
        void AddElement(Element element);
        void RemoveElement(Element element);
        void TryMoveSecondElement(Vector2Int currentRoundPosition, Vector2Int newRoundPosition);
        bool ContainsElementInPosition(Vector2Int roundPosition);
        UniTask TryFallElements();

    }
}