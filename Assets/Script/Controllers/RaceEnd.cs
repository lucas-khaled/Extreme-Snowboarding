using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceEnd : MonoBehaviour
{
    private int quantityOfActivePlayer;

    private void Awake()
    {
        EventSystem.onPlayerDeath += OnPlayerDeath;
        quantityOfActivePlayer = GameController.gameController.GetNumberOfPlayers();
    }

    private void Update()
    {
        Debug.Log(quantityOfActivePlayer);
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
        //Finalizar a corrida :)
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuPrincipal");
        EventSystem.onPlayerDeath -= OnPlayerDeath;
    }

    private void OnTriggerEnter(Collider other)
    {
        quantityOfActivePlayer--;
        if (quantityOfActivePlayer <= 0)
        {
            EndRace();
        }
    }
}
