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
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
                float turbo = turboValue * sharedValues.turboMultiplier / 100;
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
    }

    [System.Serializable]
    public class PlayerSharedValues
    {
        [BoxGroup("Player Values")]
        [SerializeField, Min(0)] private float characterHeight = 2;

        [Header("Movement Values")] [HorizontalLine(color:EColor.Yellow)] 
        [SerializeField] private float velocity = 10f;
        [SerializeField] private float jumpFactor = 1f;
        [SerializeField] [Min(0)] private float maxJumpForce = 20;
        [SerializeField] [UnityEngine.Range(1,7)] private float maxAddedVelocity = 5;
        [SerializeField] [UnityEngine.Range(10, 50)] private float maxVelocity = 30;
        [SerializeField] private float rotationFactor = 3;

        private float addedVelocity = 0;
        private float turbo = 0;

        public float turboMultiplier = 1;
    
        public string actualState { get; set; }
        public Player player { get; set; }

        public int playerCode { get; set; }

        [ExposedProperty("Etherium")]
        public bool Etherium { get; set; }
        
        [ExposedProperty("Added Jump")]
        public float AddedJump { get; set; }

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

        [MovimentationValue] [ExposedProperty("Added Velocity")]
        public float AddedVelocity
        {
            get
            {
                return addedVelocity;
            }
            set
            {
                addedVelocity = Mathf.Clamp(value, -maxAddedVelocity, maxAddedVelocity);

                if (addedVelocity > 5 && player.GetPlayerState().GetType() != typeof(Dead))
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
                return Mathf.Clamp((AddedJump+jumpFactor) * RealVelocity*0.75f, 1, maxJumpForce);
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

        [MovimentationValue] [ExposedProperty("Real Velocity")]
        public float RealVelocity
        {
            get
            {
                return Mathf.Clamp(velocity + AddedVelocity, 0, maxVelocity);
            }
        }
    }
    
}