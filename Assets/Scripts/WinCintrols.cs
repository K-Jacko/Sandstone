// GENERATED AUTOMATICALLY FROM 'Assets/WinCintrols.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @WinCintrols : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @WinCintrols()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""WinCintrols"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""8cd9ec0e-ad3a-4921-81e8-2155c11a1088"",
            ""actions"": [
                {
                    ""name"": ""Generate"",
                    ""type"": ""Button"",
                    ""id"": ""2b493b21-091d-42f6-9c0e-6fb3ad8aa662"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""26b48db9-56ba-4d11-bf1c-c959c7b00341"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Generate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Generate = m_Player.FindAction("Generate", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Generate;
    public struct PlayerActions
    {
        private @WinCintrols m_Wrapper;
        public PlayerActions(@WinCintrols wrapper) { m_Wrapper = wrapper; }
        public InputAction @Generate => m_Wrapper.m_Player_Generate;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Generate.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGenerate;
                @Generate.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGenerate;
                @Generate.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGenerate;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Generate.started += instance.OnGenerate;
                @Generate.performed += instance.OnGenerate;
                @Generate.canceled += instance.OnGenerate;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnGenerate(InputAction.CallbackContext context);
    }
}
