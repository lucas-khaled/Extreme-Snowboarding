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
using Player = Photon.Realtime.Player;
using Random = System.Random;

namespace ExtremeSnowboarding.Multiplayer
{
    [RequireComponent(typeof(PhotonView))]
    public class Lobby : MonoBehaviourPunCallbacks
    {
        [SerializeField] [Min(2)] private int minPlayers = 2;

        public Action<List<RoomInfo>> OnRoomListUpdateCallback { get; set; }
        public Action<bool> OnConnectedToMasterCallback { get; set; }
        public Action<bool> OnJoinedRoomCallback { get; set; }
        public Action<bool> OnCreatedRoomCallback { get; set; }
        public Action<DisconnectCause> OnDisconnectionCallback { get; set; }
        public Action OnPlayersInRoomUpdateCallback { get; set; }
        public Action OnLeftRoomCallback { get; set; }

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

        public void JoinRoom(string roomName)
        {
            if(PhotonNetwork.IsConnected)
                PhotonNetwork.JoinRoom(roomName);
        }

        public void CreateRoom(string scene, int maxPlayers, bool isPrivate, string roomName)
        {
            if (PhotonNetwork.IsConnected)
            {
                RoomOptions roomOptions = new RoomOptions()
                {
                    MaxPlayers = (byte) maxPlayers,
                    IsVisible = !isPrivate
                };

                sceneToLoad = scene;
                
                TypedLobby typedLobby = new TypedLobby("Geral", LobbyType.Default);
                PhotonNetwork.CreateRoom(roomName, roomOptions, typedLobby);
            }
            
        }

        public void LeaveRoom()
        {
            if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
        }

        public void JoinOrCreateRoom()
        {
            if (PhotonNetwork.IsConnected)
                PhotonNetwork.JoinRandomRoom();
            else
            {
                // unconnected 
            }
        }

        public void StartGame()
        {
            _photonView.RPC("RPC_LoadLevel", RpcTarget.All, sceneToLoad);
        }

        [PunRPC]
        private void RPC_LoadLevel(string sceneToLoad)
        {
            GetComponent<MenuUIController>().PressPlay();
            PhotonNetwork.LoadLevel(sceneToLoad);
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            OnLeftRoomCallback?.Invoke();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            OnPlayersInRoomUpdateCallback?.Invoke();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            OnPlayersInRoomUpdateCallback?.Invoke();
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            Debug.Log("Criei salinha bixo");
            OnCreatedRoomCallback?.Invoke(true);
        }

        public override void OnJoinedRoom()
        {
            OnJoinedRoomCallback?.Invoke(true);
            
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
               StartGame();
            }
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            OnJoinedRoomCallback?.Invoke(false);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            CreateRoom("Fase1", 4, false, ("Sala: "+(PhotonNetwork.CountOfRooms + 1)));
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
            PhotonNetwork.JoinLobby( new TypedLobby("Geral", LobbyType.Default));
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            Debug.Log("Conected and joined "+PhotonNetwork.CurrentLobby.Name);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("Room Updated: "+PhotonNetwork.CurrentLobby.Name);
            base.OnRoomListUpdate(roomList);
            OnRoomListUpdateCallback?.Invoke(roomList);
        }

        private void ConnectToPhoton()
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.AutomaticallySyncScene = true;
        }
    }
}
