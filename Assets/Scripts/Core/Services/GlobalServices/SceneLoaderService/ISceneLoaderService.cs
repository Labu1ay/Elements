using System;
using Cysharp.Threading.Tasks;

namespace Elements.Core.Services.GlobalServices
{
    interface ISceneLoaderService {
        string ActiveSceneName { get; }
        UniTaskVoid Load(string name, Action callback = null);
    }
}