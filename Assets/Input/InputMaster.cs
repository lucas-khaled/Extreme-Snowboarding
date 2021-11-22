// GENERATED AUTOMATICALLY FROM 'Assets/Input/InputMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMaster : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""Grounded"",
            ""id"": ""9ec26e0b-9846-49fa-a64f-76875a4c8c85"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""07a7c89b-92f9-48b2-aab6-759ca7a67764"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Item"",
                    ""type"": ""Button"",
                    ""id"": ""12988bc7-162f-4975-a958-1a6e7e8b9b54"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Boost"",
                    ""type"": ""Button"",
                    ""id"": ""84dd092a-aa5b-444d-a774-f20bec4374bd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9a52b1dd-8c5a-43dc-a5e6-c0538102f016"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1af387d5-13c7-43ed-8fec-a532029be10b"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5fd5099e-f494-431e-b848-02be12763ae5"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""Item"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""051cd59d-595e-453d-8347-6802151e21f1"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""Item"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa4f4d42-b0ef-4bdc-b98c-5a5d7546cc75"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a87354cf-7f16-481a-95ab-906cda96aed1"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Jumping"",
            ""id"": ""aecaeb17-bb21-48ce-bce3-2dcb859f24a1"",
            ""actions"": [
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Button"",
                    ""id"": ""42985dc0-4076-4962-a259-35e7b780e4aa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold(duration=0.1)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Player1Rotate"",
                    ""id"": ""9b345cbf-6ea7-4883-8605-59c6dc4b9c5f"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""acecda28-8ee5-4240-81dd-4bff4d96c54d"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""e3c9ca90-58ac-4341-aca0-9ebbe457cfcf"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Player1RotateArrow"",
                    ""id"": ""e0c6e986-6f85-4af2-9e0e-f793e1e86f59"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""56aeb2d7-da25-4b8d-bc44-27787afbaf40"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""75a66b49-6e86-4f03-bd86-316fe8d106e9"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Pause"",
            ""id"": ""ad109b84-0809-4b66-a2cd-6cbc1f32b889"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""f8bcf21d-58a5-403c-8183-23ae4887032b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a4a95dec-9a8b-4f24-9f21-df37c40ed3d2"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Player1"",
            ""bindingGroup"": ""Player1"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Grounded
        m_Grounded = asset.FindActionMap("Grounded", throwIfNotFound: true);
        m_Grounded_Jump = m_Grounded.FindAction("Jump", throwIfNotFound: true);
        m_Grounded_Item = m_Grounded.FindAction("Item", throwIfNotFound: true);
        m_Grounded_Boost = m_Grounded.FindAction("Boost", throwIfNotFound: true);
        // Jumping
        m_Jumping = asset.FindActionMap("Jumping", throwIfNotFound: true);
        m_Jumping_Rotate = m_Jumping.FindAction("Rotate", throwIfNotFound: true);
        // Pause
        m_Pause = asset.FindActionMap("Pause", throwIfNotFound: true);
        m_Pause_Pause = m_Pause.FindAction("Pause", throwIfNotFound: true);
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

    // Grounded
    private readonly InputActionMap m_Grounded;
    private IGroundedActions m_GroundedActionsCallbackInterface;
    private readonly InputAction m_Grounded_Jump;
    private readonly InputAction m_Grounded_Item;
    private readonly InputAction m_Grounded_Boost;
    public struct GroundedActions
    {
        private @InputMaster m_Wrapper;
        public GroundedActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_Grounded_Jump;
        public InputAction @Item => m_Wrapper.m_Grounded_Item;
        public InputAction @Boost => m_Wrapper.m_Grounded_Boost;
        public InputActionMap Get() { return m_Wrapper.m_Grounded; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GroundedActions set) { return set.Get(); }
        public void SetCallbacks(IGroundedActions instance)
        {
            if (m_Wrapper.m_GroundedActionsCallbackInterface != null)
            {
                @Jump.started -= m_Wrapper.m_GroundedActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_GroundedActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_GroundedActionsCallbackInterface.OnJump;
                @Item.started -= m_Wrapper.m_GroundedActionsCallbackInterface.OnItem;
                @Item.performed -= m_Wrapper.m_GroundedActionsCallbackInterface.OnItem;
                @Item.canceled -= m_Wrapper.m_GroundedActionsCallbackInterface.OnItem;
                @Boost.started -= m_Wrapper.m_GroundedActionsCallbackInterface.OnBoost;
                @Boost.performed -= m_Wrapper.m_GroundedActionsCallbackInterface.OnBoost;
                @Boost.canceled -= m_Wrapper.m_GroundedActionsCallbackInterface.OnBoost;
            }
            m_Wrapper.m_GroundedActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Item.started += instance.OnItem;
                @Item.performed += instance.OnItem;
                @Item.canceled += instance.OnItem;
                @Boost.started += instance.OnBoost;
                @Boost.performed += instance.OnBoost;
                @Boost.canceled += instance.OnBoost;
            }
        }
    }
    public GroundedActions @Grounded => new GroundedActions(this);

    // Jumping
    private readonly InputActionMap m_Jumping;
    private IJumpingActions m_JumpingActionsCallbackInterface;
    private readonly InputAction m_Jumping_Rotate;
    public struct JumpingActions
    {
        private @InputMaster m_Wrapper;
        public JumpingActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Rotate => m_Wrapper.m_Jumping_Rotate;
        public InputActionMap Get() { return m_Wrapper.m_Jumping; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(JumpingActions set) { return set.Get(); }
        public void SetCallbacks(IJumpingActions instance)
        {
            if (m_Wrapper.m_JumpingActionsCallbackInterface != null)
            {
                @Rotate.started -= m_Wrapper.m_JumpingActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_JumpingActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_JumpingActionsCallbackInterface.OnRotate;
            }
            m_Wrapper.m_JumpingActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
            }
        }
    }
    public JumpingActions @Jumping => new JumpingActions(this);

    // Pause
    private readonly InputActionMap m_Pause;
    private IPauseActions m_PauseActionsCallbackInterface;
    private readonly InputAction m_Pause_Pause;
    public struct PauseActions
    {
        private @InputMaster m_Wrapper;
        public PauseActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pause => m_Wrapper.m_Pause_Pause;
        public InputActionMap Get() { return m_Wrapper.m_Pause; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PauseActions set) { return set.Get(); }
        public void SetCallbacks(IPauseActions instance)
        {
            if (m_Wrapper.m_PauseActionsCallbackInterface != null)
            {
                @Pause.started -= m_Wrapper.m_PauseActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_PauseActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_PauseActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_PauseActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public PauseActions @Pause => new PauseActions(this);
    private int m_Player1SchemeIndex = -1;
    public InputControlScheme Player1Scheme
    {
        get
        {
            if (m_Player1SchemeIndex == -1) m_Player1SchemeIndex = asset.FindControlSchemeIndex("Player1");
            return asset.controlSchemes[m_Player1SchemeIndex];
        }
    }
    public interface IGroundedActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnItem(InputAction.CallbackContext context);
        void OnBoost(InputAction.CallbackContext context);
    }
    public interface IJumpingActions
    {
        void OnRotate(InputAction.CallbackContext context);
    }
    public interface IPauseActions
    {
        void OnPause(InputAction.CallbackContext context);
    }
}
