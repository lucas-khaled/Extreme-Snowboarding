using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType { RIGHT, STOPPED, LEFT }

public class FuckFriend : Item
{
    GameObject projectile;
    private MovementType movementType;

    public override void Activate (Player player)
    {
        GameObject instantiatedProjectile = Instantiate(projectile, player.transform.position, Quaternion.identity /* Considerar a rotação do cenário!!!! */); 
    }
    public Effect[]  GetAttributesToChange()
    {
        return atributesToChange;
    }
    public MovementType GetMovementType()
    {
        return movementType;
    }
}
