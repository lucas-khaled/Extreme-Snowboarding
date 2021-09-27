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

        [SerializeField] [BoxGroup("Animation")] private bool activateAnimation;
        [SerializeField] [BoxGroup("Animation")] [ShowIf("activateAnimation")] private string animation;

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

            ActivateAnimation(player, activateAnimation, animation, false);
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
                player.GetPlayerFeedbackList().GetFeedbackByName(vfxName, player.SharedValues.playerCode).StartFeedback();
            }
        }

        protected void ActivateAnimation(Player.Player player, bool checker, string name, bool caster)
        {
            if (!checker)
                return;

            string[] animations = { name };

            if (!caster)
                player.ChangeAnimationTo(animations);
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
