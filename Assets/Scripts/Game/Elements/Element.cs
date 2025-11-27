using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Elements.Core.Services.GameServices;
using Elements.Game.Elements.Animation;
using UnityEngine;
using Zenject;

namespace Elements.Game.Elements
{
    public class Element : MonoBehaviour
    {
        private const float MOVE_DURATION = 0.25f;
        private const float FALL_SPEED = 15f;
        
        [Inject] private readonly IGridElementsService _gridElementsService;
        [Inject] private readonly ILevelControlService _levelControlService;
        
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private ElementAnimation _elementAnimation;
        [field: SerializeField] public ElementType Type { get; private set; }
        
        public bool IsInteraction { get; private set; }
        public Vector2Int RoundPosition { get; private set; }

        private CancellationTokenSource _cts;
        private Tween _tween;

        private void Start()
        {
            _gridElementsService.AddElement(this);
            InitRoundPosition();
        }

        public void InitRoundPosition()
        {
            SetRoundPosition(new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)));
            UpdateSortingOrder(RoundPosition);
        }

        public void Move(MoveDirection direction)
        {
            var newRoundPosition = CalculateNewPosition(direction);
    
            if (!CanMove(direction, newRoundPosition))
                return;
    
            _gridElementsService.TryMoveSecondElement(RoundPosition, newRoundPosition);
    
            Move(newRoundPosition, async () =>
            {
                await UniTask.WaitForEndOfFrame();
                _gridElementsService.TryFallElements().Forget();
            });
        }

        private Vector2Int CalculateNewPosition(MoveDirection direction) => direction switch
        {
            MoveDirection.UP    => new Vector2Int(RoundPosition.x, RoundPosition.y + 1),
            MoveDirection.DOWN  => new Vector2Int(RoundPosition.x, RoundPosition.y - 1),
            MoveDirection.LEFT  => new Vector2Int(RoundPosition.x - 1, RoundPosition.y),
            MoveDirection.RIGHT => new Vector2Int(RoundPosition.x + 1, RoundPosition.y),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        private bool CanMove(MoveDirection direction, Vector2Int newPosition) => direction switch
        {
            MoveDirection.UP    => _gridElementsService.ContainsElementInPosition(newPosition),
            MoveDirection.DOWN  => newPosition.y >= 0,
            MoveDirection.LEFT  => newPosition.x > _levelControlService.LeftBorderX,
            MoveDirection.RIGHT => newPosition.x < _levelControlService.RightBorderX,
            _ => false
        };

        public void Move(Vector2Int newRoundPosition, Action callback = null)
        {
            UpdateSortingOrder(newRoundPosition);
            
            IsInteraction = true;
            
            _tween = transform
                .DOMove((Vector2)newRoundPosition, MOVE_DURATION)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    SetRoundPosition(newRoundPosition);
                    callback?.Invoke();
                    IsInteraction = false;
                });
        }

        public void Fall(Vector2Int newRoundPosition)
        {
            UpdateSortingOrder(newRoundPosition);
            SetRoundPosition(newRoundPosition);

            IsInteraction = true;
            
            _tween = transform
                .DOMove((Vector2)newRoundPosition, FALL_SPEED)
                .SetEase(Ease.Linear)
                .SetSpeedBased()
                .OnComplete(() => { IsInteraction = false; });
        }

        public async UniTaskVoid Destroy()
        {
            IsInteraction = true;
            _cts = new CancellationTokenSource();
            bool isCancelled = await _elementAnimation.AsyncSetAnimation(AnimationType.DESTROY, _cts.Token)
                .SuppressCancellationThrow();
            
            if (isCancelled) return;
            _gridElementsService.RemoveElement(this);
            Destroy(gameObject);
        }
        
        private void UpdateSortingOrder(Vector2Int roundPosition) 
            => _spriteRenderer.sortingOrder = roundPosition.x + roundPosition.y;
           

        private void SetRoundPosition(Vector2Int newRoundPosition) => 
            RoundPosition = newRoundPosition;

        private void Cancel()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private void OnDestroy()
        {
            Cancel();
            _tween?.Kill();
        }
    }
}