using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeSnowboarding
{
    public class FlyingObject : MonoBehaviour
    {
        private GameObject[] players;

        private void Start()
        {
            players = GameObject.FindGameObjectsWithTag("Player");
        }

        private void Update()
        {
            float media = 0;
            media = CalculateMedia();

            float speed = (media - this.transform.position.x) * 0.25f;
            float velocidade = Mathf.Clamp(speed, 10, 40);

            this.gameObject.GetComponent<PathFollower>().speed = velocidade;
        }

        private float CalculateMedia()
        {
            float media = 0;

            foreach (GameObject player in players)
            {
                media += player.transform.position.x;
            }

            return media /= players.Length;
        }
    }
}
