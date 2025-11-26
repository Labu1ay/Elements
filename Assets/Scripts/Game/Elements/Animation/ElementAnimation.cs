using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Elements.Game.Elements.Animation
{
    [RequireComponent(typeof(Animator))]
    public class ElementAnimation : MonoBehaviour
    {
        private const float MAX_START_DELAY = 1f;
        private const float SCALE_OFFSET = 0.03f;
        private const float ANIMATION_STEP_DURATION = 2f;
        
        [SerializeField] private Animator _animator;

        private Dictionary<AnimationType, int> _animations = new Dictionary<AnimationType, int>()
        {
            { AnimationType.DESTROY , Animator.StringToHash("Destroy") }
        };

        private Vector3 _startScale;
        private Sequence _sequence;

        private void OnValidate()
        {
            _animator ??= GetComponent<Animator>();
        }

        private  void Start()
        {
            AddStartAnimationDelay().Forget();
        }

        private async UniTask AddStartAnimationDelay()
        {
            _animator.enabled = false;
            await UniTask.Delay(TimeSpan.FromSeconds(Random.Range(0f, MAX_START_DELAY)));
            _animator.enabled = true;
            
            AddScaleAnimation();
        }

        private void AddScaleAnimation()
        {
            _startScale = transform.localScale;
            
            _sequence = DOTween.Sequence();

            var maxYScale = new Vector3(_startScale.x * (1f - SCALE_OFFSET), _startScale.y * (1f + SCALE_OFFSET), _startScale.z);
            var maxXScale = new Vector3(_startScale.x * (1f + SCALE_OFFSET), _startScale.y * (1f - SCALE_OFFSET), _startScale.z);
            
            _sequence
                .Append(transform.DOScale(maxYScale, ANIMATION_STEP_DURATION))
                .Append(transform.DOScale(_startScale, ANIMATION_STEP_DURATION))
                .Append(transform.DOScale(maxXScale, ANIMATION_STEP_DURATION))
                .Append(transform.DOScale(_startScale, ANIMATION_STEP_DURATION))
                .SetLoops(-1);
        }

        public void SetAnimation(AnimationType animationType) => 
            _animator.SetTrigger(_animations[animationType]);

        public async UniTask AsyncSetAnimation(AnimationType animationType, CancellationToken token = default)
        {
            SetAnimation(animationType);
            await UniTask.WaitForEndOfFrame(cancellationToken: token);
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            await UniTask.Delay(TimeSpan.FromSeconds(stateInfo.length), cancellationToken: token);
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }
    }
}