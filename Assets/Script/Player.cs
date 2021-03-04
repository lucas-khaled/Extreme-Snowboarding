using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerSharedValues sharedValues;

    public PlayerSharedValues SharedValues
    {
        get
        {
            return sharedValues;
        }
        private set
        {
            sharedValues = value;
        }
    }

    public Item Coletavel { get; set; }

    IPlayerState playerState = new Grounded();

    public bool update { get; set; }

    private void Update()
    {
        if(update)
            playerState.StateUpdate();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerState.InterpretateInput(GameInput.SPACE);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            playerState.InterpretateInput(GameInput.SPACE_HOLD);
        }
    }

    private void Start()
    {
        playerState.StateStart(this);
        update = true;
    }

    public void ChangeState(IPlayerState newState)
    {
        playerState.StateEnd();

        playerState = newState;
        playerState.StateStart(this);
    }

    public void StartEffect(Effect[] atributes)
    {
        /*foreach(Effect efeito in atributes) {} */
    }

    public void StartStateCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    private void OnDrawGizmos()
    {
        if(playerState.GetType() == typeof(Jumping))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(((Jumping)playerState).groundingCheck, 0.2f);
        }
    }
}

[System.Serializable]
public class PlayerSharedValues
{
    [Header("Movement Values")]
    [SerializeField]
    private float velocity = 10f;
    [SerializeField, Range(1f, 2f)]
    private float checkingPointDistance = 1f;
    [SerializeField, Range(0f, 1f)]
    private float deaccelerationOnSlope = 0.5f;
    [SerializeField]
    private float characterHeight = 2;
    [SerializeField]
    private float jumpFactor = 0.5f;
    [SerializeField]
    private float rotationFactor = 3;

    public Vector3 ActualGroundNormal { get; set; }

    public float RotationFactor
    {
        get
        {
            return rotationFactor;
        }
        private set
        {
            rotationFactor = value;
        }
    }

    public float JumpForce
    {
        get
        {
            return jumpFactor * velocity;
        }
    }

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
}

public enum GameInput { SPACE, SPACE_HOLD }

