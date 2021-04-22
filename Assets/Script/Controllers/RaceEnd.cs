using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceEnd : MonoBehaviour
{
    private int quantityOfActivePlayer;
    private Player firstPlayer;

    private void Awake()
    {
        PlayerGeneralEvents.onPlayerDeath += OnPlayerDeath;
        quantityOfActivePlayer = GameController.gameController.GetNumberOfPlayers();
    }

    private void OnPlayerDeath(Player player)
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
            other.gameObject.GetComponent<Player>().ChangeState(new RaceEndState());
            quantityOfActivePlayer--;

            CorridaController.instance.PlayerFinishedRace(other.gameObject.GetComponent<Player>());
            
            if (quantityOfActivePlayer <= 0)
            {
                EndRace();
            }
        }
    }
}
