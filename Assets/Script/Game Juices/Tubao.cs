using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ExtremeSnowboarding.Script.Player;
using ExtremeSnowboarding.Script.Controllers;
using DG.Tweening;
using Photon.Pun;

namespace ExtremeSnowboarding
{
    public class Tubao : MonoBehaviour
    {
        [SerializeField]
        private GameObject tubao;

        private GameObject tuboInstantiate;
        private Player player;


        public void InstantiateTubao()
        {
            player = CorridaController.instance.playersClassificated[0];

            tuboInstantiate = PhotonNetwork.Instantiate("TuboPivot", player.transform.position + new Vector3(0, 80, 0), new Quaternion(0, 0, 0, 1));

            tuboInstantiate.transform.DOMove(new Vector3(transform.position.x, player.transform.position.y + 3, transform.position.z), 1.5f);
        }

        public void SomeTubo()
        {
            tuboInstantiate.transform.DOMove(new Vector3(transform.position.x, player.transform.position.y + 80, transform.position.z), 3f);

            Destroy(tuboInstantiate, 3.5f);
        }
    }
}
