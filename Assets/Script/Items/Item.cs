using ExtremeSnowboarding.Script.Items.Effects;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace ExtremeSnowboarding.Script.Items
{
    public abstract class Item : ScriptableObject
    {
        [ShowAssetPreview()] [SerializeField]
        protected Sprite icon;
        
        [FormerlySerializedAs("attributesToChange")] [SerializeField]
        protected Effect[] effectsToApply;

        public abstract void Activate(Player.Player player);

        public void StartEffects(Player.Player player)
        {
            foreach (Effect effect in effectsToApply)
            {
                Debug.Log(player.name);
                effect.StartEffect(player);
            }
        }

        public Sprite GetSprite()
        {
            return icon;
        }
    }
}
