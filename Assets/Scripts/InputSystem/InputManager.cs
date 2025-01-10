using UnityEngine.InputSystem;
using UnityEngine;

namespace GJG.GJGInput
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActionAsset;

        private InputTouchScreen _inputTouchScreen;

        private void Start()
        {
            _inputTouchScreen = new InputTouchScreen(inputActionAsset);

        }
        private void OnDestroy()
        {
            _inputTouchScreen?.Dispose();
        }
    }
}