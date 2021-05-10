using ExtremeSnowboarding.Script.EstadosPlayer;
using ExtremeSnowboarding.Script.EventSystem;
using UnityEngine;

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
                EndRace();
            }
        }

        private void EndRace()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuPrincipal");
            PlayerGeneralEvents.onPlayerDeath -= OnPlayerDeath;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<Player.Player>().ChangeState(new RaceEndState());
                quantityOfActivePlayer--;

                CorridaController.instance.PlayerFinishedRace(other.gameObject.GetComponent<Player.Player>());
            
                if (quantityOfActivePlayer <= 0)
                {
                    EndRace();
                }
            }
        }
    }
}
