using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using ExtremeSnowboarding.Script.Player;
using ExtremeSnowboarding.Script.UI.Menu;
using NaughtyAttributes;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace ExtremeSnowboarding
{
    [RequireComponent(typeof(PhotonView))]
    public class Lobby : MonoBehaviourPunCallbacks
    {
        [SerializeField] [Range(1, 8)] private int maxPlayersOnRoom = 4;
        [SerializeField] [Min(2)] private int minPlayers = 2;
        //[SerializeField] [Scene] private int sceneToLoad;

        /*[Header("Escolha Controller")] [SerializeField]
        private EscolhaController escolhaController;*/
        
        public Action<bool> OnConnectedToMasterCallback { get; set; }
        public Action<bool> OnJoinedRoomCallback { get; set; }
        public Action<bool> OnCreatedRoomCallback { get; set; }
        public Action<DisconnectCause> OnDisconnectionCallback { get; set; }

        private PhotonView _photonView;
        private string sceneToLoad;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            PhotonPeer.RegisterType(typeof(PlayerData), 1, PlayerData.Serialize, PlayerData.Deserialize);
        }

        // Start is called before the first frame update
        void Start()
        {
            ConnectToPhoton();
           /* escolhaController.SetPlayers(1);
            escolhaController.ChangeLevel(sceneToLoad);*/
        }

        public void JoinOrCreateRoom(string scene)
        {
            if (PhotonNetwork.IsConnected)
            {
                sceneToLoad = scene;
                RoomOptions roomOptions = new RoomOptions();
                roomOptions.MaxPlayers = (byte)maxPlayersOnRoom;
                TypedLobby typedLobby = new TypedLobby("Test_Room", LobbyType.Default);

                PhotonNetwork.JoinOrCreateRoom("Test_Room", roomOptions, typedLobby);
            }
            else
            {
                // unconnected 
            }
        }

        [PunRPC]
        private void RPC_LoadLevel(string sceneToLoad)
        {
            PhotonNetwork.LoadLevel(sceneToLoad);
        }
        
        public override void OnJoinedRoom()
        {
            /*escolhaController.SendPlayerData();*/
            OnJoinedRoomCallback?.Invoke(true);

            #if UNITY_EDITOR
                _photonView.RPC("RPC_LoadLevel", RpcTarget.All, sceneToLoad);
                return;
            #else
            
            if (PhotonNetwork.CurrentRoom.PlayerCount >= minPlayers)
            {
                _photonView.RPC("RPC_LoadLevel", RpcTarget.All, sceneToLoad);
            }
            
            #endif
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            OnJoinedRoomCallback?.Invoke(false);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            OnJoinedRoomCallback?.Invoke(false);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            OnDisconnectionCallback?.Invoke(cause);
        }
        
        public override void OnConnectedToMaster()
        {
            OnConnectedToMasterCallback?.Invoke(true);
        }
        
        private void ConnectToPhoton()
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.AutomaticallySyncScene = true;
        }
    }
}
