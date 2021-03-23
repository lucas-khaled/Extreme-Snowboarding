using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected Player player;

    public virtual void StateStart(Player player)
    {
        this.player = player;
    }

    public abstract void StateEnd();

    public abstract void StateUpdate();

    public abstract void InterpretateInput(GameInput input);
}
    