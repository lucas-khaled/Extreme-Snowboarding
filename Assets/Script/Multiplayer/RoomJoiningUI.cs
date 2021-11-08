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
        private RoomVisualizationUI selectedRoom = null;

        private Lobby lobby;
        
        public void Init()
        {
            lobby = GameObject.FindObjectOfType<Lobby>();
            lobby.OnRoomListUpdateCallback += LoadRooms;
        }
        
        public void LoadRooms(List<RoomInfo> roomList)
        {
            Debug.Log("Callback room update: "+ roomList.Count);
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
                MonoBehaviour.Destroy(room.gameObject);
            
            roomObjects.Clear();
            selectRoomButton.interactable = false;
        }

        public void OnRoomSelected(RoomVisualizationUI visualization)
        {
            if (selectedRoom == visualization)
            {
                Debug.Log("Igual");
                return;
            }
            
            if(selectedRoom != null)
                selectedRoom.OnDeselected();
            
            selectedRoom = visualization;
            selectRoomButton.interactable = true;
        }

        public void JoinRoom()
        {
            lobby.JoinRoom(selectedRoom.roomName.text);
        }
    }
}
