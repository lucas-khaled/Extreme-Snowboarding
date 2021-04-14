using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Grounded : PlayerState
{
    float timeEtherium;

    float timeOnGround = 0;
    float timeToJump = 0;

    Rigidbody rb;

    public override void InterpretateInput(GameInput input)
    {
        if(input == GameInput.UP && timeOnGround>=timeToJump)
            player.ChangeState(new Jumping());
    }

    public override void StateEnd()
    {
        player.GetPlayerVFXList().GetVFXByName("NeveEspalha").StopParticle();
        player.GetPlayerVFXList().GetVFXByName("FastMovement").LockParticle(true);
        player.StopAllCoroutines();
        player = null;
    }

    public override void StateStart(Player player)
    {
        base.StateStart(player);

        //player.StartStateCoroutine(CalculateNextPoint());

        rb = player.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = false;

        player.StartStateCoroutine(BeEtherium());

        player.GetPlayerVFXList().GetVFXByName("NeveEspalha").StartParticle();
        player.GetPlayerVFXList().GetVFXByName("FastMovement").UnlockParticle();

        player.GetComponent<Rigidbody>().velocity = player.groundedVelocity;
    }

    public override void StateUpdate()
    {
        CorrectRotation();
        MoveByRigidbody();
        timeOnGround += Time.deltaTime;
    }

    void MoveByRigidbody()
    {
        if(rb.velocity.x < player.SharedValues.RealVelocity)
            rb.AddForce(Vector3.right * player.SharedValues.RealVelocity * Time.deltaTime, ForceMode.VelocityChange);

        player.groundedVelocity = rb.velocity;
    }

    #region PRIVATE METHODS

    void CorrectRotation()
    {
        RaycastHit rotationHit;
        if (Physics.Raycast(player.transform.position, Vector3.down, out rotationHit, 10f, LayerMask.GetMask("Track")))
        {
            player.SharedValues.ActualGroundNormal = rotationHit.normal;
            Quaternion newRotation = Quaternion.FromToRotation(player.transform.up, rotationHit.normal) * player.transform.rotation;
            newRotation.y = newRotation.x = 0;

            player.transform.position = new Vector3(player.transform.position.x, rotationHit.point.y + player.SharedValues.CharacterHeight * 0.5f, player.transform.position.z);
            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, newRotation, 100 * Time.deltaTime);
        }
    }

    IEnumerator BeEtherium()
    {
        player.SharedValues.etherium = true;
        yield return new WaitForSeconds(timeEtherium);
        player.SharedValues.etherium = false;
    }

    #endregion

    #region CONSTRUCTORS

    public Grounded()
    {
        timeEtherium = 0;
        timeToJump = 0;
    }

    public Grounded(float timeEtherium)
    {
        this.timeEtherium = timeEtherium;
    }

    public Grounded(float timeEtherium, float timeToJump)
    {
        this.timeEtherium = timeEtherium;
        this.timeToJump = timeToJump;
    }

    #endregion
}
