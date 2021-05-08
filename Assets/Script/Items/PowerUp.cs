using UnityEngine;

namespace ExtremeSnowboarding.Script.Items
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
