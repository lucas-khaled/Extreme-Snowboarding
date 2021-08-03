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

        private Rigidbody rb;

        public override void StateEnd()
        {
            player.ChangeSkiingAudio(false);

            UnsubscribeOnInputEvents();
        
            player.GetPlayerVFXList().GetVFXByName("NeveEspalha", player.SharedValues.playerCode).StopParticle();
            player.GetPlayerVFXList().GetVFXByName("FastMovement", player.SharedValues.playerCode).LockParticle(true);
            player.StopAllCoroutines();
            player = null;

        }

        public override void StateStart(Player.Player player)
        {
            base.StateStart(player);

            SubscribeOnInputEvents();

            player.SharedValues.actualState = "Grounded";

            player.ChangeSkiingAudio(true);

            rb = player.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = false;


            player.StartStateCoroutine(BeEtherium());

            player.GetPlayerVFXList().GetVFXByName("NeveEspalha", player.SharedValues.playerCode).StartParticle();
            player.GetPlayerVFXList().GetVFXByName("FastMovement", player.SharedValues.playerCode).UnlockParticle();

            rb.velocity = player.groundedVelocity;
        }

        public override void StateUpdate()
        {
            MoveByRigidbody();
            StickPlayerOnGround();
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
            if (Physics.Raycast(player.transform.position, -player.transform.up, out hit, player.SharedValues.CharacterHeight, LayerMask.GetMask("Track")))
            {
                ClampPlayerRotationByGround(hit);
                ClampPlayerPositionOnGround(hit);
                player.SharedValues.LastGroundedNormal = hit.normal.normalized;
            }
            else
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
            if((context.started || context.performed) && timeOnGround>=timeToJump && !player.SharedValues.isStun)
                player.ChangeState(new Jumping()); 
        }
    
        #endregion

        #region CONSTRUCTORS

        public Grounded()
        {
            timeEtherium = 0;
            timeToJump = 0;
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
