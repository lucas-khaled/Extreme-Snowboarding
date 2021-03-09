using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private FuckFriend fuckfriend;
    private MovementType movement;

    private void Start()
    {
        movement = fuckfriend.GetMovementType();
    }

    private void Update()
    {
        Movement();
    }

    public Projectile (FuckFriend fuckFriendParametro)
    {
        this.fuckfriend = fuckFriendParametro;
    }

    private void Movement()
    {
        switch (movement)
        {
            case MovementType.STRAIGHT:
                // movimento para frente sem seguir a pista
                break;
            case MovementType.FOWARD:
                // movimento para frente seguindo a pista
                break;
            case MovementType.STOPPED:
                // sem movimento, só mente parado
                break;
            case MovementType.BACK:
                // movimento para trás seguindo a pista
                break;
            case MovementType.STRAIGHT_BACK:
                // movimento para trás sem seguir a pista
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            fuckfriend.StartEffects(other.GetComponent<Player>());
    }
}
