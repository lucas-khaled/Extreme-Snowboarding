using ExtremeSnowboarding.Script.Items.Effects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ExtremeSnowboarding.Script.EstadosPlayer
{
    [System.Serializable]
    public class Jumping : PlayerState
    {
        Rigidbody rb;
        float airTime = 0;
        float howMuchRotation = 0f;
        float rotatingDirection = 0;
        bool isJumpingPressed;
        private bool canApplyForce;
        private int auxAudioFlip;
        
        public override void StateEnd()
        {
            rb.velocity = Vector3.zero;
            rb.useGravity = false;

            airTime = 0;
            player.SetOnAnimator("jumping", false);
            player.SetOnAnimator("trick", false);
        
            UnsubscribeOnInputEvents();
        }

        public override void StateStart(Player.Player player)
        {
            base.StateStart(player);
        
            SubscribeOnInputEvents();
        
            player.SharedValues.actualState = "Jumping";

            player.PlayJumpAudio();
            auxAudioFlip = 1;

            rb = player.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
        
            if(canApplyForce)
                rb.AddForce(player.SharedValues.JumpForce * 2.5f * Vector3.up , ForceMode.Impulse);
            else
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            }

            player.SetOnAnimator("jumping", true);
        }

        public override void StateUpdate()
        {
            airTime += Time.deltaTime;
            if(airTime >= 0.2f)
                RotatePlayer();

            if (isJumpingPressed)
                rotatingDirection = 1;
        }

        public override void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Track"))
            {       
                PlayerState newPlayerState;

                Vector3 realNormal = (collision.GetContact(0).normal.y < 0) ? -collision.GetContact(0).normal : collision.GetContact(0).normal;

                float groundAngle = Vector3.SignedAngle(Vector3.up, realNormal, Vector3.forward);

                float playerAngle = (player.transform.eulerAngles.z) % 360;
                float normalizedPlayerAngle = (playerAngle <= 180) ? playerAngle : playerAngle - 360;

                float angleDifference = Mathf.Abs(groundAngle - normalizedPlayerAngle);

                //Debug.Log("player Angle: " + normalizedPlayerAngle + "\n ground angle: " + groundAngle + "\n difference: " + angleDifference);

                if (angleDifference < 60f)
                {
                    int timeEtherium = Mathf.FloorToInt((airTime * 0.33f) % 3f);
                    player.PlayLandingAudio();
                    newPlayerState = new Grounded(timeEtherium, 0.3f);

                    ApplyAirEffects();
                }
                else
                {
                    float timeFall = 3.5f;
                    if (angleDifference > 120)
                    {
                        player.SetOnAnimator("hardFall", true);
                        player.PlayHardFallAudio();
                        timeFall = 4.5f;
                    }
                    else
                        player.PlayNormalFallAudio();

                    newPlayerState = new Fallen(timeFall);

                }

                player.ChangeState(newPlayerState);
            }
        }

        void RotatePlayer()
        {
            float rotation = player.SharedValues.RotationFactor * Time.deltaTime * 100 * rotatingDirection;
            player.transform.Rotate(Vector3.forward * rotation, Space.Self);
            howMuchRotation += rotation;
        }

        void StartRotatePlayer(InputAction.CallbackContext context)
        {
            rotatingDirection = context.ReadValue<float>();
        
            if (context.canceled)
            {
                player.SetOnAnimator("trick", false);
            }
            else
            {
                player.SetOnAnimator("trick", true);
                if (howMuchRotation > 360 * auxAudioFlip)
                {
                    player.PlayTrickAudio();
                    auxAudioFlip++;
                }
            }
        }

        void ApplyAirEffects()
        {
            int numOfMortals = 0;

            if (Mathf.Abs(howMuchRotation) > 180)
            {
                numOfMortals = Mathf.RoundToInt(Mathf.Abs(howMuchRotation / 360));
                Debug.Log("Mortal :" + numOfMortals + "x");

                float amount = player.SharedValues.MortalAddVelocityRate * numOfMortals;
                float time = Mathf.Clamp(airTime * numOfMortals, 0, 10);

                Effect mortalEffect = new Effect("AddedAcceleration",amount, time, EffectMode.ADD, player);
                mortalEffect.StartEffect(player);
            }
            

            player.AddTurbo(numOfMortals * player.SharedValues.TurboMortalMultiplier);
        }

        void SubscribeOnInputEvents()
        {
            if (playerInput == null)
                playerInput = player.playerInput;

            playerInput.SwitchCurrentActionMap("Jumping");

            playerInput.currentActionMap.Enable();
        
            playerInput.currentActionMap.FindAction("Rotate").performed += StartRotatePlayer;
            playerInput.currentActionMap.FindAction("Rotate").canceled += StartRotatePlayer;
        }
    
        void UnsubscribeOnInputEvents()
        {
            if (playerInput == null)
                playerInput = player.playerInput;

            playerInput.currentActionMap.FindAction("Rotate").performed -= StartRotatePlayer;
            playerInput.currentActionMap.FindAction("Rotate").canceled -= StartRotatePlayer;
        
            playerInput.currentActionMap.Enable();
        }

        #region CONSTRUCTORS

        public Jumping(bool canApplyForce = true)
        {
            this.canApplyForce = canApplyForce;
        }
        #endregion
    }
}
