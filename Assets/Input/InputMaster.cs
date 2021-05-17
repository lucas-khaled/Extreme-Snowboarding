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
                },
                {
                    ""name"": ""BoostCheat"",
                    ""type"": ""Button"",
                    ""id"": ""7e80c2a3-a079-4868-84fa-50f4d4cf7cf2"",
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
                    ""id"": ""c3cd7a0f-f99b-4c2e-9227-4be8dc672013"",
                    ""path"": ""<Keyboard>/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player2"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8c06a9a1-4347-4549-a4ea-5bd856243dcb"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player3"",
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
                    ""groups"": ""Player4"",
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
                    ""id"": ""aa08dc0e-a526-4978-986c-b4b83c0ed431"",
                    ""path"": ""<Keyboard>/h"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player2"",
                    ""action"": ""Item"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cef3529d-f3ce-4a2e-82ff-37b48479b6de"",
                    ""path"": ""<Keyboard>/semicolon"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player3"",
                    ""action"": ""Item"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c9147477-de78-41ad-a47e-106b1b883fc4"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player4"",
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
                    ""id"": ""98851910-d6c4-4aed-bc32-39da897fb5bd"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player2"",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dc13d79d-18e9-4e54-aabd-ce953b05bd64"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player3"",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5e504ca3-1434-46a2-b518-abe9419c5798"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player4"",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3238733b-19e2-45e7-9ccb-0e81d10c3179"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""BoostCheat"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""10a71e5b-5843-42e3-9118-2645d9ad7d96"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player2"",
                    ""action"": ""BoostCheat"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dc75b1c5-60f7-48aa-8c12-282902a6c972"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player3"",
                    ""action"": ""BoostCheat"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""311e3525-cbd0-491f-b3fc-999402efe21e"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player4"",
                    ""action"": ""BoostCheat"",
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
                    ""name"": ""Player2Rotate"",
                    ""id"": ""b18bf82c-64ca-4d57-9024-55aead433509"",
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
                    ""id"": ""9666890d-ac3b-48a7-9b5e-985d038c9ec3"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player2"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""a10d29bc-e4c2-4fc5-a0bc-9ba181733168"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player2"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Player3Rotate"",
                    ""id"": ""db3a8ae6-0414-48f0-929b-1859981dfda6"",
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
                    ""id"": ""827079d3-c7bd-461b-8c94-b02eb86e753c"",
                    ""path"": ""<Keyboard>/quote"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player3"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""bcb63d83-7a4b-49aa-b8cb-ccd8d9cd9a1c"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player3"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Player4Rotate"",
                    ""id"": ""9600cdca-f10f-4306-ac5a-1999fe2bed6f"",
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
                    ""id"": ""3e11a8ac-114c-4399-b932-689d098728c6"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player4"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""96c2cfba-4161-4e62-aafb-2bf3c60b7f8f"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player4"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
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
        },
        {
            ""name"": ""Player2"",
            ""bindingGroup"": ""Player2"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Player3"",
            ""bindingGroup"": ""Player3"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Player4"",
            ""bindingGroup"": ""Player4"",
            ""devices"": []
        }
    ]
}");
        // Grounded
        m_Grounded = asset.FindActionMap("Grounded", throwIfNotFound: true);
        m_Grounded_Jump = m_Grounded.FindAction("Jump", throwIfNotFound: true);
        m_Grounded_Item = m_Grounded.FindAction("Item", throwIfNotFound: true);
        m_Grounded_Boost = m_Grounded.FindAction("Boost", throwIfNotFound: true);
        m_Grounded_BoostCheat = m_Grounded.FindAction("BoostCheat", throwIfNotFound: true);
        // Jumping
        m_Jumping = asset.FindActionMap("Jumping", throwIfNotFound: true);
        m_Jumping_Rotate = m_Jumping.FindAction("Rotate", throwIfNotFound: true);
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
    private readonly InputAction m_Grounded_BoostCheat;
    public struct GroundedActions
    {
        private @InputMaster m_Wrapper;
        public GroundedActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_Grounded_Jump;
        public InputAction @Item => m_Wrapper.m_Grounded_Item;
        public InputAction @Boost => m_Wrapper.m_Grounded_Boost;
        public InputAction @BoostCheat => m_Wrapper.m_Grounded_BoostCheat;
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
                @BoostCheat.started -= m_Wrapper.m_GroundedActionsCallbackInterface.OnBoostCheat;
                @BoostCheat.performed -= m_Wrapper.m_GroundedActionsCallbackInterface.OnBoostCheat;
                @BoostCheat.canceled -= m_Wrapper.m_GroundedActionsCallbackInterface.OnBoostCheat;
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
                @BoostCheat.started += instance.OnBoostCheat;
                @BoostCheat.performed += instance.OnBoostCheat;
                @BoostCheat.canceled += instance.OnBoostCheat;
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
    private int m_Player1SchemeIndex = -1;
    public InputControlScheme Player1Scheme
    {
        get
        {
            if (m_Player1SchemeIndex == -1) m_Player1SchemeIndex = asset.FindControlSchemeIndex("Player1");
            return asset.controlSchemes[m_Player1SchemeIndex];
        }
    }
    private int m_Player2SchemeIndex = -1;
    public InputControlScheme Player2Scheme
    {
        get
        {
            if (m_Player2SchemeIndex == -1) m_Player2SchemeIndex = asset.FindControlSchemeIndex("Player2");
            return asset.controlSchemes[m_Player2SchemeIndex];
        }
    }
    private int m_Player3SchemeIndex = -1;
    public InputControlScheme Player3Scheme
    {
        get
        {
            if (m_Player3SchemeIndex == -1) m_Player3SchemeIndex = asset.FindControlSchemeIndex("Player3");
            return asset.controlSchemes[m_Player3SchemeIndex];
        }
    }
    private int m_Player4SchemeIndex = -1;
    public InputControlScheme Player4Scheme
    {
        get
        {
            if (m_Player4SchemeIndex == -1) m_Player4SchemeIndex = asset.FindControlSchemeIndex("Player4");
            return asset.controlSchemes[m_Player4SchemeIndex];
        }
    }
    public interface IGroundedActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnItem(InputAction.CallbackContext context);
        void OnBoost(InputAction.CallbackContext context);
        void OnBoostCheat(InputAction.CallbackContext context);
    }
    public interface IJumpingActions
    {
        void OnRotate(InputAction.CallbackContext context);
    }
}
