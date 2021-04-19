using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fallen : PlayerState
{
    float time = 0;
    float timeFall = 3;
    float timeToCorrect = 0.5f;
    float iterationTime = 0.1f;

    Quaternion newRotation = Quaternion.identity;
    bool canRotate = false;
    float rotationDifference = 0;
    public override void InterpretateInput(GameInput input)
    {

    }

    public override void StateEnd()
    {
        player.StopAllCoroutines();

        player.SetOnAnimator("fallen", false);
        player.SetOnAnimator("hardFall", false);
    }

    public override void StateStart(Player player)
    {
        base.StateStart(player);
        player.StartCoroutine(CorrectPlayerPosition());

        player.SetOnAnimator("fallen", true);
        player.SetOnAnimator("highSpeed", false);
        player.SetOnAnimator("hitByFuckFriend", false);
    }
    public override void StateUpdate()
    {
        
        if(time <= timeToCorrect && canRotate)
        {
            CorrectPlayerRotation();
        }      
        if (time >= timeFall)
        {
            player.ChangeState(new Grounded(1f));
        }

        time += Time.deltaTime;
    }

    void CorrectPlayerRotation()
    {
        if (rotationDifference == 0)
            rotationDifference = Mathf.Abs((player.transform.eulerAngles.z%360) - newRotation.eulerAngles.z);

        player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, newRotation, (rotationDifference/timeToCorrect)* Time.deltaTime);
    }

    IEnumerator CorrectPlayerPosition()
    {
        RaycastHit hit;

        if(Physics.Raycast(player.transform.position, Vector3.down, out hit, 10f, LayerMask.GetMask("Track")))
        {
            float X = hit.point.x + hit.normal.x;
            float Y = hit.point.y + (player.SharedValues.CharacterHeight / 2) * hit.normal.y;
            Vector3 newPosition = new Vector3(X, Y);

            newRotation = Quaternion.FromToRotation(player.transform.up, hit.normal) * player.transform.rotation;
            newRotation.y = newRotation.x = 0;
            canRotate = true;

            while (time <= timeToCorrect)
            {
                player.transform.position = Vector3.Lerp(player.transform.position, newPosition, timeToCorrect);

                yield return new WaitForSeconds(iterationTime);
            }
        }
    }

    public Fallen(float timeFall)
    {
        this.timeFall = timeFall;
    }

    public Fallen()
    {
    }
}
