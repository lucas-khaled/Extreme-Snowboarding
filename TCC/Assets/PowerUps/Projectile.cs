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
        if (movement == MovementType.RIGHT)
        {
            // movimentar para a direita
        }
        else if (movement == MovementType.LEFT)
        {
            // movimentar para a esquerda
        }
        else if (movement == MovementType.STOPPED)
        {
            // Ficar parado, ou uma animação de jogar?
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Effect[] effect = fuckfriend.GetAttributesToChange();
        other.gameObject.GetComponent<Player>().StartEffect(effect);
    }
}
