using Elements.Core.Services;
using Elements.Game.Elements;
using UnityEngine;
using Zenject;

namespace Elements.Core.Controllers.GameControllers
{
    public class ElementMovementController : IInitializable, ILateDisposable
    {
        private readonly IInputService _inputService;
        
        private Element _currentElement;
        private Camera _camera;

        private Vector2 _downTouchPosition;

        [Inject]
        public ElementMovementController(IInputService inputService)
        {
            _inputService = inputService;
        }

        public void Initialize()
        {
            _camera = Camera.main;
            _inputService.IsTouchDown += IsTouchDown;
        }

        private void IsTouchDown(bool isTouchDown)
        {
            if(isTouchDown)
                TouchDown();
            else
                TouchUp();
        }
        
        private void TouchDown()
        {
            Vector2 worldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
    
            var hit = Physics2D.Raycast(worldPosition, Vector2.zero, 
                Mathf.Infinity, 1 << LayerMask.NameToLayer(Constants.ELEMENT_LAYER));

            if (hit.collider?.TryGetComponent(out Element element) != true) return;
            
            _downTouchPosition = Input.mousePosition;
            _currentElement = element;
        }

        private void TouchUp()
        {
            if(_currentElement == null) return;
            
            var upTouchPosition = Input.mousePosition;
            
            var yOffset = upTouchPosition.y - _downTouchPosition.y;
            var xOffset = upTouchPosition.x - _downTouchPosition.x;

            if (Mathf.Abs(xOffset) > Mathf.Abs(yOffset))
                _currentElement.Move(xOffset > 0 ? MoveDirection.RIGHT : MoveDirection.LEFT);
            else
                _currentElement.Move(yOffset > 0 ? MoveDirection.UP : MoveDirection.DOWN);
            
            _currentElement = null;
        }

        public void LateDispose()
        {
            _inputService.IsTouchDown -= IsTouchDown;
        }
    }
}