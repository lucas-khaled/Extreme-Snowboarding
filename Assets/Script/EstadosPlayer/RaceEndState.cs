using ExtremeSnowboarding.Script.Controllers;
using UnityEngine;

namespace ExtremeSnowboarding.Script.EstadosPlayer
{
    public class RaceEndState : PlayerState
    {
        private Player.Player playerView;
        private float timeToStop;
        private float distance = 20f;

        private Rigidbody rb;

        public override void StateEnd()
        {
            player.StopAllCoroutines();
        }

        public override void StateStart(Player.Player player)
        {
            base.StateStart(player);
            distance += player.SharedValues.RealVelocity;
            timeToStop = distance * 2f / player.SharedValues.RealVelocity;

            rb = player.GetComponent<Rigidbody>();
        }

        public override void StateUpdate()
        {
            DeaccelerateByRigidbody();
            FinishRaceAnimation();
        }

        void DeaccelerateByRigidbody()
        {
            if (rb.velocity.x > 0)
                rb.AddForce(Vector3.left * player.SharedValues.RealVelocity * Time.deltaTime, ForceMode.VelocityChange);
        }

        void FinishRaceAnimation()
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

            player.Invoke("ChangePlayerView",5);
        }
    
    }
}
