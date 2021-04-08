using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public FuckFriend fuckfriend { private get; set; }
    public Player caster { private get; set; }


    private MovementType movement;
    private bool alreadyStopped = false;

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
                MoveStraightForward();
                break;
            case MovementType.FOWARD:
                MoveForward();
                break;
            case MovementType.STOPPED:
                MoveStopped();
                break;
            case MovementType.BACK:
                MoveBack();
                break;
            case MovementType.STRAIGHT_BACK:
                MoveStraightBack();
                break;
        }
    }

    #region Movement Types
    void MoveStraightForward()
    {
        transform.position += Vector3.right * speed * Time.deltaTime * 10;
    }
    void MoveForward()
    {
        //movimento para frente seguindo a pista
    }
    void MoveStopped()
    {
        if (!alreadyStopped) 
        {
            RaycastHit hit;

            Vector3 newPoint = new Vector3(this.transform.position.x,
                                           this.transform.position.y + 5f,
                                           this.transform.position.z);

            if (Physics.Raycast(newPoint, transform.TransformDirection(Vector3.down), out hit, 500f, LayerMask.GetMask("Track")))
            {
                transform.position = hit.point;

                transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            }

            alreadyStopped = true;
        }
    }
    void MoveBack()
    {
        // movimento para trás seguindo a pista
    }
    void MoveStraightBack()
    {
        transform.position += -Vector3.right * speed * Time.deltaTime * 10;
    }
    #endregion

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
