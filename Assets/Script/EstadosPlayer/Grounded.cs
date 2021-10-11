using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ExtremeSnowboarding.Script.EstadosPlayer
{
    [System.Serializable]
    public class Grounded : PlayerState
    {
        private float timeEtherium;

        private float timeOnGround;
        private float timeToJump;

        private float timeWait = 0;

        private Rigidbody rb;

        public override void StateEnd()
        {
            player.GetMovimentationFeedbacks()?.skiingFeedback?.StopFeedbacks();

            UnsubscribeOnInputEvents();
        
            player.GetPlayerFeedbackList().GetFeedbackByName("NeveEspalha", player.SharedValues.playerCode).StopFeedback();
            player.GetPlayerFeedbackList().GetFeedbackByName("FastMovement", player.SharedValues.playerCode).LockFeedback(true);
            player.StopAllCoroutines();
            player = null;

        }

        public override void StateStart(Player.Player player)
        {
            base.StateStart(player);

            SubscribeOnInputEvents();

            player.SharedValues.actualState = "Grounded";

            player.GetMovimentationFeedbacks().skiingFeedback?.PlayFeedbacks();

            rb = player.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = false;
            player.playerCamera.shouldFollowOnlyX = false;

            player.StartStateCoroutine(BeEtherium());

            player.GetPlayerFeedbackList().GetFeedbackByName("NeveEspalha", player.SharedValues.playerCode).StartFeedback();
            player.GetPlayerFeedbackList().GetFeedbackByName("FastMovement", player.SharedValues.playerCode).UnlockFeedback();

            rb.velocity = player.transform.right * player.groundedVelocity.magnitude;

            player.StartCoroutine(SkiingVariation());
        }

        private IEnumerator SkiingVariation()
        {
            yield return new WaitForSeconds(5f);
            while (true)
            {
                if (Random.Range(0, 100) > 70)
                {
                    Debug.Log("Made variation");
                    if (Random.Range(0, 100) < 50)
                        player.SetTriggerOnAnimator("esquiVariation1");
                    else
                        player.SetTriggerOnAnimator("esquiVariation2");
                }

                yield return new WaitForSeconds(20f);
            }
        }

        public override void StateUpdate()
        {
            if (timeWait < timeOnGround) 
            {
                MoveByRigidbody();
                StickPlayerOnGround();
            }
            timeOnGround += Time.deltaTime;
        }

        #region PRIVATE METHODS

        void MoveByRigidbody()
        {
            if (!player.SharedValues.isStun)
            {
                if (rb == null || rb.velocity == null)
                    return;

                if (rb.velocity.x < player.SharedValues.MaxVelocity)
                    rb.AddForce(player.SharedValues.Acceleration * Time.deltaTime * player.transform.right, ForceMode.VelocityChange);
                else if (rb.velocity.x > player.SharedValues.MaxVelocity)
                    rb.AddForce(-player.SharedValues.Acceleration * Time.deltaTime * player.transform.right, ForceMode.VelocityChange);

                if (rb.velocity.x < 0)
                    rb.AddForce(new Vector3(rb.velocity.x * -1, rb.velocity.y, rb.velocity.z), ForceMode.VelocityChange);


                player.groundedVelocity = rb.velocity;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }

        void StickPlayerOnGround()
        {
            RaycastHit hit;
            if (Physics.Raycast(player.transform.position, Vector3.down, out hit, player.SharedValues.CharacterHeight, LayerMask.GetMask("Track")))
            {
                ClampPlayerRotationByGround(hit);
                ClampPlayerPositionOnGround(hit);
                player.SharedValues.LastGroundedNormal = hit.normal.normalized;
            }
            else if (timeOnGround > timeToJump)
            {
                player.ChangeState(new Jumping(false));
            }
        }

        void ClampPlayerPositionOnGround(RaycastHit hit)
        {

            float xChange = rb.velocity.x - hit.normal.normalized.x * 2f;
            float yChange = rb.velocity.y - hit.normal.normalized.y * 2f;

            rb.velocity = new Vector3(xChange, yChange, rb.velocity.z);
        }

        void ClampPlayerRotationByGround(RaycastHit hit)
        {
            Quaternion newRotation = Quaternion.FromToRotation(player.transform.up, hit.normal) * player.transform.rotation;
            newRotation.y = newRotation.x = 0;
            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, newRotation, 100 * Time.deltaTime);
        }

        IEnumerator BeEtherium()
        {
            player.SharedValues.Etherium = true;
            yield return new WaitForSeconds(timeEtherium);
            player.SharedValues.Etherium = false;
        }
    
        void SubscribeOnInputEvents()
        {
            if (playerInput == null)
                playerInput = player.playerInput;
        
            playerInput.SwitchCurrentActionMap("Grounded");
            playerInput.currentActionMap.Enable();

            playerInput.currentActionMap.FindAction("Jump").started += Jump;
        }
    
        void UnsubscribeOnInputEvents()
        {
            if (playerInput == null)
                playerInput = player.playerInput;
        
            
            playerInput.currentActionMap.FindAction("Jump").started -= Jump;
            playerInput.currentActionMap.Disable();
        }

        void Jump(InputAction.CallbackContext context)
        {
            if((context.started || context.performed) && timeOnGround>=timeToJump && !player.SharedValues.isStun && !player.SharedValues.inputLocked)
                player.ChangeState(new Jumping()); 
        }
    
        #endregion

        #region CONSTRUCTORS

        public Grounded()
        {
            timeEtherium = 0;
            timeToJump = 0;
        }

        public Grounded(float time, bool waitForStartMovement)
        {
            timeWait = time;
        }


        public Grounded(float timeEtherium)
        {
            this.timeEtherium = timeEtherium;
            timeToJump = 0;
        }

        public Grounded(float timeEtherium, float timeToJump)
        {
            this.timeEtherium = timeEtherium;
            this.timeToJump = timeToJump;
        }

        #endregion
    }
}
