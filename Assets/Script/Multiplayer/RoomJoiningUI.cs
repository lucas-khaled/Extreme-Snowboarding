using System.Collections;
using System.Collections.Generic;
using ExtremeSnowboarding.Script.Multiplayer;
using ExtremeSnowboarding.Script.UI.General;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace ExtremeSnowboarding.Multiplayer
{
    [System.Serializable]
    public class RoomJoiningUI
    {
        [SerializeField] private RoomVisualizationUI roomPrefabUI;
        [SerializeField] private Transform roomContent;
        [SerializeField] private Button selectRoomButton;

        private List<RoomVisualizationUI> roomObjects = new List<RoomVisualizationUI>();
        private string selectedRoom;

        private Lobby lobby;
        
        public void InitRoomJoining()
        {
            lobby = GameObject.FindObjectOfType<Lobby>();
            lobby.OnRoomListUpdateCallback += LoadRooms;
        }
        
        public void LoadRooms(List<RoomInfo> roomList)
        {
            ClearRooms();
            
            foreach (var roomInfo in roomList)
            {
                RoomVisualizationUI roomVisual = MonoBehaviour.Instantiate(roomPrefabUI, roomContent);
                roomVisual.SetRoom(roomInfo, OnRoomSelected);
                roomObjects.Add(roomVisual);
            }
        }
        
        public void ClearRooms()
        {
            foreach (var room in roomObjects)
                MonoBehaviour.Destroy(room);
            
            roomObjects.Clear();
            selectRoomButton.interactable = false;
        }

        public void OnRoomSelected(string roomName)
        {
            this.selectedRoom = roomName;
            selectRoomButton.interactable = true;
        }

        public void JoinRoom()
        {
            
            lobby.JoinRoom(selectedRoom);
        }
    }
}
