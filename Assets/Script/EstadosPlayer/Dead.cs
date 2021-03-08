using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : PlayerState
{
    public override void InterpretateInput(GameInput input)
    {
        
    }

    public override void StateEnd()
    {
        
    }

    public override void StateStart(Player player)
    {
        base.StateStart(player);
        MonoBehaviour.Destroy(player.gameObject);
    }

    public override void StateUpdate()
    {
        
    }
}
