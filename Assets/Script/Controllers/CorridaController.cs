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
    private GameCamera[] cameras;


    private PlayerData[] players;

    public static CorridaController instance { get; private set; }

    ///<summary> 
    ///Get an alive player other than the specified one 
    ///</summary>
    public Player GetOtherPlayerThan(Player player)
    {
        if (players.Length > 1)
        {
            int index = Random.Range(0, players.Length);
            Player returnPlayer = players[index].player;

            if (returnPlayer == player || returnPlayer.GetPlayerState().GetType() == typeof(Dead))
                return GetOtherPlayerThan(player);
            else
                return returnPlayer;
        }
        else
            return player;        
    }

    private void Awake()
    {
        instance = this;
    }

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
