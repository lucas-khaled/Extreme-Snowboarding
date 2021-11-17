using System;
using System.Linq;
using ExitGames.Client.Photon;
using ExtremeSnowboarding.Script.Items.Effects;
using NaughtyAttributes;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace ExtremeSnowboarding.Script.Items
{
    public abstract class Item : ScriptableObject, IOnEventCallback
    {
        [ShowAssetPreview()] [SerializeField]
        protected Sprite icon;
        
        [SerializeField] [BoxGroup("VFX Activation")] protected bool activateVFX;
        [SerializeField] [BoxGroup("VFX Activation")] [ShowIf("activateVFX")] protected string[] VFXNames;

        [SerializeField] [BoxGroup("Animation")] private bool activateAnimation;
        [SerializeField] [BoxGroup("Animation")] [ShowIf("activateAnimation")] private string animation;

        [FormerlySerializedAs("attributesToChange")] [SerializeField]
        protected Effect[] effectsToApply;
        
        private const byte startEffectEventCode = 2;

        public abstract void Activate(Player.Player player);

        public void StartEffects(Player.Player player)
        {
            if (player.PhotonView.IsMine)
            {
                foreach (Effect effect in effectsToApply)
                    effect.StartEffect(player);
            }
            
            ActivateAnimation(player, activateAnimation, animation);
            ActivateVFX(player, true, VFXNames);
            
        }
        
        public void StartEffects(PhotonView view)
        {
            object[] passObject = { view.ViewID, name };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others,
            };

            PhotonNetwork.RaiseEvent(startEffectEventCode, passObject, raiseEventOptions, SendOptions.SendReliable);
        }

        public Sprite GetSprite()
        {
            return icon;
        }
        
        protected void ActivateVFX(Player.Player player, bool checker, string[] names)
        {
            if(!checker)
                return;
            
            Debug.Log("VFX vai");
            foreach (var vfxName in names) 
            { 
                Debug.Log("VFX NAME: "+vfxName);
                player.GetPlayerFeedbackList().GetFeedbackByName(vfxName, player.SharedValues.playerCode).StartFeedback();
            }
        }

        protected void ActivateAnimation(Player.Player player, bool checker, string name)
        {
            if (!checker)
                return;

            string[] animations = { name };

            player.ChangeAnimationTo(animations);

        }

        private void OnEnable()
        {
            foreach (var effect in effectsToApply)
            {
                effect.OnEnable();
            }
            
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnEvent(EventData photonEvent)
        {
            if(photonEvent.Code != startEffectEventCode) return;
            
            object[] data = (object[])photonEvent.CustomData;
            if((string)data[1] != name) return;
            
            PhotonView pv = PhotonView.Find((int) data[0]);
            if (!pv.IsMine)
                return;

            StartEffects(pv.GetComponent<Player.Player>());
        }
    }
}
