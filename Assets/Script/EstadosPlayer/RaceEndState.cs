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
        CalculateNextPoint();
        Debug.Log(timeToStop);
    }

    public override void StateUpdate()
    {
        if (timeToStop <= 0)
        {
            player.StopAllCoroutines();
            FinishRaceAnimation();
        }

        timeToStop -= Time.deltaTime * 2;
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

    void CalculateNextPoint()
    {
        //Debug.Log("<color=red> Calculating </color>");

        RaycastHit hit;
        Vector3 checkingPosition = player.transform.position + Vector3.right * distance;

        if (Physics.Raycast(checkingPosition, Vector3.down, out hit, 1000f, LayerMask.GetMask("Track")))
            player.StartCoroutine(Movement(CalculatePlayerPosition(hit)));
        else if (Physics.Raycast(checkingPosition, (Vector3.left * (player.SharedValues.DeaccelerationOnSlope) + Vector3.up * (1 - player.SharedValues.DeaccelerationOnSlope)).normalized, out hit, 1000f, LayerMask.GetMask("Track")))
            player.StartCoroutine(Movement(CalculatePlayerPosition(hit, true)));
        else
            player.StartCoroutine(Movement(checkingPosition));

    }

    IEnumerator Movement(Vector3 position)
    {
        float movementStep = 18;

        while (Vector3.Distance(player.transform.position, position) > 0.01f)
        {
            Vector3 steps = (position - player.transform.position) / movementStep;
            movementStep *= 1.5f;

            float velocityRate = 1 / (timeToStop * timeToStop);
            player.transform.position += steps;

            yield return new WaitForSeconds(velocityRate);
        }
    }

    Vector3 CalculatePlayerPosition(RaycastHit hit, bool invert = false)
    {
        int invertionValue = (invert) ? -1 : 1;
        float X = hit.point.x + hit.normal.x * invertionValue;
        float Y = hit.point.y + (player.SharedValues.CharacterHeight / 2) * hit.normal.y * invertionValue;

        return new Vector3(X, Y, player.transform.position.z);
    }
}
