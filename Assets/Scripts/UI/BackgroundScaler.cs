using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Elements.UI
{
#if UNITY_EDITOR
    [ExecuteAlways]
#endif
    public class BackgroundScaler : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
            ScaleBackground().Forget();
            
            SetPosition().Forget();
        }

        private async UniTaskVoid SetPosition()
        {
            await UniTask.WaitForEndOfFrame();
            transform.position = new Vector3(_mainCamera.transform.position.x, transform.position.y, transform.position.z);
        }

        private void Update()
        {
#if UNITY_EDITOR
            ScaleBackground();
#endif
        }

        private async UniTaskVoid ScaleBackground()
        {
            await UniTask.WaitForEndOfFrame();
            
            if (_spriteRenderer == null || _mainCamera == null)
                return;

            var cameraHeight = 2f * _mainCamera.orthographicSize;
            var cameraWidth = cameraHeight * _mainCamera.aspect;

            var spriteSize = _spriteRenderer.sprite.bounds.size;

            float scaleX = cameraWidth / spriteSize.x;
            float scaleY = cameraHeight / spriteSize.y;

            float scale = Mathf.Max(scaleX, scaleY);

            transform.localScale = new Vector3(scale, scale, transform.localScale.z);
        }
    }
}