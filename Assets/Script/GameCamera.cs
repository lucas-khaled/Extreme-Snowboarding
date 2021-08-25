using System.Collections;
using ExtremeSnowboarding.Script.EstadosPlayer;
using ExtremeSnowboarding.Script.UI.HUD;
using UnityEngine;

namespace ExtremeSnowboarding.Script
{
    public class GameCamera : MonoBehaviour
    {
        [SerializeField]
        private HudControl hud;

        Player.Player player;
        Vector3 offset;

        public bool shouldFollowOnlyX = false;

        private float auxY;
        private float auxX;

        public void SetInitialPlayer(Player.Player player)
        {
            this.player = player;
            player.playerCamera = this;
        
            if (player.GetPlayerState().GetType() != typeof(Dead))
                hud.SetPlayer(player);
        }

        public void SetPlayer(Player.Player player)
        {
            this.player = player;
            
            //if (player.GetPlayerState().GetType() != typeof(Dead))
            //    hud.SetPlayer(player);
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (player != null)
            {
                Vector3 destiny = Vector3.zero;

                if (!shouldFollowOnlyX)
                {
                    destiny = player.transform.position;
                    auxY = player.transform.position.y;
                    auxX = player.transform.position.x;
                }
                else
                {
                    if (auxX <= player.transform.position.x)
                        destiny.x = player.transform.position.x;
                    else
                        destiny.x = auxX;

                    if (auxY <= player.transform.position.y)
                        destiny.y = player.transform.position.y;
                    else
                        destiny.y = auxY;
                }

                /*else if (auxX <= player.transform.position.x && auxY <= player.transform.position.y)
                    destiny = new Vector3(auxX, auxY, player.transform.position.z);
                else
                    destiny = transform.parent.position;
                */
                transform.parent.transform.position = Vector3.Lerp(transform.parent.transform.position, destiny, 0.8f);
            }
        }

        public void ChangeClassificationToDead()
        {
            hud.ChangeClassificationToDead();
        }

        public IEnumerator CameraShake(bool deactivateCameraShake, float shakingDuration, float magnetude)
        {
            if (!deactivateCameraShake)
            {
                Vector3 originalPoisition = this.transform.localPosition;

                float timeElapsed = 0f;

                while (timeElapsed < shakingDuration)
                {
                    float x = Random.Range(-1f, 1f) * magnetude;
                    float y = Random.Range(-1f, 1f) * magnetude;

                    this.transform.localPosition = new Vector3(x + originalPoisition.x,
                        y + originalPoisition.y,
                        originalPoisition.z);

                    timeElapsed += Time.deltaTime;

                    yield return null;
                }

                this.transform.localPosition = originalPoisition;
            }
        }
    }
}
