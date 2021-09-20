using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtremeSnowboarding.Script.Player;

namespace ExtremeSnowboarding.Script
{
    public class DronePoint : MonoBehaviour
    {
        private Player.Player firstPlayer;
        private Vector3 positionPoint;

        public void UpdateFirstPlayer(Player.Player player)
        {
            firstPlayer = player;
        }

        private void Update()
        {
            positionPoint = firstPlayer.transform.position + new Vector3(5f, 1.9f, 3.7f);
            float velocityDrone = Vector3.Distance(transform.position, positionPoint) * 0.5f;
            float rotacaoZ = velocityDrone;

            Debug.Log(rotacaoZ * Mathf.Deg2Rad * 300);

            transform.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(0,0,rotacaoZ * Mathf.Deg2Rad * 300), Time.deltaTime);
            transform.position = Vector3.Lerp(this.transform.position, positionPoint, Time.deltaTime * velocityDrone);
        }
    }
}
