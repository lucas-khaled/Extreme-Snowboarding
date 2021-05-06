using UnityEngine;
using UnityEngine.InputSystem;
using Script.Items.Effects;

[System.Serializable]
public class Jumping : PlayerState
{
    Rigidbody rb;
    float airTime = 0;
    float howMuchRotation = 0f;
    float rotatingDirection = 0;
    private bool isJumpingPressed;
    private bool applyForce;

    /*public override void InterpretateInput(GameInput input)
    {
        if (GameInput.UP_HOLD == input && airTime >= 0.2f)
            RotatePlayer();
        else if (GameInput.DOWN_HOLD == input && airTime >= 0.2f)
            RotatePlayer(-1);
        else if (GameInput.NO_INPUT == input)
        {
            Debug.Log("CARAAAAAAAAAAAAAAAAAAAAAIIII");
        }

    }*/
    public override void StateEnd()
    {
        rb.velocity = Vector3.zero;
        rb.useGravity = false;

        airTime = 0;
        player.SetOnAnimator("jumping", false);
        player.SetOnAnimator("trick", false);
        
        UnsubscribeOnInputEvents();

        // Daniboy Code starts >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        if(player.GetOnAnimator("hitByFuckFriend")){
            
        }
        // Daniboy Code ends >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

    }

    public override void StateStart(Player player)
    {
        base.StateStart(player);
        
        SubscribeOnInputEvents();
        
        rb = player.gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        
        if(applyForce)
            rb.AddForce(player.SharedValues.JumpForce * 0.8f * Vector3.up , ForceMode.Impulse);

        player.SetOnAnimator("jumping", true);
    }

    public override void StateUpdate()
    {
        airTime += Time.deltaTime;
        if(airTime >= 0.2f)
            RotatePlayer();

        if (isJumpingPressed)
            rotatingDirection = 1;

    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Track"))
        {       
            PlayerState newPlayerState;

            Vector3 realNormal = (collision.GetContact(0).normal.y < 0) ? -collision.GetContact(0).normal : collision.GetContact(0).normal;

            float groundAngle = Vector3.SignedAngle(Vector3.up, realNormal, Vector3.forward);

            float playerAngle = (player.transform.eulerAngles.z) % 360;
            float normalizedPlayerAngle = (playerAngle <= 180) ? playerAngle : playerAngle - 360;

            float angleDifference = Mathf.Abs(groundAngle - normalizedPlayerAngle);

            if (angleDifference < 60f && !player.GetOnAnimator("hitByFuckFriend"))
            {
                int timeEtherium = Mathf.FloorToInt((airTime * 0.33f) % 3f);
                newPlayerState = new Grounded(timeEtherium, 0.3f);

                ApplyAirEffects();
            }
            else if(!player.GetOnAnimator("hitByFuckFriend"))
            {
                float timeFall = 3.2f;//44 frames a 30 frames por segundo fall + 69 nice levantando
                
                if (angleDifference > 120)
                {
                    player.SetOnAnimator("hardFall", true);
                    timeFall = 4.6f;//84 frames a 30 frames por segundo hardfall + 69 nice levantando
                }

                newPlayerState = new Fallen(timeFall);

            }
            // Daniboy Code starts >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            else{
                
                float timeFall = 3.2f;//44 frames a 30 frames por segundo fall + 69 nice levantando
                
                newPlayerState = new Fallen(timeFall);

            }
            // Daniboy Code ends >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

            player.ChangeState(newPlayerState);
        }
    }

    void RotatePlayer()
    {
        float rotation = player.SharedValues.RotationFactor * Time.deltaTime * 100 * rotatingDirection;
        player.transform.Rotate(Vector3.forward * rotation, Space.Self);
        howMuchRotation += rotation;
    }

    void StartRotatePlayer(InputAction.CallbackContext context)
    {
        rotatingDirection = context.ReadValue<float>();
        
        if (context.canceled)
        {
            player.SetOnAnimator("trick", false);
        }
        else
        {
            player.SetOnAnimator("trick", true);
        }
    }

    void ApplyAirEffects()
    {
        int numOfMortals = 0;

        if (Mathf.Abs(howMuchRotation) > 180)
        {
            numOfMortals = Mathf.RoundToInt(howMuchRotation / 360);
            Debug.Log("Mortal :" + numOfMortals + "x");

            float amount = 1.3f * numOfMortals;
            float time = Mathf.Clamp(airTime * numOfMortals, 0, 2);

            Effect mortalEffect = new Effect("AddedVelocity",amount, time, EffectMode.ADD);
            mortalEffect.StartEffect(player);
        }

        player.AddTurbo(numOfMortals * 5);
    }

    void SubscribeOnInputEvents()
    {
        if (playerInput == null)
            playerInput = player.playerInput;

        playerInput.SwitchCurrentActionMap("Jumping");

        playerInput.currentActionMap.Enable();
        
        playerInput.currentActionMap.FindAction("Rotate").performed += StartRotatePlayer;
        playerInput.currentActionMap.FindAction("Rotate").canceled += StartRotatePlayer;
    }
    
    void UnsubscribeOnInputEvents()
    {
        if (playerInput == null)
            playerInput = player.playerInput;

        playerInput.currentActionMap.FindAction("Rotate").performed -= StartRotatePlayer;
        playerInput.currentActionMap.FindAction("Rotate").canceled -= StartRotatePlayer;
        
        playerInput.currentActionMap.Enable();
    }

    #region Constructors

    public Jumping(bool applyForce = true)
    {
        this.applyForce = applyForce;
    }

    #endregion
}
