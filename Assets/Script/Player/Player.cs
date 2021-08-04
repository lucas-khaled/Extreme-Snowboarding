using System.Collections;
using System.Collections.Generic;
using ExtremeSnowboarding.Script.EventSystem;
using ExtremeSnowboarding.Script.Attributes;
using ExtremeSnowboarding.Script.Controllers;
using ExtremeSnowboarding.Script.EstadosPlayer;
using ExtremeSnowboarding.Script.Items;
using ExtremeSnowboarding.Script.Items.Effects;
using ExtremeSnowboarding.Script.VFX;
using NaughtyAttributes;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

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

        #region Audio references
        [SerializeField] [BoxGroup("Audio clips")]
        private AudioClip normalFallClip;
        [SerializeField] [BoxGroup("Audio clips")]
        private AudioClip hardFallClip;
        [SerializeField] [BoxGroup("Audio clips")]
        private AudioClip landingClip;
        [SerializeField] [BoxGroup("Audio clips")]
        private AudioClip jumpingClip;
        [SerializeField] [BoxGroup("Audio clips")]
        private AudioClip gotPowerUpClip;
        [SerializeField] [BoxGroup("Audio clips")]
        private AudioClip gotFuckFriendClip;
        [SerializeField] [BoxGroup("Audio clips")]
        private AudioClip skiingClip;
        [SerializeField] [BoxGroup("Audio clips")]
        private AudioClip trickClip;
        [SerializeField] [BoxGroup("Audio clips")]
        private AudioClip victoryClip;
        [SerializeField] [BoxGroup("Audio clips")]
        private AudioClip lostClip;
        #endregion

        [BoxGroup("Player Values")]
        [SerializeField] 
        private PlayerSharedValues sharedValues;
    
        [BoxGroup("Player VFX's")]
        [SerializeField]
        private PlayerVFXGroup playerVFXList;

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

        public float groundedVelocity { get; set; }

        PlayerState playerState = new Grounded();

        private Player[] playerSpectating = new Player[4];
        private Vector3 startPoint;
        private GameObject catastropheRef;
        private AudioSource audioSource;
        private AudioSource audioSourceEffects;

        public void AddPlayerSpectating(Player playerSpectator)
        {
            for (int i = 0; i < 4; i++) 
            {
                if (playerSpectating[i] == null)
                {
                    playerSpectating[i] = playerSpectator;
                    return;
                }
            }
        }
        public Player[] GetPlayerSpectators()
        {
            return playerSpectating;
        }
        public void RemovePlayerSpectating(Player player)
        {
            for (int i = 0; i < 4; i++)
            {
                if (playerSpectating[i] == player)
                playerSpectating[i] = null;
            }
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
        /// Return the actual used VFXGroup
        /// </summary>
        /// <returns></returns>
        public PlayerVFXGroup GetPlayerVFXList()
        {
            return playerVFXList;
        }

        private void Awake()
        {
            sharedValues.player = this; //setting the player reference to the shared values
            playerVFXList.StartParticles(transform); 
            InputSubcribing();
        }

        private void Start()
        {
            playerState.StateStart(this);
            startPoint = transform.position;
            catastropheRef = null;
            InvokeRepeating("CheckTurbo", 0.3f, 0.08f);
        }

        private void FixedUpdate()
        {
            playerState.StateUpdate();
        }

        #region Inputs

        /// <summary>
        /// Subscribes into the inputs callbacks
        /// </summary>
        private void InputSubcribing()
        {
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
            if (context.started && Coletavel != null)
            {
                Coletavel.Activate(this);
                Coletavel = null;

                if (EventSystem.PlayerGeneralEvents.onItemUsed != null)
                {
                    EventSystem.PlayerGeneralEvents.onItemUsed.Invoke(this, null);
                }
            }
        }
    
        // Method witch will activate boost in case the boost value is complete. It listens to "Boost" input
        private void ActivateBoost(InputAction.CallbackContext context)
        {
            if (context.started && sharedValues.Turbo >= 0.95)
            {
                Effect boostEffect = new Effect("AddedAcceleration", 7f, 10f, EffectMode.ADD, this);
                boostEffect.StartEffect(this);
                GetComponent<Rigidbody>().velocity += sharedValues.MaxVelocity * 0.75f * transform.right;
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
                Debug.Log(distance);
                float distanceModerator = Mathf.Clamp(Mathf.Sqrt(distance), 0.02f, 100f);
                AddTurbo(1 / distanceModerator);
            }
            else if (CorridaController.instance.catastrophe != null)
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

            if (PlayerGeneralEvents.onTurboChange != null)
                PlayerGeneralEvents.onTurboChange.Invoke(this, sharedValues.Turbo);
        }

        /// <summary>
        /// Changes the actual Player State. It will call "StateEnd" on the older state and "StateStart" on the new state.
        /// </summary>
        /// <param name="newState"> The new actual state </param>
        public void ChangeState(PlayerState newState)
        {
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
            //if ()
            //    PlayGotPowerUpAudio();
            //else
            //    PlayGotFuckFriendAudio();
        }

        /// <summary>
        /// Method to allow non MonoBehaviours scripts to start Coroutines.
        /// </summary>
        /// <param name="coroutine">Coroutine to be started</param>
        public void StartStateCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }

        /// <summary>
        /// Method to allow other scripts to set bool values on player's animator.
        /// </summary>
        /// <param name="variable">The animator variable name</param>
        /// <param name="value">Value to pass to animator variable</param>
        public void SetOnAnimator(string variable, bool value)
        {
            if(animator != null)
                animator.SetBool(variable, value);
        }
        
    
        //Detects player's collision and pass it to the actual state
        private void OnCollisionEnter(Collision collision)
        {
            playerState.OnCollisionEnter(collision);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position + (transform.up * sharedValues.CharacterHeight / 2), transform.position + (-transform.up * sharedValues.CharacterHeight / 2)); //Drawing character height gizmo
        }

        #region Audio

        public void SetPlayerAudioSource(AudioSource audioSourceRef, AudioSource audioSourceEffectsRef)
        {
            audioSource = audioSourceRef;
            audioSourceEffects = audioSourceEffectsRef;
        }

        public void PlayNormalFallAudio()
        {
            if (normalFallClip != null)
            audioSourceEffects.PlayOneShot(normalFallClip);
        }
        public void PlayHardFallAudio()
        {
            if (hardFallClip != null)
                audioSourceEffects.PlayOneShot(hardFallClip);
        }
        public void PlayJumpAudio()
        {
            if (jumpingClip != null)
                audioSourceEffects.PlayOneShot(jumpingClip);
        }
        public void PlayLandingAudio()
        {
            if (landingClip!= null)
                audioSourceEffects.PlayOneShot(landingClip);
        }
        public void PlayGotPowerUpAudio()
        {
            if (gotPowerUpClip != null)
                audioSourceEffects.PlayOneShot(gotPowerUpClip);
        }
        public void PlayGotFuckFriendAudio()
        {
            if (gotFuckFriendClip != null)
                audioSourceEffects.PlayOneShot(gotFuckFriendClip);
        }
        public void PlayTrickAudio()
        {
            if (trickClip != null)
                audioSourceEffects.PlayOneShot(trickClip);
        }
        public void PlayVictoryAudio()
        {
            if (victoryClip != null)
                audioSourceEffects.PlayOneShot(victoryClip);
        }
        public void PlayLostAudio()
        {
            if (lostClip != null)
                audioSourceEffects.PlayOneShot(lostClip);
        }

        public void ChangeSkiingAudio(bool isSkiing)
        {
            if (skiingClip != null)
            {
                if (isSkiing)
                    audioSourceEffects.Play();
                else
                    audioSourceEffects.Stop();
            }
        }

        #endregion
    }

    [System.Serializable]
    public class PlayerSharedValues
    {
        [BoxGroup("Player Values")]
        [SerializeField, Min(0)] private float characterHeight = 2;
        [SerializeField, UnityEngine.Range(1f,3f)] private float mortalAddVelocityRate = 2; 
        [FormerlySerializedAs("turboMultiplier")] [SerializeField] private float turboMortalMultiplier = 1;
        
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
        /// The player that owns this Shared Values.
        /// </summary>
        public Player player { get; set; }

        /// <summary>
        /// The own player code. If it is the player 1, so the value is 1. An it goes on...
        /// </summary>
        public int playerCode { get; set; }

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
                    player.GetPlayerVFXList().GetVFXByName("FastMovement", player.SharedValues.playerCode).StartParticle();
                    
                    // In case the value of addedAcceleration is grater than 5 and the player is not dead, it will play and animation and a particle to indicate high velocity
                }
                else
                {
                    player.SetOnAnimator("highSpeed", false);
                    player.GetPlayerVFXList().GetVFXByName("FastMovement", player.SharedValues.playerCode).StopParticle();
                    
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