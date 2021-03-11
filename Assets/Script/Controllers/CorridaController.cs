using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridaController : MonoBehaviour
{
    [SerializeField]
    private Vector3 posicaoSpawnPlayers;
    [SerializeField]
    private Player playerPrefab;
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
            players[i].InstancePlayer(posicaoSpawnPlayers + Vector3.forward * (i - 1), i+1, playerPrefab.gameObject, cameras[i]);
        }
    }
    private void LoadPlayers()
    {

        players = GameController.gameController.playerData;

        InstantiatePlayers();
    }
}
