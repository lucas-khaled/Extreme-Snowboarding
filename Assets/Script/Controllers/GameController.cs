using ExtremeSnowboarding.Script.Player;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ExtremeSnowboarding.Script.Controllers
{
    public class GameController : MonoBehaviour
    {
        public static GameController gameController;
        
        public string sceneToLoad { get; private set; }
        private float effectAudioLevel;
        private float musicAudioLevel;


        [Scene] [SerializeField] 
        private string[] scenesToLoad;

        public PlayerData playerData { get; set; }

        private void Awake()
        {
            sceneToLoad = "None";

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

        public void StartPlayerData(PlayerData players)
        {
            playerData = players;
        }

        public void SetLevel(int level)
        {
            sceneToLoad = scenesToLoad[level];
        }

        public void SetAudio(float effectAudioLevel, float musicAudioLevel)
        {
            this.effectAudioLevel = effectAudioLevel;
            this.musicAudioLevel = musicAudioLevel;
        }
        public float GetEffectSlider()
        {
            return effectAudioLevel;
        }
        public float GetMusicSlider()
        {
            return musicAudioLevel;
        }

        public void ChangeScene(string cena)    
        {
            SceneManager.LoadScene(cena);
        }
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
