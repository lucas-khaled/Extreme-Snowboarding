using ExtremeSnowboarding.Script.Controllers;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace ExtremeSnowboarding.Script.UI.Menu
{
    public class MenuUIController : MonoBehaviour
    {
        [SerializeField]
        private MenuCameraPointController menuCameraPointController;

        [Header("Escolha Panel")]
        [SerializeField]
        private EscolhaController escolhaController;

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

        private void Awake()
        {
            escolhaController.SetPlayers(1);
            escolhaController.ChangeLevel(1);

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

        //Daniiboy Code Start >>>>>>>>>>>>>>>>>>>>>>>>
        public void PlayReturnAnimation(string trigger)
        {
            escolhaController.SetAnimatorTriggers(trigger);
        }
        //Daniiboy Code Ends >>>>>>>>>>>>>>>>>>>>>>>>

        public void PlayerReturnAudio()
        {
            audioEffectsRef.PlayOneShot(returnAudio);
        }
        public void PlayClickAudio()
        {
            audioEffectsRef.PlayOneShot(clickAudio);
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
            GameController.gameController.Play();
        }

        public void GoNext()
        {
            menuCameraPointController.NextPoint();
        }

        public void ChangeNumOfPlayers(int num)
        {
            GameController.gameController.ChangeNumOfPlayers(num);
            escolhaController.SetPlayers(num);
        }
    }
}
