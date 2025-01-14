using UnityEngine.InputSystem;
using System;

namespace GJG.GJGInput
{
    [Serializable]
    public class InputTouchScreen : InputBaseNew
    {
        private readonly InputAction _touchAction;
        private bool _canTouch;

        public InputTouchScreen(InputActionAsset inputActionAsset) : base(inputActionAsset, "GridActions")
        {
            _canTouch = true;

            _touchAction = _inputActionMap.FindAction("Touch");

            _touchAction.performed += OnMouseClick;

            InputEvents.ScreenTouchLock += OnScreenTouchLock;
            InputEvents.ScreenTouchUnLock += OnScreenTouchUnLock;
        }

        private void OnScreenTouchLock()
        {
            _canTouch = false;
        }

        private void OnScreenTouchUnLock()
        {
            _canTouch = true;
        }

        public override void Dispose()
        {
            base.Dispose();

            _touchAction.performed -= OnMouseClick;
        }

        private void OnMouseClick(InputAction.CallbackContext context)
        {
            // input verilemez mi?
            if (!_canTouch) return;

            InputEvents.ScreenTouch?.Invoke(Mouse.current.position.ReadValue());
        }
    }
}