using System.Collections;
using ExtremeSnowboarding.Script.EstadosPlayer;
using ExtremeSnowboarding.Script.UI.HUD;
using UnityEngine;
using System.Collections.Generic;

namespace ExtremeSnowboarding.Script
{
    public class GameCamera : MonoBehaviour
    {
        [SerializeField]
        private HudControl hud;


        Player.Player player;
        Vector3 offset;


        public void SetPlayer(Player.Player player)
        {
            this.player = player;
            player.playerCamera = this;

            if (player.GetPlayerState().GetType() != typeof(Dead))
                hud.SetPlayer(player);
        }

        public void ChangeDeadPlayerCamera(Player.Player playerSpectated)
        {
            Player.Player auxPlayer = player;

            Player.Player[] playersSpectating = auxPlayer.GetPlayerSpectators();

            for (int i = 0; i < playersSpectating.Length; i++)
            {
                if (playersSpectating[i] != null)
                {
                    //Debug.Log(playersSpectating[i].name);
                    playersSpectating[i].playerCamera.ChangePlayerView(playerSpectated, playersSpectating[i]);
                    playerSpectated.AddPlayerSpectating(playersSpectating[i]);
                    this.player.RemovePlayerSpectating(playersSpectating[i]);
                }
            }

            player.playerCamera = playerSpectated.playerCamera;
            player = playerSpectated;

            playerSpectated.AddPlayerSpectating(auxPlayer);
                       
        }

        public void ChangePlayerView(Player.Player playerSpectated, Player.Player spec)
        {
            //Debug.Log("Este player: " + this.player.name);
            //Debug.Log(spec.name + " + " + playerSpectated.name);

            //spec.playerCamera.gameObject.SetActive(false);
            //player.playerCamera.gameObject.SetActive(false);

            spec.playerCamera = playerSpectated.playerCamera;
            spec = playerSpectated;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            Debug.Log(player.name);
            if (player != null)
                transform.parent.transform.position = player.transform.position;
            else
                Debug.Log("Player nulo wtf");
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
