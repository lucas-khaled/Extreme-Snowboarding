using ExtremeSnowboarding.Script.Controllers;
using UnityEngine;

namespace ExtremeSnowboarding.Script.EstadosPlayer
{
    public class RaceEndState : PlayerState
    {
        private Player.Player playerView;
        private Rigidbody rb;
        private int value;

        public override void StateEnd()
        {
            player.StopAllCoroutines();
        }

        public override void StateStart(Player.Player player)
        {
            base.StateStart(player);

            rb = player.GetComponent<Rigidbody>();

            rb.useGravity = false;
            rb.isKinematic = false;
            value = Random.Range(15,30);

            player.SharedValues.actualState = "RaceEnd";
            
            CorridaController.instance.PlayerFinishedRace(player);

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

            if (rb.velocity.x > 0)
                rb.AddForce(value * Time.deltaTime * Vector3.left, ForceMode.VelocityChange);
            else
            {
                rb.velocity = Vector3.zero;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                FinishRaceAnimation();
            }
                
        }

        private void ClampOnGround()
        {
            RaycastHit hit;
            if (Physics.Raycast(player.transform.position, -player.transform.up, out hit, player.SharedValues.CharacterHeight, LayerMask.GetMask("Track")))
            {
                ClampPlayerRotationByGround(hit);
                ClampPlayerPositionOnGround(hit);
                player.SharedValues.LastGroundedNormal = hit.normal.normalized;
            }
        }
        
        private void FinishRaceAnimation()
        {
            //if (CorridaController.instance.playersClassificated[0] != null)
            //{
            if (CorridaController.instance.playersClassificated[0] == player)
            {
                player.PlayVictoryAudio();
                player.SetOnAnimator("highSpeed", false);
                player.SetOnAnimator("wonRace", true);
            }
            else
            {
                player.PlayLostAudio();
                player.SetOnAnimator("highSpeed", false);
                player.SetOnAnimator("lostRace", true);
            }
            //}

            //player.Invoke("ChangePlayerView",5);
        }

        void ClampPlayerRotationByGround(RaycastHit hit)
        {
            Quaternion newRotation = Quaternion.FromToRotation(player.transform.up, hit.normal) * player.transform.rotation;
            newRotation.y = newRotation.x = 0;
            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, newRotation, 100 * Time.deltaTime);
        }
        void ClampPlayerPositionOnGround(RaycastHit hit)
        {

            float xChange = rb.velocity.x - hit.normal.normalized.x * 2f;
            float yChange = rb.velocity.y - hit.normal.normalized.y * 2f;

            rb.velocity = new Vector3(xChange, yChange, rb.velocity.z);
        }
    }
}
