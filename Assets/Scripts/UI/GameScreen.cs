using Elements.Core.Services.GameServices;
using Elements.Core.Services.GlobalServices;
using Sirenix.Utilities;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Elements.UI
{
    public class GameScreen : MonoBehaviour
    {
        private const string LEVEL_FORMAT = "Level {0}";
        
        [Inject] private readonly ILevelControlService _levelControlService;
        [Inject] private readonly ISceneLoaderService _sceneLoaderService;
        [Inject] private readonly IAudioService _audioService;
        
        [SerializeField] private Button[] _restartButton;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private GameObject _completePanel;

        private CompositeDisposable _disposables = new CompositeDisposable();

        private void Start()
        {
            _levelControlService.OnCompleted += OnCompleted;
            
            _restartButton.ForEach(b => b.OnClickAsObservable().Subscribe(_ =>
            {
                _audioService.PlaySound(Constants.CLICK_CLIP_KEY);
                _sceneLoaderService.Load(_sceneLoaderService.ActiveSceneName);
            }).AddTo(_disposables));

            _levelText.text = string.Format(LEVEL_FORMAT, _levelControlService.CurrentLevelIndex + 1);
        }

        private void OnCompleted()
        {
            _completePanel.SetActive(true);
        }

        private void OnDestroy()
        { 
            _levelControlService.OnCompleted -= OnCompleted;
            _disposables?.Clear();
        }
    }
}