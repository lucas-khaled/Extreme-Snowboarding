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
            if (CheckOnGround())
            {
                _rb.isKinematic = true;
                _rb.velocity = Vector3.zero;
            }
                
        }

        private bool CheckOnGround()
        {
            return Physics.Raycast(player.transform.position, Vector3.down, player.SharedValues.CharacterHeight);
        }
    }
}