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
    private GameObject objectMesh;
    [Header("Player Values")]
    [SerializeField]
    private PlayerSharedValues sharedValues;
    [Header("Player VFX's")]
    [SerializeField]
    private PlayerVFXList playerVFXList;

    private UnityEngine.UI.Image turboBarRef;

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
    private string boostInput;
    private GameObject catastropheRef;

    public GameObject GetMeshGameObject()
    {
        return objectMesh;
    }

    public SkinnedMeshRenderer GetMeshRenderer()
    {
        return objectMesh.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
    }

    public PlayerState GetPlayerState()
    {
        return playerState;
    }

    public PlayerVFXList GetPlayerVFXList()
    {
        return playerVFXList;
    }

    private void Awake()
    {
        sharedValues.player = this;
        playerVFXList.StartHash();
    }

    private void Update()
    {
        if(update)
            playerState.StateUpdate();

        InputInterpretation();
        CheckTurbo();
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
        if (Input.GetButton(fireInput))
        {
            playerState.InterpretateInput(GameInput.DOWN_HOLD);
        }
        if (Input.GetButton(boostInput))
        {
            Debug.Log(sharedValues.Turbo);
            if (sharedValues.Turbo >= 0.95)
            {
                //float amount = Mathf.Clamp(turboStrengthVariation/ 2f, 0, 3);
                //float time = Mathf.Clamp(turboTimeVariation, 0, 2);
                Effect boostEffect = new Effect("AddedVelocity", 5f, 3f, Effect.EffectMode.ADD);
                StartCoroutine(boostEffect.StartEffect(this));
                sharedValues.Turbo = 0;
            }
        }
        if (Input.GetButton("TurboCheat"))
        {
            AddTurbo(100);
        }
    }
    private void CheckTurbo()
    {
        if (catastropheRef != null)
        {
            float distance = Vector3.Distance(this.gameObject.transform.position, catastropheRef.transform.position);
            AddTurbo(1 / distance);
            turboBarRef.fillAmount = sharedValues.Turbo / 1;
        }
        else if (CorridaController.instance.catastrophe != null)
                catastropheRef = CorridaController.instance.catastrophe;
    }

    public void AddTurbo(float turboValue)
    {
        float turbo = turboValue * sharedValues.turboMultiplier / 100;
        sharedValues.Turbo += turbo;
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
        boostInput = "BoostInput" + sharedValues.playerCode;
        catastropheRef = null;
        turboBarRef = playerCamera.transform.parent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        turboBarRef.fillAmount = 0;
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
    private float jumpFactor = 1f;
    [SerializeField]
    private float rotationFactor = 3;

    private float addedVelocity = 0;
    private float turbo = 0;

    public float turboMultiplier = 1;

    public Player player { get; set; }

    public int playerCode { get; set; }

    public bool etherium { get; set; }

    public Vector3 ActualGroundNormal { get; set; }

    public float Turbo 
    { 
        get 
        {
            return turbo;
        }
        set
        {
            if (value > 1)
            {
                turbo = 1;
            }
            else if (value < 0)
                turbo = 0;
            else
                turbo = value;
        }
    }

    public float AddedVelocity
    {
        get
        {
            return addedVelocity;
        }
        set
        {
            addedVelocity = value;

            if (addedVelocity > 5 && player.GetPlayerState().GetType() != typeof(Dead))
            {
                player.SetOnAnimator("highSpeed", true);
                player.GetPlayerVFXList().GetVFXByName("FastMovement").StartParticle();
            }
            else
            {
                player.SetOnAnimator("highSpeed", false);
                player.GetPlayerVFXList().GetVFXByName("FastMovement").StopParticle();
            }          
            
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
            return Mathf.Clamp(jumpFactor * RealVelocity*0.75f, 1, jumpFactor*20);
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
            return velocity + AddedVelocity;
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

[System.Serializable]
public class PlayerVFXList
{
    [SerializeField]
    private List<PlayerVFX> VFXList;

    private PlayerVFX[] VFXHashedList;

    private bool isHashed = false;

    /// <summary>
    /// This method starts hash on the list. It can oly be played once.
    /// </summary>
    public void StartHash()
    {
        if (isHashed)
            return;

        VFXHashedList = new PlayerVFX[VFXList.Count];

        foreach(PlayerVFX vfx in VFXList)
        {
            if (vfx.GetParticle() != null)
                VFXHashedList[GetHashedID(vfx.GetParticle().name)] = vfx;
        }

        isHashed = true;
    }

    #region GET_NAME_FUNCTIONS

    /// <summary>
    /// If the StartHash() was already called, it searches by hash. Otherwise, it will search the entire list. 
    /// Returns null when the particle name doesn't exists.
    /// </summary>
    public PlayerVFX GetVFXByName(string name)
    {
        if (isHashed)
            return GetVFXByNameHashed(name);
        else
            return GetVFXByNameNotHashed(name);
    }

    private PlayerVFX GetVFXByNameHashed(string name)
    {
        int index = GetHashedID(name, true);
        if (index == -1)
            return null;
        else
            return VFXHashedList[index];
    }

    private PlayerVFX GetVFXByNameNotHashed(string name)
    {
        foreach(PlayerVFX vfx in VFXList)
        {
            if (vfx.GetParticle().name == name)
                return vfx;
        }
        return null;
    }

    #endregion

    #region HASH_FUNCTIONS

    private int GetHashedID(string name, bool onlySearchingName = false)
    {
        int sum = 0;
        for(int i = 1; i<=name.Length; i++)
        {
            int operation = i % 3;
            switch (operation)
            {
                case 1:
                    sum += (int)name[i-1];
                    break;
                case 2:
                    sum -= (int)name[i-1];
                    break;
                case 3:
                    sum *= (int)name[i-1];
                    break;
            }
        }

        int initialIndex = Mathf.Abs(sum) % VFXList.Count;
        int finalIndex = (onlySearchingName) ? GetOffset(initialIndex, name) : GetEmptyIndex(initialIndex);

        Debug.Log(name + ": "+initialIndex + " - " + finalIndex);

        return finalIndex;
    }

    private int GetOffset(int index, string name)
    {
        return GetOffsetRecursive(index, index, name);
    }

    private int GetOffsetRecursive(int index, int initialValue, string name)
    {
        if (VFXHashedList[index].GetParticle().name == name)
            return index;
        else
        {
            if (index + 1 == initialValue)
                return index;
            if (index == VFXHashedList.Length - 1)
                return GetOffsetRecursive(0, initialValue, name);
            else
                return GetOffsetRecursive(index + 1, initialValue, name);
        }
    }



    private int GetEmptyIndex(int index)
    {
        return GetEmptyIndexRecursive(index, index);
    }

    private int GetEmptyIndexRecursive(int index, int initialValue)
    {
        if (VFXHashedList[index] == null)
            return index;
        else
        {
            if (index + 1 == initialValue)
                return -1;
            if (index == VFXHashedList.Length - 1)
                return GetEmptyIndexRecursive(0, index);
            else
                return GetEmptyIndexRecursive(index + 1, index);
        }
    }

    #endregion
}

[System.Serializable]
public class PlayerVFX
{
    [SerializeField]
    private ParticleSystem particle;

    private bool locked = false;

    public ParticleSystem GetParticle()
    {
        return particle;
    }

    /// <summary>
    /// Will lock the particle from having their state changed.
    /// </summary>
    /// <param name="stopPlaying"> Whether you want that particle locked state is on stop. </param>
    /// <param name="deactivate"> Wheter you want to also deactivate the particle Game Object. </param>
    public void LockParticle(bool stopPlaying = false, bool deactivate = false)
    {
        if (stopPlaying)
            StopParticle(deactivate);

        locked = true;
    }

    /// <summary>
    /// It will unlock the particle from having their state changed.
    /// </summary>
    /// <param name="startPlaying"> Wheter you want to also start the particle </param>
    public void UnlockParticle(bool startPlaying = false)
    {
        if (startPlaying)
            StartParticle();

        locked = false;
    }

    public void StartParticle()
    {
        if (locked)
            return;

        if (particle != null && particle.gameObject.scene.IsValid())
        {
            particle.gameObject.SetActive(true);
            particle.Play();
        }
    }

    public void StopParticle(bool deActivate = false)
    {
        if (locked)
            return;
        if (particle != null && particle.gameObject.scene.IsValid())
        {
            particle.gameObject.SetActive(!deActivate);
            particle.Stop();
        }
    }
}

public enum GameInput { UP, UP_HOLD, DOWN_HOLD }

