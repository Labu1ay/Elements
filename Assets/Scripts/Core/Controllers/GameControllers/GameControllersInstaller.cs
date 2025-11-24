using Zenject;

namespace Elements.Core.Controllers.GameControllers
{
    public class GameControllersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ElementMovementController>().AsSingle();
        }
    }
}