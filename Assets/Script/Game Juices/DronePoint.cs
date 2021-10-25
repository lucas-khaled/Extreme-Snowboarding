using System.Collections;
using System.Collections.Generic;
using ExtremeSnowboarding.Script.Controllers;
using UnityEngine;
using ExtremeSnowboarding.Script.Player;

namespace ExtremeSnowboarding.Script
{
    public class DronePoint : MonoBehaviour
    {
        private Player.Player firstPlayer;
        private Vector3 positionPoint;
        

        private void Update()
        {
            firstPlayer = CorridaController.instance.GetPlayerByPlace(1);
            positionPoint = firstPlayer.transform.position + new Vector3(5f, 1.9f, 3.7f);
            float velocityDrone = Vector3.Distance(transform.position, positionPoint) * 0.5f;
            float rotacaoZ = velocityDrone;

            transform.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(0,0,rotacaoZ * Mathf.Deg2Rad * 300), Time.deltaTime);
            transform.position = Vector3.Lerp(this.transform.position, positionPoint, Time.deltaTime * velocityDrone);
        }
    }
}
