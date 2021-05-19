using ExtremeSnowboarding.Script.Controllers;
using UnityEngine;

namespace ExtremeSnowboarding.Script.EstadosPlayer
{
    public class RaceEndState : PlayerState
    {
        private Player.Player playerView;
        private Rigidbody rb;

        public override void StateEnd()
        {
            player.StopAllCoroutines();
        }

        public override void StateStart(Player.Player player)
        {
            base.StateStart(player);
            CorridaController.instance.PlayerFinishedRace(player);
            rb = player.GetComponent<Rigidbody>();
        }

        public override void StateUpdate()
        {
            ClampOnGround();
            DeAccelerateByRigidbody();
        }

        private void DeAccelerateByRigidbody()
        {
            if (rb == null || rb.velocity == Vector3.zero)
                return;

            if (rb.velocity.x > 1)
                rb.AddForce(2 * Time.deltaTime * Vector3.left, ForceMode.VelocityChange);
            else
            {
                Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
                rb.velocity = Vector3.zero;
                FinishRaceAnimation();
            }
                
        }

        private void ClampOnGround()
        {
            RaycastHit rotationHit;
            if (Physics.Raycast(player.transform.position, Vector3.down, out rotationHit, 10f, LayerMask.GetMask("Track")))
            {
                Quaternion newRotation = Quaternion.FromToRotation(player.transform.up, rotationHit.normal) * player.transform.rotation;
                newRotation.y = newRotation.x = 0;
                player.transform.position = new Vector3(player.transform.position.x, rotationHit.point.y + player.SharedValues.CharacterHeight * 0.5f, player.transform.position.z);
                player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, newRotation, 100 * Time.deltaTime);
            }
        }
        
        private void FinishRaceAnimation()
        {
            //if (CorridaController.instance.playersClassificated[0] != null)
            //{
            if (CorridaController.instance.playersClassificated[0] == player)
            {
                player.SetOnAnimator("highSpeed", false);
                player.SetOnAnimator("wonRace", true);
            }
            else
            {
                player.SetOnAnimator("highSpeed", false);
                player.SetOnAnimator("lostRace", true);
            }
            //}

            //player.Invoke("ChangePlayerView",5);
        }
    
    }
}
