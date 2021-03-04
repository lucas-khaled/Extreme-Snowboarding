using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fallen : IPlayerState
{
    float timeFall = 3;
    Player player;
    public void InterpretateInput(GameInput input)
    {
    }

    public void StateEnd()
    {

    }

    public void StateStart(Player player)
    {
        Debug.Log("Fallen");
        this.player = player;
    }

    public void StateUpdate()
    {
        timeFall -= Time.deltaTime;
        if (timeFall <= 0)
        {
            player.ChangeState(new Grounded());
        }
    }
}
