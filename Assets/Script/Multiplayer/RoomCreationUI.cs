using System.Collections;
using System.Collections.Generic;
using ExtremeSnowboarding.Script.UI.General;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExtremeSnowboarding.Multiplayer
{
    [System.Serializable]
    public class RoomCreationUI
    {
        [SerializeField] private ExtremeToggleGroup sceneToggleGroup;
        [SerializeField] private Toggle isPrivateToggle;
        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private TMP_InputField numOfPlayersInput;

        private string roomScene;

        public void CreateRoom()
        {
            Lobby lobby = GameObject.FindObjectOfType<Lobby>();
            int numOfPlayers = int.Parse(numOfPlayersInput.text);
            string sceneName = nameInput.text;
            
            lobby.CreateRoom(roomScene, numOfPlayers, isPrivateToggle.isOn, sceneName);
        }
        
        public void StarRoomCreation()
        {
            sceneToggleGroup.OnToggleChanged.AddListener(SetRoomScene);
        }

        private void SetRoomScene(string scene)
        {
            roomScene = scene;
        }
    }
}
