using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float velocity = 5f;
    [SerializeField, Range(1f, 2f)]
    private float checkingPointDistance = 1f;
    [SerializeField, Range(0f, 1f)]
    private float deaccelerationOnSlope = 0.5f;
    [SerializeField]
    private float characterHeight = 2;


    public float CharacterHeight
    {
        get
        {
            return characterHeight;
        }
        private set
        {
            characterHeight = value;
        }
    }

    public float DeaccelerationOnSlope
    {
        get
        {
            return deaccelerationOnSlope;
        }
        set
        {
            deaccelerationOnSlope = value;
        }
    }

    public float Velocity
    {
        get
        {
            return velocity;
        }
        set
        {
            velocity = value;
        }
    }

    public float CheckingPointDistance
    {
        get
        {
            return checkingPointDistance;
        }
        private set
        {
            checkingPointDistance = value;
        }
    }

    IPlayerState playerState = new Grounded();

    private void Update()
    {
        playerState.StateUpdate();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerState.InterpretateInput(GameInput.SPACE);
        }
    }

    private void Start()
    {
        playerState.StateStart(this);
    }

    public void ChangeState(IPlayerState newState)
    {
        playerState.StateEnd();

        playerState = newState;
        playerState.StateStart(this);
    }
}

public enum GameInput { SPACE }
