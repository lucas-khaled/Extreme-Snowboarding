using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.InputSystem;

public abstract class PlayerState
{
    protected Player player;
    protected PlayerInput playerInput;

    public virtual void StateStart(Player player)
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
    