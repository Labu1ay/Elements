using System;
using DG.Tweening;
using Elements.Core.Services.GameServices;
using UnityEngine;
using Zenject;

namespace Elements.Game.Elements
{
    public class Element : MonoBehaviour
    {
        private const float MOVE_DURATION = 0.25f;
        
        [Inject] private readonly IGridElementsService _gridElementsService;
        
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [field: SerializeField] public Vector2Int RoundPosition { get; private set; }

        private Tween _tween;

        private void Start()
        {
            _gridElementsService.AddElement(this);
            SetRoundPosition(new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)));
        }
        
        public void Move(MoveDirection direction)
        {
            Vector2Int newRoundPosition = direction switch
            {
                MoveDirection.UP => new Vector2Int(RoundPosition.x, RoundPosition.y + 1),
                MoveDirection.DOWN => new Vector2Int(RoundPosition.x, RoundPosition.y - 1),
                MoveDirection.LEFT => new Vector2Int(RoundPosition.x - 1, RoundPosition.y),
                MoveDirection.RIGHT => new Vector2Int(RoundPosition.x + 1, RoundPosition.y),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };

            Move(newRoundPosition);
            _gridElementsService.TryMoveSecondElement(RoundPosition, newRoundPosition);
        }

        public void Move(Vector2Int newRoundPosition)
        {
            _tween = transform
                .DOMove((Vector2)newRoundPosition, MOVE_DURATION)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    SetRoundPosition(newRoundPosition);
                });
        }

        private void SetRoundPosition(Vector2Int newRoundPosition)
        {
            RoundPosition = newRoundPosition;
            _spriteRenderer.sortingOrder = RoundPosition.x + RoundPosition.y;
        }

        private void OnDestroy()
        {
            _tween?.Kill();
        }
    }
}