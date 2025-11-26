using System;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Elements.Game.Balloon
{
    public class BalloonMover : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite[] _balloonSprites;
        
        private float _speed;
        private float _amplitude;
        private float _frequency;
        private bool _moveRight;
        private float _lifeMaxX = 7f;
        private Vector3 _startPos;
        private float _time;

        private Action<BalloonMover> _onDespawn;

        private IDisposable _disposable;

        public void Init(bool moveRight, Action<BalloonMover> onDespawn)
        {
            _spriteRenderer.sprite = _balloonSprites[Random.Range(0, _balloonSprites.Length)];
            
            _moveRight = moveRight;
            _onDespawn = onDespawn;
            _speed = Random.Range(0.5f, 1.5f);
            _amplitude = Random.Range(0.3f, 1f);
            _frequency = Random.Range(1f, 2.5f);
            _startPos = transform.position;
            _time = 0f;
            gameObject.SetActive(true);

            _disposable = Observable.EveryUpdate().Subscribe(_ => EveryUpdate());
        }

        private void EveryUpdate()
        {
            _time += Time.deltaTime;
            var direction = _moveRight ? 1f : -1f;

            var x = _startPos.x + direction * _speed * _time;
            var y = _startPos.y + Mathf.Sin(_time * _frequency) * _amplitude;

            transform.position = new Vector3(x, y, transform.position.z);

            if (Mathf.Abs(transform.position.x) > _lifeMaxX) 
                _onDespawn?.Invoke(this);
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
        }
    }
}