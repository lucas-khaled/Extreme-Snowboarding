using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using ExtremeSnowboarding.Script.Items;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace ExtremeSnowboarding
{
    public class NetInstantiationInfo : MonoBehaviour, IOnEventCallback
    {
        public static NetInstantiationInfo Instance { get; private set; }

        [SerializeField] private List<FuckFriend> fuckFriendsList;
        private const byte FuckFriendProjectileCode = 4;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            
        }
        
        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void FuckFriendInstantiationInfo(FuckFriend fuck, Projectile proj)
        {
            object[] data = 
            {
                fuck.name, proj.name
            };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others,
            };

            SendOptions sendOptions = new SendOptions
            {
                Reliability = true
            };

            PhotonNetwork.RaiseEvent(FuckFriendProjectileCode, data, raiseEventOptions, sendOptions);
        }


        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code != FuckFriendProjectileCode) return;

            object[] data = (object[]) photonEvent.CustomData;
            string ffName = data[0] as string;
            string projName = data[1] as string;
            
            FuckFriend fuck = fuckFriendsList.Find(x => x.name == ffName);
            Projectile proj = GameObject.Find(projName).GetComponent<Projectile>();
            proj.fuckfriend = fuck;
        }
    }
}
