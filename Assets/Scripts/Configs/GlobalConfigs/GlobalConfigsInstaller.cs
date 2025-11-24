using UnityEngine;
using Zenject;

namespace Elements.Configs.GlobalConfigs
{
    public class GlobalConfigsInstaller : MonoInstaller
    {
        [SerializeField] private AudioClipConfig _audioClipConfig;

        public override void InstallBindings()
        {
            Container.Bind<AudioClipConfig>().FromInstance(_audioClipConfig).AsSingle();
        }
    }
}