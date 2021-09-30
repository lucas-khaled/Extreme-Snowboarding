using System.Collections.Generic;
using ExtremeSnowboarding.Script.Controllers;
using ExtremeSnowboarding.Script.Player;
using UnityEngine;

namespace ExtremeSnowboarding.Script.UI.Menu
{
    [System.Serializable]
    public class EscolhaController
    {
        [SerializeField]
        private Transform escolhaPanel;
        [Header("Prefabs")]
        [SerializeField]
        private PlayerMenu playerChoose;
        [Header("Organization Values")]
        [SerializeField]
        private float rowSize = 4;
        private int level;

        List<PlayerMenu> playersModels = new List<PlayerMenu>();

        public void SetAnimatorTriggers(string trigger)
        {
            foreach(PlayerMenu playerMenus in playersModels)
            {
                playerMenus.animatorRenato.SetTrigger(trigger);
            }
        }

        public void SetPlayers(int numOfPlayers)
        {
            if (playersModels.Count == numOfPlayers)
                return;

            if(playersModels.Count > numOfPlayers)
            {
                for(int i = playersModels.Count-1; i >= numOfPlayers ; i--)
                {
                    GameObject player = playersModels[i].gameObject;
                    MonoBehaviour.Destroy(player);
                    playersModels.RemoveAt(i);
                }
            }
            else if(playersModels.Count < numOfPlayers)
            {
                for (int i = playersModels.Count; i < numOfPlayers; i++)
                {
                    GameObject playerGO = MonoBehaviour.Instantiate(playerChoose.gameObject, escolhaPanel);
                    PlayerMenu player = playerGO.GetComponent<PlayerMenu>();
                    playersModels.Add(player);
                }
            }

            Ordenate();

        }

        public void ChangeLevel(int level)
        {
            this.level = level;
        }

        public void SendPlayerData()
        {
            GameController.gameController.StartPlayerData(ConstructPlayerData());
        }
        public void SendLevel()
        {
            GameController.gameController.SetLevel(level - 1);
        }

        private PlayerData[] ConstructPlayerData()
        {
            PlayerData[] playersData = new PlayerData[playersModels.Count];
            int index = 0;
            foreach(PlayerMenu player in playersModels)
            {
                playersData[index] = new PlayerData(player.primaryColor, player.secondaryColor, player.changeColorShader, index, player.GetSelectedMeshes(), player.GetTexture(1), player.GetTexture(2), player.overrider);
                index++;
            }

            return playersData;
        }

        private void Ordenate()
        {
            Vector3 thisPosition = escolhaPanel.transform.position;

            if (playersModels.Count == 1)
            {
                playersModels[0].transform.position = thisPosition;
                return;
            }
            else if(playersModels.Count == 2)
            {
                playersModels[0].transform.position = thisPosition - escolhaPanel.transform.right * rowSize * 0.25f;
                playersModels[1].transform.position = thisPosition + escolhaPanel.transform.right * rowSize * 0.25f;
                return;
            }

            float offset = rowSize / (playersModels.Count-1);
            bool isOdd = playersModels.Count % 2 == 1;
        
            for (int i = 0; i<playersModels.Count; i++)
            {
            
                Vector3 newPosition = (isOdd) 
                    ? thisPosition - escolhaPanel.transform.right * (rowSize/(playersModels.Count+1)) * (i-1)
                    : thisPosition - escolhaPanel.transform.right * (rowSize*0.5f-i*offset);

                playersModels[i].transform.position = newPosition;
            }
        }
    }
}
