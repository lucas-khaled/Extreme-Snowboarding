using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : IPlayerState
{
    float velocityRate;
    float movementStep;
    MonoBehaviour coroutineStarter;
    Player player;

    public void InterpretateInput(GameInput input)
    {
        if(input == GameInput.SPACE)
            player.ChangeState(new Jumping());
    }

    public void StateEnd()
    {
        player.StopAllCoroutines();
        player = null;
    }

    public void StateStart(Player player)
    {
        this.player = player;
        movementStep = player.CheckingPointDistance * 10;
        velocityRate = 2 / (player.Velocity * movementStep);

        player.StartStateCoroutine(CalculateNextPoint());
        player.StartStateCoroutine(CorrectRotation());
    }

    public void StateUpdate()
    {

    }

    IEnumerator CalculateNextPoint()
    {
        while (true)
        {
            //Debug.Log("<color=red> Calculating </color>");

            RaycastHit hit;
            Vector3 checkingPosition = player.transform.position + Vector3.right * player.CheckingPointDistance;

            Coroutine movingCoroutine = null;

            if (Physics.Raycast(checkingPosition, Vector3.down, out hit, 1000f, LayerMask.GetMask("Track")))
                movingCoroutine = player.StartCoroutine(Movement(CalculatePlayerPosition(hit)));
            else if (Physics.Raycast(checkingPosition, (Vector3.left * (player.DeaccelerationOnSlope) + Vector3.up * (1 - player.DeaccelerationOnSlope)).normalized, out hit, 1000f, LayerMask.GetMask("Track")))
                movingCoroutine = player.StartCoroutine(Movement(CalculatePlayerPosition(hit, true)));
            else
                movingCoroutine = player.StartCoroutine(Movement(checkingPosition));

            yield return movingCoroutine;
        }
    }

    IEnumerator Movement(Vector3 position)
    {
        Vector3 steps = (position - player.transform.position) / movementStep;

        //Debug.Log(position);

        while (Vector3.Distance(player.transform.position, position) > 0.01f)
        {
            player.transform.position += steps;

            //Debug.Log(Vector3.Distance(transform.position, position));

            yield return new WaitForSeconds(velocityRate);
        }

    }

    Vector3 CalculatePlayerPosition(RaycastHit hit, bool invert = false)
    {
        int invertionValue = (invert) ? -1 : 1;
        float X = hit.point.x + hit.normal.x * invertionValue;
        float Y = hit.point.y + (player.CharacterHeight / 2) * hit.normal.y * invertionValue;

        return new Vector2(X, Y);
    }

    IEnumerator CorrectRotation()
    {
        RaycastHit rotationHit;
        while (true)
        {
            if (Physics.Raycast(player.transform.position, Vector3.down, out rotationHit, 10f, LayerMask.GetMask("Track")))
            {
                Quaternion newRotation = Quaternion.FromToRotation(player.transform.up, rotationHit.normal) * player.transform.rotation;

                newRotation.x = Mathf.Lerp(player.transform.rotation.x, newRotation.x, 0.5f);
                player.transform.rotation = newRotation;//Quaternion.Lerp(transform.rotation, newRotation, 0.5f);
            }

            yield return new WaitForSeconds(0.2f);
        }
    }
}
