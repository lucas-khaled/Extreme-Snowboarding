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
        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private ExtremeToggleGroup numOfPlayersToggle;

        private string roomScene = "Fase1";
        private int qntPlayer = 2;

        public void CreateRoom()
        {
            Lobby lobby = GameObject.FindObjectOfType<Lobby>();
            int numOfPlayers = qntPlayer;
            string sceneName = nameInput.text;
            
            lobby.CreateRoom(roomScene, numOfPlayers, false, sceneName);
        }
        
        public void Init()
        {
            sceneToggleGroup.OnToggleChanged.AddListener(SetRoomScene);
            numOfPlayersToggle.OnToggleChanged.AddListener(SetQuantityOfPlayers);
        }

        private void SetRoomScene(string scene)
        {
            Debug.Log("Scene NAme: "+scene);
            roomScene = scene;
        }

        private void SetQuantityOfPlayers(string qntPlayers)
        {
            Debug.Log("Toggle qnt of players: "+qntPlayers);
            this.qntPlayer = int.Parse(qntPlayers);
        }
    }
}
