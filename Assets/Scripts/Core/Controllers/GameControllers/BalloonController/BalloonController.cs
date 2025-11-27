using System;
using System.Collections.Generic;
using Elements.Core.Services.GlobalServices;
using Elements.Game.Balloon;
using Elements.Utils;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Elements.Core.Controllers.GameControllers
{
    public class BalloonController : IInitializable, ILateDisposable
    {
        private const string BALLOON_PATH = "Balloon";
        private const int MAX_BALLOONS = 3;
        private const float MAX_Y = 8f;
        private const float MIN_Y = 3.5f;
        private const float SPAWN_X_OFFSET = 5;

        private readonly IAssetService _assetService;

        private Vector2 _spawnInterval = new (2f, 5f);
        private float _timer;
        private ObjectPool<BalloonMover> _pool;
        private readonly List<BalloonMover> _activeBalloons = new();
        
        private BalloonMover _balloonPrefab;
        private Transform _content;

        private IDisposable _disposable;

        [Inject]
        public BalloonController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        public void Initialize()
        {
            _content = new GameObject("Balloons").transform;
            _balloonPrefab = _assetService.Load<BalloonMover>(BALLOON_PATH);
            _pool = new ObjectPool<BalloonMover>(_balloonPrefab, _content, MAX_BALLOONS);

            _disposable = Observable.EveryUpdate().Subscribe(_ =>
            {
                _timer -= Time.deltaTime;

                if (_activeBalloons.Count < MAX_BALLOONS && _timer <= 0f)
                {
                    SpawnBalloon();
                    _timer = Random.Range(_spawnInterval.x, _spawnInterval.y);
                }

                for (int i = _activeBalloons.Count - 1; i >= 0; i--)
                {
                    var balloon = _activeBalloons[i];
                    
                    if (!balloon.gameObject.activeInHierarchy) 
                        _activeBalloons.RemoveAt(i);
                }
            });
        }

        private void SpawnBalloon()
        {
            var moveRight = Random.value > 0.5f;
            var startX = moveRight ? -SPAWN_X_OFFSET : SPAWN_X_OFFSET;
            var startY = Random.Range(MIN_Y, MAX_Y);

            var balloon = _pool.Instantiate(_balloonPrefab, _content, new Vector3(startX, startY, 0f),
                Quaternion.identity);

            balloon.Init(moveRight, OnBalloonDespawn);

            _activeBalloons.Add(balloon);
        }

        private void OnBalloonDespawn(BalloonMover balloon) => _pool.Destroy(balloon);

        public void LateDispose()
        {
            _disposable?.Dispose();
        }
    }
}