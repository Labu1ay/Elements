using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elements.Configs.GameConfigs
{
    [CreateAssetMenu(fileName = "CameraSettingsConfig", menuName = "configs/CameraSettingsConfig", order = 0)]
    public class CameraSettingsConfig : ScriptableObject
    {
        [SerializeField] private List<CameraSettings> _cameraSettings;

        public CameraSettings GetCameraSettings(int widthCount)
            => _cameraSettings.FirstOrDefault(s => s.WidthCount == widthCount);
    }
}