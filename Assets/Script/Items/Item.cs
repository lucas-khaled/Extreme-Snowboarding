using System;
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
        
        [SerializeField] [BoxGroup("VFX Activation")] protected bool activateVFX;
        [SerializeField] [BoxGroup("VFX Activation")] [ShowIf("activateVFX")] protected string[] VFXNames;
        
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
            ActivateVFX(player, true, VFXNames);
        }

        public Sprite GetSprite()
        {
            return icon;
        }
        
        protected void ActivateVFX(Player.Player player, bool checker, string[] names)
        {
            if(!checker)
                return;
            foreach (var vfxName in names) 
            { 
                player.GetPlayerVFXList().GetVFXByName(vfxName, player.SharedValues.playerCode).StartParticle();
            }
        }

        private void OnEnable()
        {
            foreach (var effect in effectsToApply)
            {
                effect.OnEnable();
            }
        }
    }
}
