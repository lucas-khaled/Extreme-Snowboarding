using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class restartPlayer_Test : MonoBehaviour
{
    Vector3 startPoint;
    // Start is called before the first frame update
    void Start()
    {
        startPoint = GameObject.FindObjectOfType<Player>().transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Player>().update = false;
            Debug.Log("ihuuu");
            other.transform.position = startPoint;
            other.GetComponent<Player>().update = true;
        }
    }
}
