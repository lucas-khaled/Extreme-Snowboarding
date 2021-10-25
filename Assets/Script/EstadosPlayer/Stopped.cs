using UnityEngine;

namespace ExtremeSnowboarding.Script.EstadosPlayer
{
    public class Stopped : PlayerState
    {
        private Rigidbody _rb;
        public override void StateStart(Player.Player player)
        {
            base.StateStart(player);
            _rb = player.GetComponent<Rigidbody>();
            _rb.useGravity = true;
        }

        public override void StateEnd()
        {
            
        }

        public override void StateUpdate()
        {
            
        }
    }
}