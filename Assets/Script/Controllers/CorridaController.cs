using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridaController : MonoBehaviour
{
    [SerializeField]
    private Vector3 posicaoSpawnPlayers;

    private PlayerData[] players;

    private void Start()
    {
        LoadPlayers();
    }
    private void InstantiatePlayers()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].InstancePlayer(posicaoSpawnPlayers);
        }
    }
    private void LoadPlayers()
    {

        players = GameController.gameController.playerData;

        InstantiatePlayers();
    }
}
