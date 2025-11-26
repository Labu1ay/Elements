using Zenject;

namespace Elements.Core.Services.GameServices
{
    public class GameServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MatchElementsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<GridElementsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelControlService>().AsSingle();
        }
    }
}