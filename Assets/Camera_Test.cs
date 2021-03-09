using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Test : MonoBehaviour
{

    Player player;
    Vector3 offset;

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(player != null)
            transform.parent.transform.position = player.transform.position;
    }
}
