using System;
using System.Collections;
using System.Collections.Generic;
using ExtremeSnowboarding.Script.Player;
using ExtremeSnowboarding.Script.VFX;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace ExtremeSnowboarding
{
    public class PlayerFeedbacksTest : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PlayerFeedbacksGroup playerFeedbacksList;
        [SerializeField] private PlayerMovimentationFeedbacks movimentationFeedbacks;

        private void Awake()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinOrCreateRoom("Test Feedbacks", new RoomOptions(),
                new TypedLobby("Teste", LobbyType.Default));
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            Debug.Log("Test");
            playerFeedbacksList.StartFeedbacks(transform, 1); 
            movimentationFeedbacks.StartFeedbacks(transform);
        }
    }
}
