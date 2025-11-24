using UnityEngine;

namespace Elements.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(SpriteRenderer))]
    public class BackgroundScaler : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private Camera _mainCamera;

        private void OnValidate()
        {
            _spriteRenderer ??= GetComponent<SpriteRenderer>();
        }

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
            if (_spriteRenderer == null || _mainCamera == null) return;

            var worldHeight = 2f * _mainCamera.orthographicSize;
            var worldWidth = worldHeight * _mainCamera.aspect;

            var spriteSize = _spriteRenderer.sprite.bounds.size;

            var scaleX = worldWidth / spriteSize.x;
            var scaleY = worldHeight / spriteSize.y;
            var scale = Mathf.Max(scaleX, scaleY);

            transform.localScale = new Vector3(scale, scale, 1f);

            var cameraBottom = _mainCamera.transform.position.y - worldHeight / 2f;
            var spriteBottom = _spriteRenderer.bounds.min.y;
            var delta = cameraBottom - spriteBottom;

            transform.position += new Vector3(0, delta, 0);
        }
    }
}