using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : IPlayerState
{
    Player player;
    Rigidbody rb;

    public void InterpretateInput(GameInput input)
    {
        if (GameInput.SPACE == input)
            player.transform.Rotate(Vector3.forward * player.rotateFactor, Space.Self);
    }

    public void StateEnd()
    {
        
    }

    public void StateStart(Player player)
    {
        this.player = player;
        rb = player.gameObject.AddComponent<Rigidbody>();
        rb.AddForce(player.JumpForce * (Vector3.up + Vector3.right), ForceMode.Impulse);
    }

    public  Vector3 groundingCheck;
    public void StateUpdate()
    {
        groundingCheck = player.transform.position;
        float Y = player.CharacterHeight * 0.5f * Mathf.Cos(player.transform.rotation.z);
        float X = player.CharacterHeight * 0.5f * Mathf.Sin(player.transform.rotation.z);
        groundingCheck.y -= Y;
        groundingCheck.x += X;

        Debug.Log(Y + " - " + X);
    }
}
