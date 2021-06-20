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
            }
        }
    }
}
