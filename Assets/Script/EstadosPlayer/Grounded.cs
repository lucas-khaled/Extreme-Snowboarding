using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Grounded : PlayerState
{
    float timeEtherium;

    float timeOnGround = 0;
    float timeToJump = 0;

    public override void InterpretateInput(GameInput input)
    {
        if(input == GameInput.UP && timeOnGround>=timeToJump)
            player.ChangeState(new Jumping());
    }

    public override void StateEnd()
    {
        player.GetPlayerVFXList().GetVFXByName("NeveEspalha").StopParticle();
        player.StopAllCoroutines();
        player = null;
    }

    public override void StateStart(Player player)
    {
        base.StateStart(player);   

        player.StartStateCoroutine(CalculateNextPoint());

        player.StartStateCoroutine(BeEtherium());

        player.GetPlayerVFXList().GetVFXByName("NeveEspalha").StartParticle();
    }

    public override void StateUpdate()
    {
        CorrectRotation();
        timeOnGround += Time.deltaTime;
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

            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, newRotation, 100 * Time.deltaTime);
        }
    }

    IEnumerator CalculateNextPoint()
    {
        while (true)
        {
            //Debug.Log("<color=red> Calculating </color>");

            RaycastHit hit;
            Vector3 checkingPosition = player.transform.position + Vector3.right * player.SharedValues.CheckingPointDistance;

            Coroutine movingCoroutine = null;

            if (Physics.Raycast(checkingPosition, Vector3.down, out hit, 1000f, LayerMask.GetMask("Track")))
                movingCoroutine = player.StartCoroutine(Movement(CalculatePlayerPosition(hit)));
            else if (Physics.Raycast(checkingPosition, (Vector3.left * (player.SharedValues.DeaccelerationOnSlope) + Vector3.up * (1 - player.SharedValues.DeaccelerationOnSlope)).normalized, out hit, 1000f, LayerMask.GetMask("Track")))
                movingCoroutine = player.StartCoroutine(Movement(CalculatePlayerPosition(hit, true)));
            else
                movingCoroutine = player.StartCoroutine(Movement(checkingPosition));

            yield return movingCoroutine;
        }
    }

    IEnumerator Movement(Vector3 position)
    {
        int movementStep = Mathf.RoundToInt(player.SharedValues.CheckingPointDistance / player.SharedValues.RealVelocity * 100f);
        Vector3 steps = (position - player.transform.position) / movementStep;
       

        while (Vector3.Distance(player.transform.position, position) > 0.01f)
        {
            float velocityRate = 1 / (player.SharedValues.RealVelocity * player.SharedValues.RealVelocity);
            player.transform.position += steps;

            yield return new WaitForSeconds(velocityRate);
        }

    }

    Vector3 CalculatePlayerPosition(RaycastHit hit, bool invert = false)
    {
        int invertionValue = (invert) ? -1 : 1;
        float X = hit.point.x + hit.normal.x * invertionValue;
        float Y = hit.point.y + (player.SharedValues.CharacterHeight / 2) * hit.normal.y * invertionValue;

        player.SharedValues.InclinationVelocity = hit.normal.x * invertionValue;

        return new Vector3(X, Y, player.transform.position.z);
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
