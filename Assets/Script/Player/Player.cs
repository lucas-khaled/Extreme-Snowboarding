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

        public GameObject GetMeshGameObject()
        {
            if (meshRenderers[0] != null)
            return meshRenderers[0].transform.parent.gameObject;

            return null;
        }

        public void SetPlayerMeshes(Material material, Mesh[] meshes)
        {
            for(int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].material = material;
                meshRenderers[i].sharedMesh = meshes[i];
            }
        }

        public PlayerState GetPlayerState()
        {
            return playerState;
        }

        public PlayerVFXGroup GetPlayerVFXList()
        {
            return playerVFXList;
        }

        private void Awake()
        {
            sharedValues.player = this;
            playerVFXList.StartParticles(transform);
            InputSubcribing();
        }

        private void Start()
        {
            playerState.StateStart(this);
            startPoint = transform.position;
            catastropheRef = null;
        }

        private void FixedUpdate()
        {
            playerState.StateUpdate();
        }

        private void Update()
        {
            //InputInterpretation();
            CheckTurbo();
        }

        #region Inputs

        private void InputSubcribing()
        {
            playerInput.currentActionMap.FindAction("Item").started += ActivateItem;
            playerInput.currentActionMap.FindAction("Boost").started += ActivateBoost;
            playerInput.currentActionMap.FindAction("BoostCheat").started += BoostCheat;
        }

        private void BoostCheat(InputAction.CallbackContext context)
        {
            if(context.started)
                AddTurbo(100);
        }
    
        private void ActivateItem(InputAction.CallbackContext context)
        {
            if (context.started && Coletavel != null)
            {
                Coletavel.Activate(this);
                Coletavel = null;

                if (EventSystem.PlayerGeneralEvents.onFuckFriendChange != null)
                {
                    EventSystem.PlayerGeneralEvents.onFuckFriendChange.Invoke(this, null);
                }
            }
        }
    
        private void ActivateBoost(InputAction.CallbackContext context)
        {
            if (context.started && sharedValues.Turbo >= 0.95)
            {
                //float amount = Mathf.Clamp(turboStrengthVariation/ 2f, 0, 3);
                //float time = Mathf.Clamp(turboTimeVariation, 0, 2);
                Effect boostEffect = new Effect("AddedVelocity", 5f, 3f, EffectMode.ADD, this);
                boostEffect.StartEffect(this);
                AddTurbo(-sharedValues.Turbo);
            }
        }

        #endregion
    
    
        private void CheckTurbo()
        {
            if (catastropheRef != null)
            {
                float distance = Vector3.Distance(this.gameObject.transform.position, catastropheRef.transform.position);
                AddTurbo(1 / distance);
            }
            else if (CorridaController.instance.catastrophe != null)
                catastropheRef = CorridaController.instance.catastrophe;
        }

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

        void Restart()
        {
            SceneManager.LoadScene("MenuPrincipal");
        }

        public void ChangeState(PlayerState newState)
        {
            playerState.StateEnd();

            playerState = newState;
            playerState.StateStart(this);
        }

        public void GetItem(Item item)
        {
            Coletavel = item;
            //if ()
            //    PlayGotPowerUpAudio();
            //else
            //    PlayGotFuckFriendAudio();
        }

        public void StartStateCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }

        public void SetOnAnimator(string variable, bool value)
        {
            if(animator != null)
                animator.SetBool(variable, value);
        }

        public bool GetOnAnimator(string name)
        {
            return animator.GetBool(name);
        }
    
        private void OnCollisionEnter(Collision collision)
        {
            playerState.OnCollisionEnter(collision);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position + (Vector3.up * sharedValues.CharacterHeight / 2), transform.position + (Vector3.down * sharedValues.CharacterHeight / 2));
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

        
    
        public string actualState { get; set; }
        public Player player { get; set; }

        public int playerCode { get; set; }

        [ExposedProperty("Stun")]
        public bool isStun { get; set; }

        [ExposedProperty("Etherium")]
        public bool Etherium { get; set; }
        
        [ExposedProperty("Added Jump")]
        public float AddedJump { get; set; }

        public float Acceleration => acceleration;
        public float MortalAddVelocityRate => mortalAddVelocityRate;
        public float TurboMortalMultiplier => turboMortalMultiplier;

        [MovimentationValue] [ExposedProperty("Turbo")]
        public float Turbo 
        { 
            get 
            {
                return turbo;
            }
            set
            {
                if (value > 1)
                {
                    turbo = 1;
                }
                else if (value < 0)
                    turbo = 0;
                else
                    turbo = value;
            }
        }

        [MovimentationValue] [ExposedProperty("Added Acceleration")]
        public float AddedAcceleration
        {
            get
            {
                return addedAcceleration;
            }
            set
            {
                addedAcceleration = Mathf.Clamp(value, -maxAddedAcceleration, maxAddedAcceleration);

                if (addedAcceleration > 5 && player.GetPlayerState().GetType() != typeof(Dead))
                {
                    player.SetOnAnimator("highSpeed", true);
                    player.GetPlayerVFXList().GetVFXByName("FastMovement", player.SharedValues.playerCode).StartParticle();
                }
                else
                {
                    player.SetOnAnimator("highSpeed", false);
                    player.GetPlayerVFXList().GetVFXByName("FastMovement", player.SharedValues.playerCode).StopParticle();
                }          
            
            }
        }

    
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

        [MovimentationValue]
        public float JumpForce
        {
            get
            {
                return Mathf.Clamp((AddedJump+jumpFactor) + RealAcceleration*velocityOverJumpRate, 1, maxJumpForce);
            }
        }

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

        [MovimentationValue]
        public float RealAcceleration
        {
            get
            {
                return acceleration + AddedAcceleration;
            }
        }

        [MovimentationValue]
        public float MaxVelocity
        {
            get
            {
                return maxVelocity + AddedAcceleration;
            }
        }
    }
    
}