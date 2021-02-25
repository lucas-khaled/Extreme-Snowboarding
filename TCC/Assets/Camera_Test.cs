using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Test : MonoBehaviour
{

    Player_Test player;
    Vector3 offset;


    private void Awake()
    {
        player = GameObject.FindObjectOfType<Player_Test>();
    }



    // Update is called once per frame
    void LateUpdate()
    {
        transform.parent.transform.position = player.transform.position;
    }
}
