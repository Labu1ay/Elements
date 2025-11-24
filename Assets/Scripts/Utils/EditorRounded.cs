using UnityEngine;

namespace Elements.Utils
{
    [ExecuteInEditMode]
    public class EditorRounded : MonoBehaviour
    {
        private const float STEP_ROUNDED = 1f;
        private void OnEnable() => UnityEditor.EditorApplication.update += OnUpdate;
        private void OnDisable() => UnityEditor.EditorApplication.update -= OnUpdate;

        private void OnUpdate() {
            if (!Application.isPlaying && transform.hasChanged) {
                RoundUp();
                transform.hasChanged = false;
            }
        }

        private void RoundUp() {
            Vector3 roundedPosition = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0f);
            transform.position = roundedPosition;
        }
    
        private void OnDrawGizmos() {
            Gizmos.DrawWireCube(transform.position, new Vector3(STEP_ROUNDED, STEP_ROUNDED, 0f));
        }
    }
}