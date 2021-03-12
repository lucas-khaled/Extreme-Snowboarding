using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType { STRAIGHT, FOWARD, STOPPED, BACK, STRAIGHT_BACK }

[CreateAssetMenu(fileName = "FuckFriend", menuName = "Itens/Fuck Friend", order = 1)]
public class FuckFriend : Item
{
    [Header("Fuck Friend Values")]
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    private MovementType movementType;

    public override void Activate (Player player)
    {
        GameObject instantiatedProjectile = Instantiate(projectile, player.transform.position, Quaternion.identity /* Considerar a rotação do cenário!!!! */);
        Projectile proj = instantiatedProjectile.GetComponent<Projectile>();

        proj.fuckfriend = this;
        proj.caster = player;

    }
    public MovementType GetMovementType()
    {
        return movementType;
    }

}
