using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Elements.Core.Services
{
    public class InputService : IInputService, IInitializable, ILateDisposable
    {
        public event Action<bool> IsTouchDown; 
        private IDisposable _disposable;
        
        public void Initialize()
        {
            _disposable = Observable.EveryUpdate().Subscribe(_ =>
            {
                if(Input.GetMouseButtonDown(0)) IsTouchDown?.Invoke(true);
                else if(Input.GetMouseButtonUp(0)) IsTouchDown?.Invoke(false);
            });
        }

        public void LateDispose()
        {
            _disposable?.Dispose();
        }
    }
}