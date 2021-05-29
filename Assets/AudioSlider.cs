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
    }
}
