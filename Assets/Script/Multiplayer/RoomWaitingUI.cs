using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExtremeSnowboarding.Multiplayer
{
    [System.Serializable]
    public class RoomWaitingUI
    {
        [SerializeField] private TMP_Text roomNameText;
        [SerializeField] private TMP_Text[] playersInRoomTexts;
        [SerializeField] private TMP_InputField changeNickInput;
        [SerializeField] private Button startGameButton;

        private bool started = false;
        private Lobby lobby;

        public void Init()
        {
            lobby = GameObject.FindObjectOfType<Lobby>();
            lobby.OnJoinedRoomCallback += Start;
            lobby.OnPlayersInRoomUpdateCallback += UpdatePlayerList;
            lobby.OnLeftRoomCallback += Quit;
        }

        public void Start(bool joined)
        {
            if(!joined) return;
            
            started = true;
            startGameButton.gameObject.SetActive(PhotonNetwork.LocalPlayer.IsMasterClient);
            startGameButton.interactable = false;
            
            roomNameText.SetText(PhotonNetwork.CurrentRoom.Name);
            changeNickInput.SetTextWithoutNotify(PhotonNetwork.LocalPlayer.NickName);
            UpdatePlayerList();
        }

        public void UpdatePlayerList()
        {
            if(!started) return;
            
            Player[] players = PhotonNetwork.CurrentRoom.Players.Values.ToArray();
            
            int index = 0;
            foreach (var player in players)
            {
                playersInRoomTexts[index].SetText(player.NickName);
                index++;
            }

            if (index > 1 && PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                startGameButton.interactable = false;
            }

            if (index < playersInRoomTexts.Length-1)
            {
                for (int i = index; i < playersInRoomTexts.Length; i++)
                    playersInRoomTexts[i].SetText(" - ");
            }
        }
        
        private void Quit()
        {
            started = false;
            startGameButton.interactable = false;
            
            roomNameText.SetText(string.Empty);
        }
    }
}
