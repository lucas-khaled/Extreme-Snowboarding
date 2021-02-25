using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Test : MonoBehaviour
{

    [SerializeField]
    private float velocity = 5f;
    [SerializeField, Range(0f, 1f)]
    private float deaccelerationOnSlope = 0.5f;
    [SerializeField, Range(1f,2f)]
    private float checkingPointDistance = 1f;
    [SerializeField]
    private float characterHeight = 2;
    [SerializeField]
    private float rotationLerp = 0.5f;

    float velocityRate;
    float movementStep;

    private void Start()
    {
        movementStep = checkingPointDistance * 10;
        velocityRate = 2 / (velocity * movementStep);
        StartCoroutine(CalculateNextPoint());
        StartCoroutine(CorrectRotation());
    }


    IEnumerator CalculateNextPoint()
    {
        while (true)
        {
            //Debug.Log("<color=red> Calculating </color>");

            RaycastHit hit;
            Vector3 checkingPosition = transform.position + Vector3.right * checkingPointDistance;

            Coroutine movingCoroutine = null;

            if (Physics.Raycast(checkingPosition, Vector3.down, out hit, 1000f, LayerMask.GetMask("Track")))
                movingCoroutine = StartCoroutine(Movement(CalculatePlayerPosition(hit)));
            else if (Physics.Raycast(checkingPosition, (Vector3.left * (deaccelerationOnSlope) + Vector3.up * (1 - deaccelerationOnSlope)).normalized, out hit, 1000f, LayerMask.GetMask("Track")))
                movingCoroutine = StartCoroutine(Movement(CalculatePlayerPosition(hit, true)));
            else
                movingCoroutine = StartCoroutine(Movement(checkingPosition));

            

            yield return movingCoroutine;
        }
    }


    Vector3 CalculatePlayerPosition(RaycastHit hit, bool invert = false)
    {
        int invertionValue = (invert) ? -1 : 1;
        float X = hit.point.x + hit.normal.x * invertionValue;
        float Y = hit.point.y + (characterHeight / 2) * hit.normal.y * invertionValue;

        return new Vector2(X, Y);
    }

    IEnumerator Movement(Vector3 position)
    {
        Vector3 steps = (position - transform.position) /movementStep;

        //Debug.Log(position);

        while (Vector3.Distance(transform.position, position) > 0.01f)
        {
            transform.position += steps;

            //Debug.Log(Vector3.Distance(transform.position, position));

            yield return new WaitForSeconds(velocityRate);
        }

    }

    IEnumerator CorrectRotation()
    {
        RaycastHit rotationHit;
        while (true)
        {
            if(Physics.Raycast(transform.position, Vector3.down, out rotationHit, 10f, LayerMask.GetMask("Track")))
            {
                Quaternion newRotation = Quaternion.FromToRotation(transform.up, rotationHit.normal) * transform.rotation;

                newRotation.x = Mathf.Lerp(transform.rotation.x, newRotation.x, rotationLerp);
                transform.rotation = newRotation;//Quaternion.Lerp(transform.rotation, newRotation, 0.5f);
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 checkingPoint = transform.position + Vector3.right * checkingPointDistance;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position + transform.up * characterHeight/2, transform.position - transform.up * characterHeight/2);

        Gizmos.DrawLine(checkingPoint, checkingPoint + (Vector3.left * (deaccelerationOnSlope) + Vector3.up * (1 - deaccelerationOnSlope)).normalized);

        Gizmos.DrawSphere(transform.position + Vector3.right * checkingPointDistance, 0.1f);
    }
}
