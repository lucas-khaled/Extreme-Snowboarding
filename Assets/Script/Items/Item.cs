using ExtremeSnowboarding.Script.Items.Effects;
using UnityEngine;
using UnityEngine.Serialization;

namespace ExtremeSnowboarding.Script.Items
{
    public abstract class Item : ScriptableObject
    {
        [FormerlySerializedAs("attributesToChange")] [SerializeField]
        protected Effect[] effectsToApply;

        public abstract void Activate(Player.Player player);

        public void StartEffects(Player.Player player)
        {
            foreach (Effect effect in effectsToApply)
            {
                effect.StartEffect(player);
            }
        }
    }
}
