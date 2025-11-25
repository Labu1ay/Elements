using UnityEngine;

namespace Elements.UI
{
    [ExecuteAlways]
    public class BackgroundScaler : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private Camera _mainCamera;

        void Start()
        {
            _mainCamera = Camera.main;
            ScaleBackground();
        }

        void Update()
        {
#if UNITY_EDITOR
            ScaleBackground();
#endif
        }

        private void ScaleBackground()
        {
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