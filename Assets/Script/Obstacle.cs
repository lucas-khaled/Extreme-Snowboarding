using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Obstacle : MonoBehaviour
{
    private Collider myCollider;


    private void Awake()
    {
        myCollider = GetComponent<Collider>();
        myCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (!player.SharedValues.Etherium)
                player.ChangeState(new Fallen());
        }
    }
}
