using System.Collections;
using ExtremeSnowboarding.Script.EventSystem;
using ExtremeSnowboarding.Script.Attributes;
using ExtremeSnowboarding.Script.Controllers;
using ExtremeSnowboarding.Script.EstadosPlayer;
using ExtremeSnowboarding.Script.Items;
using ExtremeSnowboarding.Script.Items.Effects;
using ExtremeSnowboarding.Script.VFX;
using NaughtyAttributes;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using DG.Tweening;
using ExtremeSnowboarding.Multiplayer;
using UnityEditor;

namespace ExtremeSnowboarding.Script.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(PlayerInput))]
    public class Player : MonoBehaviour
    {
        [BoxGroup("References")]
        public PlayerInput playerInput;
        [SerializeField] [BoxGroup("References")]
        private Animator animator;
        [SerializeField] [BoxGroup("References")]
        private SkinnedMeshRenderer[] meshRenderers;

        [BoxGroup("Player Values")]
        [SerializeField] 
        private PlayerSharedValues sharedValues;
    
        [FormerlySerializedAs("playerVFXList")]
        [BoxGroup("ScriptableObjects")]
        [SerializeField] private PlayerFeedbacksGroup playerFeedbacksList;

        [FormerlySerializedAs("movimentationAudios")]
        [BoxGroup("ScriptableObjects")]
        [SerializeField] private PlayerMovimentationFeedbacks movimentationFeedbacks;

        /// <summary>
        /// Values that meant to be shared across the player states
        /// </summary>
        public PlayerSharedValues SharedValues
        {
            get
            {
                return sharedValues;
            }
            private set
            {
                sharedValues = value;
            }
        }

        public Item Coletavel { get; set; }

        public GameCamera playerCamera { get;  set; }

        public Vector3 groundedVelocity { get; set; }
        
        public PhotonView PhotonView { get; private set; }

        PlayerState playerState = new Stopped();
        
        private GameObject catastropheRef;

        

        public PlayerMovimentationFeedbacks GetMovimentationFeedbacks()
        {
            return movimentationFeedbacks;
        } 

        /// <summary>
        /// Get the parent object of player's mesh renderers
        /// </summary>
        /// <returns></returns>
        public GameObject GetMeshGameObject()
        {
            if (meshRenderers[0] != null)
                return meshRenderers[0].transform.parent.gameObject;

            return null;
        }

        /// <summary>
        /// Set the indicated meshes to player's renderers
        /// </summary>
        /// <param name="material">The material to be set into the renderers</param>
        /// <param name="meshes">The array of meshes to be set</param>
        public void SetPlayerMeshes(Material material, Mesh[] meshes)
        {
            for(int i = 0; i < meshRenderers.Length; i++)
            {
                if (meshes.Length <= i)
                {
                    Debug.Log("Passed Meshes Are lower than renderers.\nRenderers size is "+meshRenderers.Length+ ", meanwhile meshes size is "+meshes.Length);
                    break;
                }
                meshRenderers[i].material = material;
                meshRenderers[i].sharedMesh = meshes[i];
            }
        }

        /// <summary>
        /// Return the actual state of player
        /// </summary>
        /// <returns></returns>
        public PlayerState GetPlayerState()
        {
            return playerState;
        }

        /// <summary>
        /// Return the actual used FeedbackGroup
        /// </summary>
        /// <returns></returns>
        public PlayerFeedbacksGroup GetPlayerFeedbackList()
        {
            return playerFeedbacksList;
        }

        /// <summary>
        /// Set the player Material and Meshes
        /// </summary>
        /// <param name="firstColor">The primary color</param>
        /// <param name="secondColor">The secondary color</param>
        /// <param name="playerMeshes">The choosed meshes names</param>
        /// <param name="settings">The Instantiation settings</param>
        public void SetMaterialsAndMeshes(Color firstColor, Color secondColor, string[] playerMeshes, string playerOverrideName, MultiplayerInstantiationSettings settings)
        {
            Debug.Log("Player meshes names: "+playerMeshes[0]+" - "+playerMeshes[1]+" - "+playerMeshes[2]);
            Material material = new Material(settings.playerShader);

            material.SetColor("_PrimaryColor", firstColor);
            material.SetColor("_SecondaryColor", secondColor);

            material.SetTexture("_Color1Mask", settings.playerMask01);
            material.SetTexture("_Color2Mask", settings.playerMask02);
            
            SetPlayerMeshes(material, settings.GetMeshesByNames(playerMeshes));
            SetOverrideController(settings.GetOverriderByName(playerOverrideName));
        }

        public void SetOverrideController(AnimatorOverrideController animatorOverriderController)
        {
            AnimatorOverrider animOverrider = animator.GetComponent<AnimatorOverrider>();
            animOverrider.SetAnimations(animatorOverriderController);
        }
        

        private void Awake()
        {
            PhotonView = GetComponent<PhotonView>();
            sharedValues.player = this; //setting the player reference to the shared values

            if (PhotonView.IsMine)
            {
                playerFeedbacksList.StartFeedbacks(transform, sharedValues.playerCode); 
                movimentationFeedbacks.StartFeedbacks(transform);
                InputSubcribing();
            }
            else
            {
                Destroy(GetComponent<AudioListener>());
            }
            
        }

        private void Start()
        {
            if(!PhotonView.IsMine) return;
            
            playerState.StateStart(this);
            catastropheRef = null;
            InvokeRepeating("CheckTurbo", 0.3f, 0.08f);
        }

        private void FixedUpdate()
        {
            if(!PhotonView.IsMine) return;
            
            playerState.StateUpdate();
        }

        #region Inputs

        /// <summary>
        /// Subscribes into the inputs callbacks
        /// </summary>
        private void InputSubcribing()
        {
            if(!PhotonView.IsMine) return;
            
            playerInput.currentActionMap.FindAction("Item").started += ActivateItem;
            playerInput.currentActionMap.FindAction("Boost").started += ActivateBoost;  
            playerInput.currentActionMap.FindAction("BoostCheat").started += BoostCheat;
        }

        // The CheatCode method to rise your boost to maximum, it listens to "BoostCheat" input
        private void BoostCheat(InputAction.CallbackContext context)
        {
            if(context.started)
                AddTurbo(100);
        }
    
        // Method that will activate the holded item, empty the item slot and invoke an delegate. It listens to "Item" input
        private void ActivateItem(InputAction.CallbackContext context)
        {
            if (context.started && Coletavel != null && !sharedValues.inputLocked)
            {
                Coletavel.Activate(this);
                Coletavel = null;

                if (PlayerGeneralEvents.onItemUsed != null)
                {
                    PlayerGeneralEvents.onItemUsed.Invoke(this, null);
                }
            }
        }
    
        // Method witch will activate boost in case the boost value is complete. It listens to "Boost" input
        private void ActivateBoost(InputAction.CallbackContext context)
        {
            if (context.started && !sharedValues.inputLocked)
            {
                float forcaBoostRelative = 7 / (1 / sharedValues.Turbo);

                Effect boostEffect = new Effect("AddedAcceleration", (Mathf.RoundToInt(forcaBoostRelative) ^ 2) / 4f, (Mathf.RoundToInt(forcaBoostRelative) ^ 2) / 2, EffectMode.ADD, this);
                boostEffect.StartEffect(this);
                playerFeedbacksList.GetFeedbackByName("Boost", sharedValues.playerCode).StartFeedback();
                //GetComponent<Rigidbody>().velocity += sharedValues.MaxVelocity * (sharedValues.Turbo) * 0.5f * transform.right;
                AddTurbo(-sharedValues.Turbo);
            }
        }

        #endregion
    
        /// <summary>
        /// This method will check the catastrophe distance and add it accordingly to the turbo value
        /// </summary>   
        private void CheckTurbo()
        {
            if (catastropheRef != null && playerState.GetType() != typeof(Dead))
            {
                float distance = Vector3.Distance(this.gameObject.transform.position, catastropheRef.transform.position);
                float distanceModerator = Mathf.Clamp(Mathf.Sqrt(distance), 0.02f, 100f);
                AddTurbo(1 / distanceModerator);
            }
            else if (CorridaController.instance != null && CorridaController.instance.catastrophe != null)
                catastropheRef = CorridaController.instance.catastrophe;
        }

        /// <summary>
        /// Adds a value to turbo value. It invokes a delegate on PlayerGeneralEvents
        /// </summary>
        /// <param name="turboValue"> Before being added,this value will be divided by 100 in case it is greater than 0 </param>
        public void AddTurbo(float turboValue)
        {
            if (turboValue > 0)
            {
                float turbo = turboValue / 100;
                sharedValues.Turbo += turbo;
            }
            else
            {
                sharedValues.Turbo += turboValue;
            }
            
            PlayerGeneralEvents.onTurboChange?.Invoke(this, sharedValues.Turbo);
        }

        /// <summary>
        /// Changes the actual Player State. It will call "StateEnd" on the older state and "StateStart" on the new state.
        /// </summary>
        /// <param name="newState"> The new actual state </param>
        public void ChangeState(PlayerState newState)
        {
            if(!PhotonView.IsMine) return;
            
            playerState.StateEnd();

            playerState = newState;
            playerState.StateStart(this);
        }

        /// <summary>
        /// Sets the item to player's item slot.
        /// </summary>
        /// <param name="item">The Item to be set</param>
        public void SetItem(Item item)
        {
            Coletavel = item;
        }

        /// <summary>
        /// Method to allow non MonoBehaviours scripts to start Coroutines.
        /// </summary>
        /// <param name="coroutine">Coroutine to be started</param>
        public void StartStateCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }

        private void CallSetOnAnimator(string variable, bool value)
        {
            if(animator != null)
                animator.SetBool(variable, value);
        }

        [PunRPC]
        private void SetOnAnimator_RPC(string variable, bool value)
        {
            CallSetOnAnimator(variable, value);
        }

        /// <summary>
        /// Method to allow other scripts to set bool values on player's animator.
        /// </summary>
        /// <param name="variable">The animator variable name</param>
        /// <param name="value">Value to pass to animator variable</param>
        public void SetOnAnimator(string variable, bool value)
        {
            PhotonView.RPC("SetOnAnimator_RPC", RpcTarget.All, variable, value);
        }


        /// <summary>
        /// Method to allow other scripts to set triggers on player's animator.
        /// </summary>
        /// <param name="variable">The animator variable name</param>
        public void SetTriggerOnAnimator(string variable)
        {
            if (animator != null)
                animator.SetTrigger(variable);
        }

        /// <summary>
        /// Method to change the player animation instantly
        /// </summary>
        /// <param name="possibleAnimations">State names that can be randomized</param>
        /// <param name="crossFadeLength"> Length of the crossfade </param>
        /// <param name="valueSetedOnAnimator">The animator variable name</param>
        /// <param name="value">Value to pass to animator variable</param>
        public void ChangeAnimationTo(string[] possibleAnimations,  string valueSetedOnAnimator = null, bool value = true, float crossFadeLength = 0.15f)
        {
            PhotonView.RPC("ChangeAnimationTo_RPC", RpcTarget.All, 
                SerializeUtilities.StringArray2Byte(possibleAnimations), 
                valueSetedOnAnimator, value, crossFadeLength);
        }

        [PunRPC]
        private void ChangeAnimationTo_RPC(byte[] possibleAnimations, string valueSetedOnAnimator, bool value,
            float crossFadeLength)
        {
            ChangeAnimationTo(SerializeUtilities.Byte2StringArray(possibleAnimations), crossFadeLength);
            if(valueSetedOnAnimator != null)
                CallSetOnAnimator(valueSetedOnAnimator, value);
        }
        

        /// <summary>
        /// Method to change the player animation instantly
        /// </summary>
        /// <param name="possibleAnimations">State names that can be randomized</param>
        /// <param name="crossFadeLength"> Length of the crossfade </param>
        private void ChangeAnimationTo(string[] possibleAnimations, float crossFadeLength = 0.15f)
        {
            string animationChoosen = possibleAnimations[Random.Range(0, possibleAnimations.Length)];

            animator.CrossFade(animationChoosen, crossFadeLength);
        }

        //Detects player's collision and pass it to the actual state
        private void OnCollisionEnter(Collision collision)
        {
            if (PhotonView.IsMine)
            {
                playerState.OnCollisionEnter(collision);
                Debug.Log("mine");
            }
            else
                Debug.Log("not mine");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position + (transform.up * sharedValues.CharacterHeight / 2), transform.position + (-transform.up * sharedValues.CharacterHeight / 2)); //Drawing character height gizmo
        }
    }

    [System.Serializable]
    public class PlayerSharedValues
    {
        [BoxGroup("Player Values")]
        [SerializeField, Min(0)] private float characterHeight = 2;
        [SerializeField, UnityEngine.Range(1f,3f)] private float mortalAddVelocityRate = 2; 
        [FormerlySerializedAs("turboMultiplier")] [SerializeField] private float turboMortalMultiplier = 20;
        
        [Header("Movement Values")] [HorizontalLine(color:EColor.Yellow)] 
        
        [FormerlySerializedAs("velocity")] [SerializeField] private float acceleration = 1;
        [SerializeField] private float jumpFactor = 1f;
        [SerializeField] [Min(0)] private float maxJumpForce = 20;
        [SerializeField] [UnityEngine.Range(0f, 1f)] private float velocityOverJumpRate = 0.5f;
        [FormerlySerializedAs("maxAddedVelocity")] [SerializeField] [UnityEngine.Range(1f,20f)] private float maxAddedAcceleration = 5;
        [SerializeField] [UnityEngine.Range(10f, 50f)] private float maxVelocity = 30;
        [SerializeField] private float rotationFactor = 3;

        private float addedAcceleration = 0;
        private float turbo = 0;
        
        public Vector2 LastGroundedNormal { get; set; }
        
        /// <summary>
        /// String that contains the actual player state name.
        /// </summary>
        public string actualState { get; set; }

        /// <summary>
        /// Bool that locks the player jump is false.
        /// </summary>
        public bool canJump { get; set; }
        
        /// <summary>
        /// The player that owns this Shared Values.
        /// </summary>
        public Player player { get; set; }

        /// <summary>
        /// Player classification, being 1 the first.
        /// </summary>
        public int qualification { get; set; }

        /// <summary>
        /// The own player code. If it is the player 1, so the value is 1. An it goes on...
        /// </summary>
        public int playerCode => PhotonNetwork.LocalPlayer.ActorNumber;

        /// <summary>
        /// This property is true if the player cannot move. Otherwise is false.
        /// </summary>
        [ExposedProperty("Stun")]
        public bool isStun { get; set; }

        /// <summary>
        /// Sets if the player is etherium an can't be hitted by obstacles or FuckFriends.
        /// </summary>
        [ExposedProperty("Etherium")]
        public bool Etherium { get; set; }
        
        /// <summary>
        /// The added value by Items to the final jump value.
        /// </summary>
        [ExposedProperty("Added Jump")]
        public float AddedJump { get; set; }

        /// <summary>
        /// This property is true if the player can't use inputs.
        /// </summary>
        [ExposedProperty("Input lock")]
        public bool inputLocked { get; set; }

        /// <summary>
        /// It returns the actual acceleration value.
        /// </summary>
        public float Acceleration => acceleration;
        
        /// <summary>
        /// It returns the rate of each mortal addition in acceleration.
        /// </summary>
        public float MortalAddVelocityRate => mortalAddVelocityRate;
        
        /// <summary>
        /// It returns the rate of each mortal addition in turbo.
        /// </summary>
        public float TurboMortalMultiplier => turboMortalMultiplier;
        
        /// <summary>
        /// It returns player's maximum velocity at movement
        /// </summary>
        [MovimentationValue]
        public float MaxVelocity => maxVelocity + AddedAcceleration; // The maximum velocity will grow depending on addedAcceleration
        
        /// <summary>
        /// It Returns the real acceleration to be applied at movement.
        /// </summary>
        [MovimentationValue]
        public float RealAcceleration => acceleration + AddedAcceleration;

        /// <summary>
        /// Returns the final Force to be applied at jump.
        /// </summary>
        [MovimentationValue]
        public float JumpForce => Mathf.Clamp((AddedJump+jumpFactor) + (RealAcceleration*velocityOverJumpRate*0.2f), 1, maxJumpForce);
        // Combines the addedJump with the jump factor and adds it with the realVelocity multiplied by the rate of velocity over jump. It also clamps it between 3 and maxJumpForce.

        
        /// <summary>
        /// Property to control the rotation Factor
        /// </summary>
        [ExposedProperty("Rotation Factor")]
        public float RotationFactor
        {
            get
            {
                return rotationFactor;
            }
            private set
            {
                rotationFactor = value;
            }
        }

        /// <summary>
        /// Property to control turbo value. It will always clamp the value between 0 and 1 inclusive.
        /// </summary>
        [MovimentationValue] [ExposedProperty("Turbo")]
        public float Turbo 
        { 
            get 
            {
                return turbo;
            }
            set
            {
                turbo = Mathf.Clamp(value, 0f, 1f);
            }
        }

        /// <summary>
        /// Property to control the AddedAcceleration value.
        /// </summary>
        [MovimentationValue] [ExposedProperty("Added Acceleration")]
        public float AddedAcceleration
        {
            get
            {
                return addedAcceleration;
            }
            set
            {
                addedAcceleration = Mathf.Clamp(value, -maxAddedAcceleration, maxAddedAcceleration); //It will clamp between -maxAddedAcceleration and maxAddedAcceleration.

                if (addedAcceleration > 5 && player.GetPlayerState().GetType() != typeof(Dead))
                {
                    player.SetOnAnimator("highSpeed", true);
                    player.GetPlayerFeedbackList().GetFeedbackByName("FastMovement", player.SharedValues.playerCode).StartFeedback();
                    
                    // In case the value of addedAcceleration is grater than 5 and the player is not dead, it will play and animation and a particle to indicate high velocity
                }
                else
                {
                    player.SetOnAnimator("highSpeed", false);
                    player.GetPlayerFeedbackList().GetFeedbackByName("FastMovement", player.SharedValues.playerCode).StopFeedback();
                    
                    // Otherwise, they will be turned off
                }          
            
            }
        }
        

        /// <summary>
        /// Property to control the character height value.
        /// </summary>
        [ExposedProperty("Character Height")]
        public float CharacterHeight
        {
            get
            {
                return characterHeight;
            }
            private set
            {
                characterHeight = value;
            }
        }

    }
    
}