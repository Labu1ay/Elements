using Zenject;

namespace Elements.Core.Services.GameServices
{
    public class GameServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GridElementsService>().AsSingle();
        }
    }
}