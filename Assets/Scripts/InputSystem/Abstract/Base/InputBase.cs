using System;
using UnityEngine.InputSystem;

namespace GJG.GJGInput
{
    public abstract class InputBaseNew : IDisposable
    {
        protected InputActionMap _inputActionMap;

        protected InputBaseNew(InputActionAsset inputActionAsset, string mapName)
        {
            _inputActionMap = inputActionAsset.FindActionMap(mapName);
            InputActionActive();
        }

        public virtual void Dispose()
        {
            InputActionDeActive();
        }


        protected virtual void InputActionActive()
        {
            _inputActionMap?.Enable();
        }

        protected virtual void InputActionDeActive()
        {
            _inputActionMap?.Disable();
        }
    }
}