using ExtremeSnowboarding.Script.UI.Menu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeSnowboarding
{
    public class ScriptTosco : MonoBehaviour
    {
        [SerializeField]
        private Animator animatorPlayer;

        [SerializeField] private GameObject[] players;

        [SerializeField] private GameObject tubarones;

        [SerializeField] private GameObject homenos;

        [SerializeField] private MenuCameraPointController cameraai;

        private bool rodou = false;

        private void Start()
        {
            StartCoroutine(tosquisse());
            rodou = true;

        }

        private IEnumerator tosquisse()
        {
            yield return new WaitForSeconds(10f);


            yield return new WaitForSeconds(5f);
            animatorPlayer.SetTrigger("gettingUp");
            cameraai.GoToPointByTag("Sala");

            yield return new WaitForSeconds(9.2f);
            players[0].SetActive(true);

            yield return new WaitForSeconds(0.3f);
            players[1].SetActive(true);

            yield return new WaitForSeconds(0.5f);
            players[2].SetActive(true);

            yield return new WaitForSeconds(0.6f);
            players[3].SetActive(true);

            yield return new WaitForSeconds(0.5f);
            homenos.SetActive(true);
            yield return new WaitForSeconds(0.8f);
            tubarones.SetActive(true);

        
        }

    }
}
