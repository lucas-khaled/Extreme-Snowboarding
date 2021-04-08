using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridaController : MonoBehaviour
{
    [SerializeField]
    private Vector3 posicaoSpawnPlayers;
    [SerializeField]
    private Player playerPrefab;

    [HideInInspector]
    public GameCamera[] cameras;
    [HideInInspector]
    public PlayerData[] players;
    [HideInInspector]
    public List<Player> playersClassificated;

    int alivePlayers;

    public static CorridaController instance { get; private set; }

    ///<summary> 
    ///Get an alive player other than the specified one 
    ///</summary>
    public Player GetOtherPlayerThan(Player player)
    {
        if (alivePlayers > 1)
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
        EventSystem.onPlayerPass += OnPlayerPass;
        EventSystem.onPlayerDeath += OnPlayerDeath;
        instance = this;
    }

    #region Listeners

    private void OnPlayerDeath(Player player)
    {
        alivePlayers--;
    }

    private void OnPlayerPass(Player player)
    {

    }

    #endregion

    private void Start()
    {
        LoadPlayers();
        InvokeRepeating("CheckPlayerClassification",0,0.5f);
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
        alivePlayers = players.Length;
        InstantiatePlayers();
    }

    private void CheckPlayerClassification()
    {
        bool changed = false;
        PlayerData playerChanged = null;

        for (int i = 0; i < players.Length - 1; i++)
        {
            float distanceXPlayer1 = players[i].player.transform.position.x;
            float distanceXPlayer2 = players[i + 1].player.transform.position.x;

            if (distanceXPlayer1 < distanceXPlayer2)
            {
                PlayerData changePlayerAux = players[i];
                playerChanged = players[i];
                players[i] = players[i + 1];
                players[i + 1] = changePlayerAux;
                changed = true;
            }
        }
        if (changed)
        {
            if (EventSystem.onPlayerPass != null && playerChanged != null)
                EventSystem.onPlayerPass.Invoke(playerChanged.player);

            changed = false;
        }
    }
    public void PlayerFinishedRace(Player player)
    {
        playersClassificated.Add(player);
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(posicaoSpawnPlayers, "snowboard_icon.png");
    }
}
