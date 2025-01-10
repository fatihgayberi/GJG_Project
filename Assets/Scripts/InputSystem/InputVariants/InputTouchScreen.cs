using UnityEngine.InputSystem;
using System;

namespace GJG.GJGInput
{
    [Serializable]
    public class InputTouchScreen : InputBaseNew
    {
        private readonly InputAction _touchAction;

        public InputTouchScreen(InputActionAsset inputActionAsset) : base(inputActionAsset, "GridActions")
        {
            _touchAction = _inputActionMap.FindAction("Touch");

            _touchAction.performed += OnMouseClick;
        }

        public override void Dispose()
        {
            base.Dispose();

            _touchAction.performed -= OnMouseClick;
        }

        private void OnMouseClick(InputAction.CallbackContext context)
        {
            InputEvents.ScreenTouch?.Invoke(Mouse.current.position.ReadValue());
        }
    }
}