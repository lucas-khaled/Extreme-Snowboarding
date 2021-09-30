using System;
using ExitGames.Client.Photon;
using ExtremeSnowboarding.Script.Items.Effects;
using NaughtyAttributes;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;

namespace ExtremeSnowboarding.Script.Items
{
    public abstract class Item : ScriptableObject, IOnEventCallback
    {
        [ShowAssetPreview()] [SerializeField]
        protected Sprite icon;
        
        [SerializeField] [BoxGroup("VFX Activation")] protected bool activateVFX;
        [SerializeField] [BoxGroup("VFX Activation")] [ShowIf("activateVFX")] protected string[] VFXNames;
        
        [FormerlySerializedAs("attributesToChange")] [SerializeField]
        protected Effect[] effectsToApply;
        
        private const byte startEffectEventCode = 2;

        public abstract void Activate(Player.Player player);

        public void StartEffects(Player.Player player)
        {
            Debug.Log("Doing on: "+player.name);
            foreach (Effect effect in effectsToApply)
                effect.StartEffect(player);
            
            ActivateVFX(player, true, VFXNames);
        }

        public void StartEffects(PhotonView view, Photon.Realtime.Player photonPlayer)
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
            foreach (var vfxName in names) 
            { 
                player.GetPlayerFeedbackList().GetFeedbackByName(vfxName, player.SharedValues.playerCode).StartFeedback();
            }
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
            {
                Debug.Log("IS NOT MINE");
                return;
            }
            
            StartEffects(pv.GetComponent<Player.Player>());
        }
    }
}
