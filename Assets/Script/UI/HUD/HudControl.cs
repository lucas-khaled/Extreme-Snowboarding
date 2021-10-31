using ExtremeSnowboarding.Script.EventSystem;
using UnityEngine;
using UnityEngine.UI;

namespace ExtremeSnowboarding.Script.UI.HUD
{
    public class HudControl : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Image refClassificationImage;
        [SerializeField]
        private Image refTurboBar;
        [SerializeField]
        private Image refFuckFriendImage;
        [SerializeField]
        private GameObject refFire;
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioSource audioSourceEffects;

        [Header ("Values")]
        [SerializeField]
        private Sprite[] classificationSprites;
        [SerializeField]
        private Sprite deadPlayerSprite;

        Player.Player player;

        private void Awake()
        {
            PlayerGeneralEvents.onPlayerPass += OnPlayerPass;
            PlayerGeneralEvents.onTurboChange += OnTurboChange;
            PlayerGeneralEvents.onItemUsed += OnFuckFriendChange;
        }

        public void SetPlayer(Player.Player player)
        {
            this.player = player;
            audioSource.Play();
        }

        private void OnPlayerPass(Player.Player player, int classification)
        {
            if (this.player == player && player.GetPlayerState().GetType() != typeof(ExtremeSnowboarding.Script.EstadosPlayer.Dead))
                refClassificationImage.sprite = classificationSprites[classification];
        }

        public void ChangeClassificationToDead()
        {
            refClassificationImage.sprite = deadPlayerSprite;
        }

        private void OnTurboChange(Player.Player player, float turboQuantity)
        {
            if (this.player == player)
            {
                float fill = turboQuantity / 1;
                refTurboBar.fillAmount = fill;
                refFire.transform.localPosition = new Vector2(fill * 312, 0);
            }
        }

        private void OnFuckFriendChange(Player.Player player, Sprite fuckFriendSprite)
        {
            if (this.player == player)
            {
                if (fuckFriendSprite != null)
                    refFuckFriendImage.sprite = fuckFriendSprite;
                else
                    refFuckFriendImage.sprite = null;
            }
        }
    }
}
