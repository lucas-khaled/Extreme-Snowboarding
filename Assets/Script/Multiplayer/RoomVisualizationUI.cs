using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExtremeSnowboarding.Script.Multiplayer
{
    [RequireComponent(typeof(Button))]
    public class RoomVisualizationUI : MonoBehaviour
    {
        public TMP_Text roomName;
        [SerializeField] private TMP_Text capacity;
        [SerializeField] private TMP_Text ping;

        public RoomInfo roomInfo { get; private set; }

        private Action<string> onRoomSelected;
        private Button roomButton;

        public void SetRoom(RoomInfo info, Action<string> roomSelected)
        {
            roomInfo = info;
            
            roomName.SetText(info.Name);
            capacity.SetText(info.PlayerCount + " | " +info.MaxPlayers);
            ping.SetText(PhotonNetwork.GetPing().ToString());

            onRoomSelected = roomSelected;

            roomButton = GetComponent<Button>();
            roomButton.onClick.AddListener(delegate { OnRoomButtonClicked(); });
        }

        private void OnRoomButtonClicked()
        {
            onRoomSelected.Invoke(roomName.text);
        }
    }
}