using System.Collections.Generic;
using UnityEngine;

namespace Elements.Configs.GameConfigs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "configs/LevelConfig", order = 0)]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private List<LevelData> _levels;

        public LevelData GetLevel(int levelNumber)
        {
            var index = levelNumber % _levels.Count;
            return _levels[index];
        }
    }
}