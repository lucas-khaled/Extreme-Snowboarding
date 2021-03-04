using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Jumping : IPlayerState
{
    Player player;
    Rigidbody rb;
    RaycastHit groundingHit;
    float airTime = 0;

    public Vector3 groundingCheck;

    public void InterpretateInput(GameInput input)
    {
        if (GameInput.SPACE_HOLD == input && airTime >=0.2f)
            player.transform.Rotate(Vector3.forward * player.SharedValues.RotationFactor*Time.deltaTime*100, Space.Self);
    }

    public void StateEnd()
    {
        rb.velocity = Vector3.zero;
        MonoBehaviour.Destroy(rb);
        airTime = 0;
    }

    public void StateStart(Player player)
    {
        this.player = player;
        rb = player.gameObject.AddComponent<Rigidbody>();
        rb.AddForce(player.SharedValues.JumpForce * CalculateJumpDirection(player), ForceMode.Impulse);
    }

    public void StateUpdate()
    {
        if(airTime >= 1)
        {
            groundingCheck = player.transform.position;
            float Y = player.SharedValues.CharacterHeight * 0.5f * Mathf.Cos(player.transform.rotation.z);
            float X = player.SharedValues.CharacterHeight * 0.5f * Mathf.Sin(player.transform.rotation.z);
            groundingCheck.y -= Y;
            groundingCheck.x += X;

            if (Physics.SphereCast(groundingCheck, 0.02f, Vector3.down, out groundingHit, 0.3f, LayerMask.GetMask("Track")))
            {
                IPlayerState newPlayerState;

                Vector3 realNormal = (groundingHit.normal.y < 0) ? -groundingHit.normal : groundingHit.normal;

                float groundAngle = Vector3.SignedAngle(Vector3.up, realNormal, Vector3.back);

                float playerAngle = (player.transform.eulerAngles.z) % 360;
                float normalizedPlayerAngle = (playerAngle <= 180) ? playerAngle : playerAngle-360;

                float angleDifference = Mathf.Abs(groundAngle - normalizedPlayerAngle);

                Debug.Log("player Angle: "+playerAngle+"\n ground angle: "+groundAngle+"\n difference: "+angleDifference);

                if (angleDifference < 60f)
                    newPlayerState = new Grounded();
                else
                    newPlayerState = new Fallen();

                player.ChangeState(newPlayerState);
            }
        }

        airTime += Time.deltaTime;
    }

    Vector3 CalculateJumpDirection(Player player)
    {
        float X = Mathf.Clamp(player.SharedValues.ActualGroundNormal.x, 0.3f, 1);
        float Y = player.SharedValues.ActualGroundNormal.y;

        return new Vector2(X, Y).normalized;
    }
}
