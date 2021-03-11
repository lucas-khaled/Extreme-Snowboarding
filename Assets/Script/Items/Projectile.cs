using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public FuckFriend fuckfriend { private get; set; }
    public Player caster { private get; set; }


    private MovementType movement;

    [SerializeField]
    private float speed = 5f;

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
                MoveStraight();
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

    void MoveStraight()
    {
        transform.position += Vector3.right * speed * Time.deltaTime * 10;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player playerHitted = other.GetComponent<Player>();
            if(playerHitted != caster)
            {
                fuckfriend.StartEffects(other.GetComponent<Player>());
                Destroy(this.gameObject);
            }
        }
        else
            Destroy(this.gameObject);

    }
}
