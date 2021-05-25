using ExtremeSnowboarding.Script.Player;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ExtremeSnowboarding.Script.Controllers
{
    public class GameController : MonoBehaviour
    {
        public static GameController gameController;

        private int numOfPlayer = 1;

        [Scene] [SerializeField] 
        private string[] scenesToLoad;

        public PlayerData[] playerData { get; set; }

        private void Awake()
        {
            if (gameController == null)
            {
                gameController = this;
                DontDestroyOnLoad(this);
            }
            else if (GameController.gameController != this)
            {
                Destroy(this);
            }
        }
    
        public void StartPlayerData(PlayerData[] players)
        {
            playerData = players;
        }

        public void ChangeNumOfPlayers(int num)
        {
            numOfPlayer = num;
        }
        public int GetNumberOfPlayers()
        {
            return numOfPlayer;
        }

        public void Play()
        {
            string sceneToLoad = scenesToLoad[Random.Range(0, scenesToLoad.Length)];
            ChangeScene(sceneToLoad);
        }

        public void ChangeScene(string cena)    
        {
            SceneManager.LoadScene(cena);
        }
        public void SairDoJogo()
        {
            Application.Quit();
        }
    }
}
