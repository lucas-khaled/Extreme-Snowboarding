using System.Collections;
using UnityEngine;
using ExtremeSnowboarding.Script.Items.Effects;
using UnityEngine.InputSystem;

namespace ExtremeSnowboarding.Script.EstadosPlayer
{
    public class Flying : PlayerState
    {
        private float maxPosition;
        private Rigidbody rb;

        public override void StateStart(Player.Player player)
        {
            base.StateStart(player);

            player.SetOnAnimator("jumping", true);

            maxPosition = player.transform.position.y + 10;

            rb = player.GetComponent<Rigidbody>();

            rb.useGravity = true;
            rb.isKinematic = false;

        }

        public override void StateUpdate()
        {
            if (player.transform.position.y < maxPosition)
            {
                rb.AddForce(0.75f * Vector3.up, ForceMode.Impulse);
            }
            else
            {
                player.ChangeState(new Jumping());
            }
        }

        public override void StateEnd()
        {

        }
    }
}
