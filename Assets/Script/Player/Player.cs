using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
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

    public GameCamera playerCamera { get;  set; }

    PlayerState playerState = new Grounded();

    private Vector3 startPoint;
    private string jumpInput;
    private string fireInput;

    

    public PlayerState GetPlayerState()
    {
        return playerState;
    }

    private void Awake()
    {
        sharedValues.player = this;
    }

    private void Update()
    {
        if(update)
            playerState.StateUpdate();

        InputInterpretation();
    }

    private void InputInterpretation()
    {
        if (Input.GetButtonDown(jumpInput))
            playerState.InterpretateInput(GameInput.UP);
        if (Input.GetButton(jumpInput))
            playerState.InterpretateInput(GameInput.UP_HOLD);
        if (Input.GetKey(KeyCode.Z))
            Restart();
        if (Input.GetButtonDown(fireInput) && Coletavel != null)
        {
            Coletavel.Activate(this);
            Coletavel = null;
        }
    }

    void Restart()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    private void Start()
    {
        playerState.StateStart(this);
        update = true;
        startPoint = transform.position;
        jumpInput = "JumpPlayer" + sharedValues.playerCode;
        fireInput = "FirePlayer" + sharedValues.playerCode;
    }

    public void ChangeState(PlayerState newState)
    {
        Debug.Log("Trocou estado " + newState.GetType().ToString());

        playerState.StateEnd();

        playerState = newState;
        playerState.StateStart(this);
    }

    public void StartStateCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public void SetOnAnimator(string variable, bool value)
    {
        if(animator != null)
            animator.SetBool(variable, value);
    }

    private void OnDrawGizmos()
    {
        if(playerState.GetType() == typeof(Jumping))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(((Jumping)playerState).groundingCheck, 0.2f);
        }
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

    private float addedVelocity = 0;

    public Player player { get; set; }

    public int playerCode { get; set; }

    public bool etherium { get; set; }

    public Vector3 ActualGroundNormal { get; set; }

    public float AddedVelocity
    {
        get
        {
            return addedVelocity;
        }
        set
        {
            addedVelocity = value;
            bool setAnim = false;

            if (addedVelocity > 5 && player.GetPlayerState().GetType() != typeof(Dead))
                setAnim = true;

            player.SetOnAnimator("highSpeed", setAnim);
        }
    }
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
            return Mathf.Clamp(jumpFactor * RealVelocity, 1, 10);
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

    public float RealVelocity
    {
        get
        {
            return velocity + AddedVelocity + InclinationVelocity;
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

public enum GameInput { UP, UP_HOLD }

