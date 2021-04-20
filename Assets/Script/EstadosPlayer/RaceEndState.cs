using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceEndState : PlayerState
{
    private Player playerView;
    private float timeToStop;
    private float deaccelerationFactor = 4f;
    private bool alreadyCalled = false;
    private float distance = 20f;

    private Rigidbody rb;

    public override void InterpretateInput(GameInput input)
    {
        if (GameInput.UP == input)
            ChangePlayerView();
    }

    public override void StateEnd()
    {
        player.StopAllCoroutines();
    }

    public override void StateStart(Player player)
    {
        base.StateStart(player);
        distance += player.SharedValues.RealVelocity;
        timeToStop = distance * 2f / player.SharedValues.RealVelocity;

        rb = player.GetComponent<Rigidbody>();
    }

    public override void StateUpdate()
    {
        DeaccelerateByRigidbody();
        FinishRaceAnimation();
    }

    void DeaccelerateByRigidbody()
    {
        if (rb.velocity.x > 0)
            rb.AddForce(Vector3.left * player.SharedValues.RealVelocity * Time.deltaTime, ForceMode.VelocityChange);
    }

    void FinishRaceAnimation()
    {
        //if (CorridaController.instance.playersClassificated[0] != null)
        //{
            if (CorridaController.instance.playersClassificated[0] == player)
            {
                player.SetOnAnimator("highSpeed", false);
                player.SetOnAnimator("wonRace", true);
            }
            else
            {
                player.SetOnAnimator("highSpeed", false);
                player.SetOnAnimator("lostRace", true);
            }
        //}

        player.Invoke("ChangePlayerView",5);
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
