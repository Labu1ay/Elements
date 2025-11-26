using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class CameraRelativePosition : MonoBehaviour
{
    [Range(0f, 1f)] [SerializeField] private float _normalizedOffsetFromBottom = 0.2f;
    private Camera _camera;
    
    private void Start()
    {
        _camera = GetComponent<Camera>();
        AdjustCameraPosition();
    }
    
    private void Update()
    {
#if UNITY_EDITOR
        AdjustCameraPosition();
#endif
    }

    private void AdjustCameraPosition()
    {
        if (!_camera.orthographic) return;

        var halfHeight = _camera.orthographicSize;
        var worldHeight = halfHeight * 2f;

        var offsetWorldUnits = worldHeight * _normalizedOffsetFromBottom;

        var desiredCameraY = halfHeight - offsetWorldUnits;

        var position = _camera.transform.position;
        position.y = desiredCameraY;
        _camera.transform.position = position;
    }
}