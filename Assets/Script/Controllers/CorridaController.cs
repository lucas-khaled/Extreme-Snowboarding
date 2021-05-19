using System.Collections.Generic;
using ExtremeSnowboarding.Script.EstadosPlayer;
using ExtremeSnowboarding.Script.EventSystem;
using ExtremeSnowboarding.Script.Player;
using UnityEngine;

namespace ExtremeSnowboarding.Script.Controllers
{
    public class CorridaController : MonoBehaviour
    {
        [SerializeField]
        private Vector3 posicaoSpawnPlayers;
        [SerializeField]
        private Player.Player playerPrefab;

        public GameCamera[] cameras;
        public GameObject catastrophe { get; set; }
        
        public List<Player.Player> playersClassificated { get; private set; }
        
        private PlayerData[] players;
        
        
        int alivePlayers;

        public static CorridaController instance { get; private set; }

        /// <summary>
        /// Return the total of player that started the game
        /// </summary>
        /// <returns></returns>
        public int GetPlayersInGameCount()
        {
            return players.Length;
        }
        
        ///<summary> 
        ///Get an alive player other than the specified one 
        ///</summary>
        public Player.Player GetOtherPlayerThan(Player.Player player)
        {
            if (alivePlayers > 1)
            {
                int index = Random.Range(0, players.Length);
                Player.Player returnPlayer = players[index].player;

                if (returnPlayer == player || returnPlayer.GetPlayerState().GetType() == typeof(Dead))
                    return GetOtherPlayerThan(player);
                else
                    return returnPlayer;
            }
            else
                return player;
        }

        /// <summary>
        /// Get the specific player classification at the moment.
        /// </summary>
        /// <param name="player"> The player who you wants to know the place he is. </param>
        /// <returns> It will return +1 the index of the player array. If is not possible to find the player, it will return -1. </returns>
        public int GetPlayerPlace(Player.Player player)
        {
            int index = 1;
            
            foreach (PlayerData p in players)
            {
                if (p.player == player)
                    return index;
                index++;
            }
            
            return -1;
        }

        /// <summary>
        /// Get the player who is at the specified classification at the moment.
        /// </summary>
        /// <param name="place"> This value will be clamped between the first and the last race position. </param>
        /// <returns> The player at this place. </returns>
        public Player.Player GetPlayerByPlace(int place)
        {
            int realPlace = Mathf.Clamp(place, 1, players.Length);
            return players[realPlace-1].player;
        }
        
        /// <summary>
        /// Set a player to finish the race
        /// </summary>
        /// <param name="player"> The player who finished the race </param>
        public void PlayerFinishedRace(Player.Player player)
        {
            playersClassificated.Add(player);
        }

        private void Awake()
        {
            PlayerGeneralEvents.onPlayerPass += OnPlayerPass;
            PlayerGeneralEvents.onPlayerDeath += OnPlayerDeath;
            instance = this;
        }

        #region Listeners

        private void OnPlayerDeath(Player.Player player)
        {
            alivePlayers--;
        }

        private void OnPlayerPass(Player.Player player, int classification)
        {

        }

        #endregion

        private void Start()
        {
            LoadPlayers();
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null)
                    PlayerGeneralEvents.onPlayerPass.Invoke(players[i].player, i);
            }
            InvokeRepeating("CheckPlayerClassification",0,0.1f);
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
            PlayerData playerChanged2 = null;
            int playerChangedPosition = 0;

            for (int i = 0; i < players.Length - 1; i++)
            {
                float distanceXPlayer1 = players[i].player.transform.position.x;
                float distanceXPlayer2 = players[i + 1].player.transform.position.x;

                if (distanceXPlayer1 < distanceXPlayer2)
                {
                    playerChanged = players[i];
                    playerChanged2 = players[i + 1];

                    playerChanged = players[i];
                    players[i] = players[i + 1];
                    players[i + 1] = playerChanged;
                    playerChangedPosition = i + 1;
                    changed = true;
                }

                if (changed)
                {
                    if (PlayerGeneralEvents.onPlayerPass != null)
                    {
                        if (playerChanged != null && playerChanged2 != null)
                        {
                            PlayerGeneralEvents.onPlayerPass.Invoke(playerChanged.player, playerChangedPosition);
                            PlayerGeneralEvents.onPlayerPass.Invoke(playerChanged2.player, playerChangedPosition - 1);
                        }
                    }

                    changed = false;
                }
            }
        }
        
        void OnDrawGizmos()
        {
            Gizmos.DrawIcon(posicaoSpawnPlayers, "snowboard_icon.png");
        }
    }
}
