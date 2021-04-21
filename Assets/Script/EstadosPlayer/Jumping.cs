using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class Jumping : PlayerState
{
    Rigidbody rb;
    float airTime = 0;
    float howMuchRotation = 0f;
    float rotatingDirection = 0;
    bool isJumpingPressed;

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
        
        UnsubscribeOnInputEvents();
    }

    public override void StateStart(Player player)
    {
        base.StateStart(player);
        
        SubscribeOnInputEvents();
        
        rb = player.gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(player.SharedValues.JumpForce * Vector3.up * 0.8f, ForceMode.Impulse);

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

            //Debug.Log("player Angle: " + normalizedPlayerAngle + "\n ground angle: " + groundAngle + "\n difference: " + angleDifference);

            if (angleDifference < 60f)
            {
                int timeEtherium = Mathf.FloorToInt((airTime * 0.33f) % 3f);
                newPlayerState = new Grounded(timeEtherium, 0.3f);

                ApplyAirEffects();
            }
            else
            {
                float timeFall = 3;
                if (angleDifference > 120)
                {
                    player.SetOnAnimator("hardFall", true);
                    timeFall = 4f;
                }

                newPlayerState = new Fallen(timeFall);

            }

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
            // parar de tocar animação de mortal
        }
        else
        {
            // tocar animação de mortal
        }
    }

    void ApplyAirEffects()
    {
        if (airTime > 1)
        {
            float amount = Mathf.Clamp(airTime / 2f, 0, 3);
            float time = Mathf.Clamp(airTime, 0, 2);
            Effect airEffect = new Effect("AddedVelocity", amount, time, Effect.EffectMode.ADD);
            player.StartCoroutine(airEffect.StartEffect(player));
        }

        int numOfMortals = 0;

        if (howMuchRotation > 180)
        {
            numOfMortals = Mathf.RoundToInt(howMuchRotation / 360);
            Debug.Log("Mortal :" + numOfMortals + "x");

            float amount = 1.3f * numOfMortals;
            float time = Mathf.Clamp(airTime * numOfMortals, 0, 2);

            Effect mortalEffect = new Effect("AddedVelocity",amount, time, Effect.EffectMode.ADD);
            player.StartCoroutine(mortalEffect.StartEffect(player));
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
}
