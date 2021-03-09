using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
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

    public bool update { get; set; }

    public bool etherium { get; set; } 

    public Camera_Test playerCamera { get;  set; }

    PlayerState playerState = new Grounded();

    private Vector3 startPoint;

    private void Update()
    {
        if(update)
            playerState.StateUpdate();

        if (Input.GetKeyDown(KeyCode.Space))
            playerState.InterpretateInput(GameInput.SPACE);
        if (Input.GetKey(KeyCode.Space))
            playerState.InterpretateInput(GameInput.SPACE_HOLD);
        if (Input.GetKey(KeyCode.R))
            Restart();
    }

    void Restart()
    {
        transform.position = startPoint;
    }

    private void Start()
    {
        playerState.StateStart(this);
        update = true;
        startPoint = transform.position;
    }

    public void ChangeState(PlayerState newState)
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position + (Vector3.up * sharedValues.CharacterHeight / 2), transform.position + (Vector3.down * sharedValues.CharacterHeight / 2));
        Gizmos.DrawLine(transform.position + (Vector3.right * sharedValues.CharacterRadius / 2), transform.position + (Vector3.left * sharedValues.CharacterRadius / 2));
    }
}

[System.Serializable]
public class PlayerSharedValues
{
    [Header("Player Values")]
    [SerializeField, Min(0)] 
    private float characterHeight = 2;
    [SerializeField, Min(0)]
    private float characterRadius = 1;

    [Header("Movement Values")]
    [SerializeField]
    private float velocity = 10f;
    [SerializeField, Range(1f, 2f)]
    private float checkingPointDistance = 1f;
    [SerializeField, Range(0f, 1f)]
    private float deaccelerationOnSlope = 0.5f;
    [SerializeField]
    private float jumpFactor = 0.5f;
    [SerializeField]
    private float rotationFactor = 3;

    public Vector3 ActualGroundNormal { get; set; }

    public float AddedVelocity { get; set; }
    public float InclinationVelocity { get; set; }

    public float CharacterRadius
    {
        get
        {
            return characterRadius;
        }
        set
        {
            characterRadius = value;
        }
    }

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
            return jumpFactor * Velocity;
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
            return velocity + AddedVelocity + InclinationVelocity;
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

