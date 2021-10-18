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
        private PlayerMenu playerChoose;

        private int level;

        public void SetAnimatorTriggers(string trigger)
        {
            playerChoose.animatorRenato.SetTrigger(trigger);
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

        private PlayerData ConstructPlayerData()
        {
            PlayerData playersData = new PlayerData(playerChoose.primaryColor, playerChoose.secondaryColor, playerChoose.GetSelectedMeshesNames());
            return playersData;
        }
    }
}
