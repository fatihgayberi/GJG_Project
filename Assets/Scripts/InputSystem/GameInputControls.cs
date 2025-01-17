//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/Scripts/InputSystem/GameInputControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @GameInputControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameInputControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInputControls"",
    ""maps"": [
        {
            ""name"": ""GridActions"",
            ""id"": ""a76a36b9-dd2a-4d9d-8914-8f865ffc8ca8"",
            ""actions"": [
                {
                    ""name"": ""Touch"",
                    ""type"": ""Button"",
                    ""id"": ""6d6e1501-d39d-4e98-85a3-0c7591d1abd9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""73887b3a-635a-4c5e-8547-b97658e9df8c"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Touch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GridActions
        m_GridActions = asset.FindActionMap("GridActions", throwIfNotFound: true);
        m_GridActions_Touch = m_GridActions.FindAction("Touch", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // GridActions
    private readonly InputActionMap m_GridActions;
    private List<IGridActionsActions> m_GridActionsActionsCallbackInterfaces = new List<IGridActionsActions>();
    private readonly InputAction m_GridActions_Touch;
    public struct GridActionsActions
    {
        private @GameInputControls m_Wrapper;
        public GridActionsActions(@GameInputControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Touch => m_Wrapper.m_GridActions_Touch;
        public InputActionMap Get() { return m_Wrapper.m_GridActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GridActionsActions set) { return set.Get(); }
        public void AddCallbacks(IGridActionsActions instance)
        {
            if (instance == null || m_Wrapper.m_GridActionsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GridActionsActionsCallbackInterfaces.Add(instance);
            @Touch.started += instance.OnTouch;
            @Touch.performed += instance.OnTouch;
            @Touch.canceled += instance.OnTouch;
        }

        private void UnregisterCallbacks(IGridActionsActions instance)
        {
            @Touch.started -= instance.OnTouch;
            @Touch.performed -= instance.OnTouch;
            @Touch.canceled -= instance.OnTouch;
        }

        public void RemoveCallbacks(IGridActionsActions instance)
        {
            if (m_Wrapper.m_GridActionsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGridActionsActions instance)
        {
            foreach (var item in m_Wrapper.m_GridActionsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GridActionsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GridActionsActions @GridActions => new GridActionsActions(this);
    public interface IGridActionsActions
    {
        void OnTouch(InputAction.CallbackContext context);
    }
}
