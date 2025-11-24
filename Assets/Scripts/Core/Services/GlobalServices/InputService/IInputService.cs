using System;

namespace Elements.Core.Services
{
    public interface IInputService
    {
        event Action<bool> IsTouchDown;
    }
}