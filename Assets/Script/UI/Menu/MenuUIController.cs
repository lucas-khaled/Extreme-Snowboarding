using ExtremeSnowboarding.Script.Controllers;
using UnityEngine;

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
        private AudioSource audioEffectsRef;
        [SerializeField]
        private AudioClip clickAudio;
        [SerializeField]
        private AudioClip returnAudio;

        private void Awake()
        {
            escolhaController.SetPlayers(1);
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

        public void PressPlay()
        {
            escolhaController.SendPlayerData();
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
