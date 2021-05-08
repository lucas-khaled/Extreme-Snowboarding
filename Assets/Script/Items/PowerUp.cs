using UnityEngine;

namespace Script.Items
{
    [CreateAssetMenu(fileName = "PowerUp", menuName = "Itens/PowerUp", order = 1)]
    public class PowerUp : Item
    {
        public override void Activate(Player.Player player)
        {
            StartEffects(player);
        }
    }
}
