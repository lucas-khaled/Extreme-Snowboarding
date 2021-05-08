using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.EstadosPlayer
{
    public abstract class PlayerState
    {
        protected Player.Player player;
        protected PlayerInput playerInput;

        public virtual void StateStart(Player.Player player)
        {
            this.player = player;
        }

        public abstract void StateEnd();

        public abstract void StateUpdate();

        //public abstract void InterpretateInput(GameInput input);

        public virtual void OnCollisionEnter(Collision collision)
        {
        }

    }
}
    