using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Elements.Game.Elements;
using UnityEngine;
using Zenject;

namespace Elements.Core.Services.GameServices
{
    public class GridElementsService : IGridElementsService
    {
        private readonly IMatchElementsService _matchElementsService;
        
        private List<Element> _elements = new List<Element>();

        [Inject]
        public GridElementsService(IMatchElementsService matchElementsService)
        {
            _matchElementsService = matchElementsService;
        }

        public void AddElement(Element element) => _elements.Add(element);
        public void RemoveElement(Element element) => _elements.Remove(element);

        public void TryMoveSecondElement(Vector2Int currentRoundPosition, Vector2Int newRoundPosition)
        {
            if (!ContainsElementInPosition(newRoundPosition)) return;
            
            var element = _elements.First(e => e.RoundPosition == newRoundPosition);
            element.Move(currentRoundPosition);
        }

        public bool ContainsElementInPosition(Vector2Int roundPosition) => 
            _elements.Any(e => e.RoundPosition == roundPosition);

        public async UniTask TryFallElements()
        {
            _elements.Sort((a, b) => a.RoundPosition.x == b.RoundPosition.x 
                    ? a.RoundPosition.y.CompareTo(b.RoundPosition.y) 
                    : a.RoundPosition.x.CompareTo(b.RoundPosition.x));
            
            foreach (var element in _elements)
            {
                var needPosition = element.RoundPosition;

                for (int i = element.RoundPosition.y; i >= 0; i--)
                {
                    var newRoundPosition = new Vector2Int(element.RoundPosition.x, i);
                    if (!ContainsElementInPosition(newRoundPosition))
                        needPosition = newRoundPosition;
                }
            
                if(needPosition == element.RoundPosition) continue;
                
                element.Fall(needPosition);
            }
            
            await UniTask.WaitWhile(() => _elements.Any(e => e.IsInteraction));

            TryMatchElements().Forget();
        }

        private async UniTask TryMatchElements()
        {
            var allGroups = _matchElementsService.FindConnectedGroups(_elements);

            foreach (var element in allGroups.SelectMany(group => group))
                element.Destroy().Forget();

            if (allGroups.Count > 0)
            {
                await UniTask.WaitWhile(() => _elements.Any(e => e.IsInteraction));
                TryFallElements().Forget();
            }

            foreach (var group in allGroups) 
                group.Clear();
            
            allGroups.Clear();
        }
    }
}