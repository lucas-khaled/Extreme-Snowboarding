using System.Collections;
using System.Collections.Generic;
using ExtremeSnowboarding.Script.Controllers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace ExtremeSnowboarding
{
    public class AudioSlider : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField]
        private AudioMixer gameMixer;
        [SerializeField]
        private Slider effectsSlider;
        [SerializeField]
        private Slider musicSlider;

        private void Start()
        {
            Debug.Log(GameController.gameController.GetEffectSlider() +"\n Music: "+GameController.gameController.GetMusicSlider());
            effectsSlider.value = GameController.gameController.GetEffectSlider();
            musicSlider.value = GameController.gameController.GetMusicSlider();

            gameMixer.SetFloat("SoundTrack", musicSlider.value);
            gameMixer.SetFloat("Effects", effectsSlider.value);
        }

        public void OnEffectValueChange()
        {
            gameMixer.SetFloat("Effects", effectsSlider.value);
        }
        public void OnMusicValueChange()
        {
            gameMixer.SetFloat("SoundTrack", musicSlider.value);
        }

        public void SendAudioSettings()
        {
            GameController.gameController.SetAudio(effectsSlider.value, musicSlider.value);
        }
    }
}
