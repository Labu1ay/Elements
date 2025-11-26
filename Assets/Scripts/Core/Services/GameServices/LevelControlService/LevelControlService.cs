using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Elements.Configs.GameConfigs;
using Elements.Core.Services.GameServices.Data;
using Elements.Core.Services.GlobalServices;
using Elements.Game.Elements;
using Elements.UI;
using UnityEngine;
using Zenject;

namespace Elements.Core.Services.GameServices
{
    public class LevelControlService : ILevelControlService, IInitializable, ILateDisposable
    {
        private const string LEVEL_INDEX_KEY = "CurrentLevelIndex";
        private const string LEVEL_ELEMENTS_KEY = "LevelElementsPosition";

        private readonly IAssetService _assetService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly LevelConfig _levelConfig;
        private readonly ElementsPathConfig _elementsPathConfig;
        private readonly DiContainer _diContainer;
        
        public int CurrentLevelIndex { get; private set; }
        public int LeftBorderX { get; private set; }
        public int RightBorderX { get; private set; }

        public event Action OnCompleted;

        private static bool _isFirstLoading;

        [Inject]
        public LevelControlService(
            IAssetService assetService,
            ISaveLoadService saveLoadService, 
            LevelConfig levelConfig,
            ElementsPathConfig elementsPathConfig,
            DiContainer diContainer)
        {
            _assetService = assetService;
            _saveLoadService = saveLoadService;
            _levelConfig = levelConfig;
            _elementsPathConfig = elementsPathConfig;
            _diContainer = diContainer;
        }

        public async void Initialize()
        {
            CurrentLevelIndex = _saveLoadService.Load<int>(LEVEL_INDEX_KEY);
            var levelData = _levelConfig.GetLevel(CurrentLevelIndex);
            
            InitCameraAndBorders(levelData);
            
            if (!_isFirstLoading && _saveLoadService.HasKey(LEVEL_ELEMENTS_KEY))
            {
                var data = _saveLoadService.Load<List<ElementSavingData>>(LEVEL_ELEMENTS_KEY);
                var content = new GameObject("Elements").transform;
                var curtain = _assetService.Instantiate<LoadingCurtain>(Constants.CURTAIN_PATH, _diContainer, content);
                
                foreach (var savingData in data)
                {
                    Element element =
                        _assetService.Instantiate<Element>(_elementsPathConfig.GetElementPath(savingData.Type), _diContainer, content);

                    await UniTask.WaitForEndOfFrame();
                    
                    element.transform.position = new Vector2(savingData.Position.Item1, savingData.Position.Item2);
                    element.InitRoundPosition();
                }
                
                curtain.Hide();
            }
            else
            {
                _assetService.Instantiate(levelData.LevelPath, _diContainer);
            }

            _assetService.UnloadUnusedAssets();
            _isFirstLoading = true;
        }

        private void InitCameraAndBorders(LevelData levelData)
        {
            var camera = Camera.main;
            var centerPosition = levelData.WidthCount % 2 == 0 ? 0.5f : 0f;
            
            camera.transform.position = new Vector3(centerPosition, 
                camera.transform.position.y , camera.transform.position.z);

            LeftBorderX = (int)Math.Floor(centerPosition - levelData.WidthCount / 2f);
            RightBorderX = (int)Math.Ceiling(centerPosition + levelData.WidthCount / 2f);
        }

        public void LevelComplete()
        {
            _saveLoadService.Save(LEVEL_INDEX_KEY, CurrentLevelIndex + 1);
            OnCompleted?.Invoke();
        }

        public void SaveElementsData(List<ElementSavingData> data) => 
            _saveLoadService.Save(LEVEL_ELEMENTS_KEY, data);

        public void LateDispose()
        {
            
        }
    }
}