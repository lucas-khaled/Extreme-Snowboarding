using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    [SerializeField] float paralaxEffect;

    private GameObject thisCamera;
    private float length;
    private float startpos;

    void Start()
    {
        thisCamera = transform.parent.transform.parent.gameObject;
        startpos = transform.position.x;
        length = this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = (thisCamera.transform.position.x * (1 - paralaxEffect));
        float dist = (thisCamera.transform.position.x * paralaxEffect);

        transform.position = new Vector3(startpos + dist, 
                                         thisCamera.transform.position.y, 
                                         transform.position.z);

        if (temp > startpos + length)
            startpos += length;
        else if (temp < startpos - length)
            startpos -= length;
            
    }
}
