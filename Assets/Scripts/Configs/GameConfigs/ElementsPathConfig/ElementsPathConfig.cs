using System.Collections.Generic;
using Elements.Game.Elements;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Elements.Configs.GameConfigs
{
    [CreateAssetMenu(fileName = "ElementsPathConfig", menuName = "configs/ElementsPathConfig", order = 0)]
    public class ElementsPathConfig : SerializedScriptableObject {

        [OdinSerialize] Dictionary<ElementType, string> _paths = new Dictionary<ElementType, string>();

        public string GetElementPath(ElementType type) {
            return _paths.ContainsKey(type) ? _paths[type] : null;
        }
    }
}