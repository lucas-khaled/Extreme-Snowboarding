using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    [SerializeField] float paralaxEffect;

    private GameObject camera;
    private float length;
    private float startpos;

    void Start()
    {
        camera = transform.parent.transform.parent.gameObject;
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = (camera.transform.position.x * (1 - paralaxEffect));
        float dist = (camera.transform.position.x * paralaxEffect);

        transform.position = new Vector3(startpos + dist, 
                                         transform.position.y, 
                                         transform.position.z);

        if (temp > startpos + length)
            startpos += length;
        else if (temp < startpos - length)
            startpos -= length;
            
    }
}
