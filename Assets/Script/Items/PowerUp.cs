using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Itens/PowerUp", order = 1)]
public class PowerUp : Item
{
    public override void Activate(Player player)
    {
        StartEffects(player);
    }
}
