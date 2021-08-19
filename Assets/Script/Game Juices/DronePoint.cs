using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtremeSnowboarding.Script.Player;

namespace ExtremeSnowboarding.Script
{
    public class DronePoint : MonoBehaviour
    {
        [SerializeField]
        private float minHeight = 5f;

        private Vector3 destinyPosition;

        [SerializeField]
        private Player.Player firstPlayer;

        private void Update()
        {
            if (firstPlayer != null)
            {
                destinyPosition = firstPlayer.transform.position + new Vector3(3.139479f, 1.91144f, 3.7f);

                if (Physics.Raycast(destinyPosition, Vector3.down, minHeight, LayerMask.GetMask("Track")))
                    destinyPosition.y += minHeight;

                transform.position = destinyPosition;
            }
        }

        public void UpdateFirstPlayer(Player.Player player)
        {
            firstPlayer = player;
        }
    }
}
