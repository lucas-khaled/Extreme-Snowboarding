using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public enum MovementType { STRAIGHT, FOWARD, STOPPED, BACK, STRAIGHT_BACK }

[CreateAssetMenu(fileName = "FuckFriend", menuName = "Itens/Fuck Friend", order = 1)]
public class FuckFriend : Item
{
    [BoxGroup("Fuck Friend Values")]
    [SerializeField] [ShowAssetPreview()] 
    GameObject projectile;
    [SerializeField] [BoxGroup("Fuck Friend Values")]
    private MovementType movementType;

    public override void Activate (Player player)
    {
        GameObject instantiatedProjectile = Instantiate(projectile, player.transform.position, Quaternion.identity /* Considerar a rota��o do cen�rio!!!! */);
        Projectile proj = instantiatedProjectile.GetComponent<Projectile>();

        proj.fuckfriend = this;
        proj.caster = player;

    }
    public MovementType GetMovementType()
    {
        return movementType;
    }

}
