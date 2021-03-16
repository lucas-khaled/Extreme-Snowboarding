using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : PlayerState
{
    Player playerView;

    public override void InterpretateInput(GameInput input)
    {
        if (GameInput.UP == input)
            ChangePlayerView();
    }

    public override void StateEnd()
    {
        
    }

    public override void StateStart(Player player)
    {
        base.StateStart(player);
        playerView = player;
        ChangePlayerView();

        if (EventSystem.onPlayerDeath != null)
            EventSystem.onPlayerDeath.Invoke(player);

        MonoBehaviour.Destroy(player.GetComponent<MeshRenderer>()); 
    }

    public override void StateUpdate()
    {
        
    }

    ///<summary> 
    ///Change the player camera visualization to a random alive player (doesn't work if there's no players alive)
    ///</summary>
    public void ChangePlayerView()
    {
        playerView = CorridaController.instance.GetOtherPlayerThan(playerView);
        player.playerCamera.SetPlayer(playerView);
    }

    ///<summary>
    ///Change the player camera visualization to the specified player 
    ///</summary>
    public void ChangePlayerView(Player player)
    {
        playerView = player;
        player.playerCamera.SetPlayer(player);
    }
}
