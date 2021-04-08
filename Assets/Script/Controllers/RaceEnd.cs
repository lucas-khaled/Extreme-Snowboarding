using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceEnd : MonoBehaviour
{
    private int quantityOfActivePlayer;
    private Player playerWon;

    private void Awake()
    {
        EventSystem.onPlayerDeath += OnPlayerDeath;
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

    private void EndRace(Player player = null)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuPrincipal");
        EventSystem.onPlayerDeath -= OnPlayerDeath;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            quantityOfActivePlayer--;
            Player player = other.gameObject.GetComponent<Player>();

            if (playerWon == null)
            {
                playerWon = player;
                player.SetOnAnimator("wonRace", true);
            }
            else
                player.SetOnAnimator("lostRace", true);

            if (quantityOfActivePlayer <= 0)
                EndRace(player);
        }
    }
}
