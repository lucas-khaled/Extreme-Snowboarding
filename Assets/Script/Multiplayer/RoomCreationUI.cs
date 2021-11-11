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
        [SerializeField] private Toggle randomRoomToggle;
        [SerializeField] private Toggle startWhenFullToggle;
        [SerializeField] private GameObject randomChoicePanel;

        private string roomScene = "Fase1";
        private int qntPlayer = 2;

        public void CreateRoom()
        {
            Lobby lobby = GameObject.FindObjectOfType<Lobby>();
            int numOfPlayers = qntPlayer;
            string sceneName = nameInput.text;

            if (randomRoomToggle.isOn)
                roomScene = lobby.gameScenes.GetRandomScene();

            lobby.StartWhenFull = startWhenFullToggle.isOn;
            lobby.CreateRoom(roomScene, numOfPlayers, false, sceneName);
        }
        
        public void Init()
        {
            sceneToggleGroup.OnToggleChanged.AddListener(SetRoomScene);
            numOfPlayersToggle.OnToggleChanged.AddListener(SetQuantityOfPlayers);
        }

        public void SetRamdomChoice()
        {
            randomChoicePanel.SetActive(randomRoomToggle.isOn);
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
