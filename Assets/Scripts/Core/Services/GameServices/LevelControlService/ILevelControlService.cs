using System;
using System.Collections.Generic;
using Elements.Core.Services.GameServices.Data;

namespace Elements.Core.Services.GameServices
{
    public interface ILevelControlService
    {
        event Action OnCompleted;
        int CurrentLevelIndex { get; }
        int LeftBorderX { get; }
        int RightBorderX { get; }
        void LevelComplete();
        void SaveElementsData(List<ElementSavingData> data);
    }
}