using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridaController : MonoBehaviour
{
    [SerializeField]
    private Vector3 posicaoSpawnPlayers;
    [SerializeField]
    private Camera_Test[] cameras;
    private PlayerData[] players;

    private void Start()
    {
        LoadPlayers();
    }
    private void InstantiatePlayers()
    {
        for (int i = 0; i < players.Length; i++)
        {
            int playerCode = i + 1;
            players[i].InstancePlayer(posicaoSpawnPlayers + Vector3.forward * (i - 1), playerCode, cameras[playerCode - 1]);
        }
    }
    private void LoadPlayers()
    {

        players = GameController.gameController.playerData;

        InstantiatePlayers();
    }
}
