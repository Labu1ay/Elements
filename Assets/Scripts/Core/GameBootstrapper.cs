using Elements.Core.Services.GlobalServices;
using UnityEngine;
using Zenject;

namespace Elements.Core
{
    public class GameBootstrapper : MonoBehaviour
    {
        [Inject] private readonly ISceneLoaderService _sceneLoaderService;

        private void Start()
        {
            Application.targetFrameRate = 120;
            _sceneLoaderService.Load(Constants.GAME_SCENE_NAME);
        }
    }
}