using ExtremeSnowboarding.Script.EstadosPlayer;
using ExtremeSnowboarding.Script.EventSystem;
using UnityEngine;
using System.Collections;

namespace ExtremeSnowboarding.Script.Controllers
{
    public class RaceEnd : MonoBehaviour
    {
        private int quantityOfActivePlayer;
        private Player.Player firstPlayer;

        [SerializeField]
        private GameObject[] confettes;

        private void Awake()
        {
            PlayerGeneralEvents.onPlayerDeath += OnPlayerDeath;
            quantityOfActivePlayer = GameController.gameController.GetNumberOfPlayers();
        }

        private void OnPlayerDeath(Player.Player player)
        {
            quantityOfActivePlayer--;
            if (quantityOfActivePlayer <= 0)
            {
                StartCoroutine(EndRace());
            }
        }

        private IEnumerator EndRace()
        {
            yield return new WaitForSeconds(10);
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuPrincipal");
            PlayerGeneralEvents.onPlayerDeath -= OnPlayerDeath;
        }

        private IEnumerator Confette()
        {
            foreach(GameObject confete in confettes)
            {
                confete.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<Player.Player>().ChangeState(new RaceEndState());
                quantityOfActivePlayer--;

                if (quantityOfActivePlayer <= 0)
                {
                    StartCoroutine(EndRace());
                }

                if (CorridaController.instance.playersClassificated[0] == other.GetComponent<Player.Player>())
                {
                    StartCoroutine(Confette());
                }
            }
        }
    }
}
