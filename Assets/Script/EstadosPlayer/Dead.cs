using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : PlayerState
{
    Camera_Test playerCamera;
    public override void InterpretateInput(GameInput input)
    {
        //trocar para camera de outro player
    }

    public override void StateEnd()
    {
        
    }

    public override void StateStart(Player player)
    {
        base.StateStart(player);
        playerCamera = player.playerCamera;

        //passar a camera de outro player para a camera do player morto

        MonoBehaviour.Destroy(player.gameObject);
        
    }

    public override void StateUpdate()
    {
        
    }
}
