using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridaController : MonoBehaviour
{
    [SerializeField]
    private Vector3 posicaoSpawnPlayers;
    [SerializeField]
    private Player playerPrefab;


    private PlayerData[] players;

    private void Start()
    {
        LoadPlayers();
    }
    private void InstantiatePlayers()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].InstancePlayer(posicaoSpawnPlayers, playerPrefab.gameObject);
        }
    }
    private void LoadPlayers()
    {

        players = GameController.gameController.playerData;

        InstantiatePlayers();

        Camera.main.GetComponent<Camera_Test>().SetPlayer(players[0].player);
    }
}
