using System.Collections;
using UnityEngine;
using ExtremeSnowboarding.Script.Items.Effects;
using UnityEngine.InputSystem;
using PathCreation.Examples;

namespace ExtremeSnowboarding.Script.EstadosPlayer
{
    public class Flying : PlayerState
    {
        private float maxPosition;
        private Rigidbody rb;
        private PathFollower path;

        public override void StateStart(Player.Player player)
        {
            base.StateStart(player);
            player.SharedValues.actualState = "Flying";

            //player.SetOnAnimator("jumping", true);
            //maxPosition = player.transform.position.y + 10;

            player.playerCamera.shouldFollowOnlyX = true;
            rb = player.GetComponent<Rigidbody>();

            //rb.useGravity = true;
            //rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            player.groundedVelocity = Vector3.zero;
            player.StartCoroutine(changeShouldFollowByTimer());
            path = player.gameObject.GetComponent<PathFollower>();
            PathFollower.onPathFinished += OnPathFinished;
            path.auxOnce = false;
            
            if(PlayerPrefs.GetString("Mesh") == "Male")
                player.GetMovimentationFeedbacks().maleFlyingFeedback?.PlayFeedbacks();
            else
                player.GetMovimentationFeedbacks().femaleFlyingFeedback?.PlayFeedbacks();
        }

        public override void StateUpdate()
        {
            /*if (player.transform.position.y < maxPosition)
            {
                rb.AddForce(0.75f * Vector3.up, ForceMode.Impulse);
            }
            else
            {
                player.ChangeState(new Jumping());
            }*/
        }

        public override void StateEnd()
        {
            player.SetOnAnimator("Flying", false);
            path.shouldFollowPath = false;
            player.playerCamera.shouldFollowOnlyX = false;
            rb.AddForce(Vector3.right * 10, ForceMode.VelocityChange);
        }

        private void OnPathFinished(GameObject gameObject)
        {
            player.ChangeState(new Grounded());
            path.distanceTravelled = 0;
        }

        private IEnumerator changeShouldFollowByTimer()
        {
            yield return new WaitForSeconds(2f);
            path.shouldFollowPath = true;
            string[] animations = { "RetornandoDoAbismo" };
            player.ChangeAnimationTo(animations, "Flying", true);
        }
    }
}
