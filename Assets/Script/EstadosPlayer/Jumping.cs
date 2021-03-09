using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Jumping : PlayerState
{
    Rigidbody rb;
    RaycastHit groundingHit;
    float airTime = 0;

    float howMuchRotation = 0f;

    public Vector3 groundingCheck;

    public override void InterpretateInput(GameInput input)
    {
        if (GameInput.SPACE_HOLD == input && airTime >= 0.2f)
            RotatePlayer();
    }
    public override void StateEnd()
    {
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;

        airTime = 0;
    }

    public override void StateStart(Player player)
    {
        base.StateStart(player);

        rb = player.gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(player.SharedValues.JumpForce * CalculateJumpDirection(player), ForceMode.Impulse);
    }

    public override void StateUpdate()
    {
        if(airTime >= 1)
        {
            CheckGround();
        }

        airTime += Time.deltaTime;
    }


    void CheckGround()
    {
        groundingCheck = player.transform.position;
        float Y = player.SharedValues.CharacterHeight * 0.5f * Mathf.Cos(player.transform.rotation.z);
        float X = player.SharedValues.CharacterHeight * 0.5f * Mathf.Sin(player.transform.rotation.z);
        groundingCheck.y -= Y;
        groundingCheck.x += X;

        if (Physics.SphereCast(groundingCheck, 0.05f, Vector3.down, out groundingHit, 0.3f, LayerMask.GetMask("Track")))
        {
            PlayerState newPlayerState;

            Vector3 realNormal = (groundingHit.normal.y < 0) ? -groundingHit.normal : groundingHit.normal;

            float groundAngle = Vector3.SignedAngle(Vector3.up, realNormal, Vector3.forward);

            float playerAngle = (player.transform.eulerAngles.z) % 360;
            float normalizedPlayerAngle = (playerAngle <= 180) ? playerAngle : playerAngle - 360;

            float angleDifference = Mathf.Abs(groundAngle - normalizedPlayerAngle);

            Debug.Log("player Angle: " + normalizedPlayerAngle + "\n ground angle: " + groundAngle + "\n difference: " + angleDifference);

            if (angleDifference < 60f)
            {
                int timeEtherium = Mathf.FloorToInt((airTime * 0.33f) % 3f); 
                newPlayerState = new Grounded(timeEtherium);
                if (howMuchRotation > 180)
                    Debug.Log("<color=red> Mortaaaal "+Mathf.RoundToInt(howMuchRotation/360)+"x </color>");
            }
            else
                newPlayerState = new Fallen();

            player.ChangeState(newPlayerState);
        }
    }

    Vector3 CalculateJumpDirection(Player player)
    {
        float X = Mathf.Clamp(player.SharedValues.ActualGroundNormal.x, 0.3f, 1);
        float Y = player.SharedValues.ActualGroundNormal.y;

        return new Vector2(X, Y).normalized;
    }

    void RotatePlayer()
    {
        player.transform.Rotate(Vector3.forward * player.SharedValues.RotationFactor * Time.deltaTime * 100, Space.Self);
        howMuchRotation += player.SharedValues.RotationFactor * Time.deltaTime * 100;
    }
}
