using ExtremeSnowboarding.Multiplayer;
using ExtremeSnowboarding.Script.Controllers;
using NaughtyAttributes;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace ExtremeSnowboarding.Script.UI.Menu
{
    public class MenuUIController : MonoBehaviour
    {
        [Header("Camera Controller")]
        [SerializeField]
        private MenuCameraPointController menuCameraPointController;

        [Header("Instantiation settings")]
        [SerializeField]
        private MultiplayerInstantiationSettings instantiationSettings;

        [BoxGroup("Multiplayer")] [SerializeField]
        private RoomCreationUI roomCreationUI;

        [BoxGroup("Multiplayer")] [SerializeField]
        private RoomJoiningUI roomJoiningUI;

        [BoxGroup("Multiplayer")] [SerializeField]
        private RoomWaitingUI roomWaitingUI;

        [BoxGroup("Paineis")]
        [SerializeField] private GameObject saguaoPanel;
        [BoxGroup("Paineis")]
        [SerializeField] private GameObject criacaoPanel;
        [BoxGroup("Paineis")]
        [SerializeField] private GameObject encontrarSalaPanel;
        [BoxGroup("Paineis")]
        [SerializeField] private GameObject conectandoPanel;
        [BoxGroup("Paineis")]
        [SerializeField] private GameObject inicioPanel;
        
        [Header("Escolha Controller")]
        [SerializeField]
        private EscolhaController escolhaController;

        [Header("Lobby")] 
        [SerializeField] private Lobby lobby;

        [Header("Audio")]
        [SerializeField]
        private AudioMixer gameMixer;
        [SerializeField]
        private AudioSource audioEffectsRef;
        [SerializeField]
        private AudioClip clickAudio;
        [SerializeField]
        private AudioClip returnAudio;

        private float EffectSliderValue;
        private float MusicSliderValue;

        private void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            escolhaController.ChangeLevel(1);
            escolhaController.ChangeOverrider(instantiationSettings.GetOverriderByName("Base"));

            lobby.OnConnectedToMasterCallback += OnConnected;
            
            roomCreationUI.Init();
            roomJoiningUI.Init();
            roomWaitingUI.Init();

            float efeitoValue = GameController.gameController.GetEffectSlider();
            float musicValue = GameController.gameController.GetMusicSlider();

            if (GameController.gameController != null)
            {
                GameObject.Find("SliderMusica").GetComponent<Slider>().value = musicValue;
                GameObject.Find("SliderEfeitos").GetComponent<Slider>().value = efeitoValue;

                gameMixer.SetFloat("Effects", efeitoValue);
                gameMixer.SetFloat("SoundTrack", musicValue);
            }
            else
            {
                GameObject.Find("SliderMusica").GetComponent<Slider>().value = 0;
                GameObject.Find("SliderEfeitos").GetComponent<Slider>().value = 0;

                gameMixer.SetFloat("Effects", 0);
                gameMixer.SetFloat("SoundTrack", 0);
            }
        }

        private void OnConnected(bool connected)
        {
            if (connected)
            {
                conectandoPanel.SetActive(false);
                inicioPanel.SetActive(true);
            }
        }

        public void QuitGame()
        {
            GameController.gameController.QuitGame();
        }

        public void SetRandomRoomChoice()
        {
            roomCreationUI.SetRamdomChoice();
        }

        public void ChangeOverrider(AnimatorOverrideController overrider)
        {
            escolhaController.ChangeOverrider(overrider);
        }
        
        public void ChangeLevel(int level)
        {
            escolhaController.ChangeLevel(level);
        }

        public void ChangeAudioEffectOnMixer(string sliderName)
        {
            EffectSliderValue = GameObject.Find(sliderName).GetComponent<Slider>().value;
            gameMixer.SetFloat("Effects", EffectSliderValue);
        }
        public void ChangeAudioMusicOnMixer(string sliderName)
        {
            MusicSliderValue = GameObject.Find(sliderName).GetComponent<Slider>().value;
            gameMixer.SetFloat("SoundTrack", MusicSliderValue);
        }

        public void PressPlay()
        {
            escolhaController.SendPlayerData();
            escolhaController.SendLevel();
            GameController.gameController.SetAudio(EffectSliderValue, MusicSliderValue);
        }

        public void CreateRoom()
        {
            roomCreationUI.CreateRoom();
            criacaoPanel.gameObject.SetActive(false);
            saguaoPanel.SetActive(true);
        }

        public void JoinRoom()
        {
            roomJoiningUI.JoinRoom();
            encontrarSalaPanel.gameObject.SetActive(false);
            saguaoPanel.SetActive(true);
        }

        public void ChangeNickName(TMP_InputField input)
        {
            PhotonNetwork.LocalPlayer.NickName = input.text;
            PlayerPrefs.SetString("Nickname", input.text);
        }
    }
}
