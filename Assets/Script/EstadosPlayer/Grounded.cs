using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ExtremeSnowboarding.Script.EstadosPlayer
{
    [System.Serializable]
    public class Grounded : PlayerState
    {
        float timeEtherium;

        float timeOnGround;
        float timeToJump;

        Rigidbody rb;

        /*public override void InterpretateInput(GameInput input)
    {
        Debug.Log(input.ToString());
        if (input == GameInput.UP && timeOnGround>=timeToJump)
        {
            Debug.Log("VASIFUDE");
            player.ChangeState(new Jumping()); 
        }
    }*/

        public override void StateEnd()
        {
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
            ClampOnGround();
            MoveByRigidbody();
            timeOnGround += Time.deltaTime;
        }

        #region PRIVATE METHODS
    
        void MoveByRigidbody()
        {
            if(rb == null)
                return;
            
            if(rb.velocity.x < player.SharedValues.RealVelocity)
                rb.AddForce(player.SharedValues.RealVelocity * Time.deltaTime * Vector3.right, ForceMode.VelocityChange);

            player.groundedVelocity = rb.velocity;
        }

        void ClampOnGround()
        {
            RaycastHit rotationHit;
            if (Physics.Raycast(player.transform.position, Vector3.down, out rotationHit, 10f, LayerMask.GetMask("Track")))
            {
                if(Vector3.Distance((player.transform.position + player.SharedValues.CharacterHeight * 0.5f * Vector3.down), rotationHit.point) > 2f)
                {
                    player.ChangeState(new Jumping());
                    return;
                }
            
                Quaternion newRotation = Quaternion.FromToRotation(player.transform.up, rotationHit.normal) * player.transform.rotation;
                newRotation.y = newRotation.x = 0;

                if (Vector3.Distance(rotationHit.point, player.transform.position) >
                    player.SharedValues.CharacterHeight)
                {
                    player.ChangeState(new Jumping(false));
                    return;
                }

                player.transform.position = new Vector3(player.transform.position.x, rotationHit.point.y + player.SharedValues.CharacterHeight * 0.5f, player.transform.position.z);
                player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, newRotation, 100 * Time.deltaTime);
            }
            else
            {
                player.ChangeState(new Jumping(false));
            }
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
            if((context.started || context.performed) && timeOnGround>=timeToJump)
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
