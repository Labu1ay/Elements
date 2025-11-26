using UnityEngine;
using Zenject;

namespace Elements.Configs.GameConfigs
{
    public class GameConfigsInstaller : MonoInstaller
    {
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private ElementsPathConfig _elementsPathConfig;
        
        public override void InstallBindings()
        {
            Container.Bind<LevelConfig>().FromInstance(_levelConfig).AsSingle();
            Container.Bind<ElementsPathConfig>().FromInstance(_elementsPathConfig).AsSingle();
        }
    }
}