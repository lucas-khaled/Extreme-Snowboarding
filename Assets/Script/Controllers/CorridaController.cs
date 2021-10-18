using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using ExtremeSnowboarding.Multiplayer;
using ExtremeSnowboarding.Script.EstadosPlayer;
using ExtremeSnowboarding.Script.EventSystem;
using ExtremeSnowboarding.Script.Player;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.InputSystem;
using UnityEngine;

namespace ExtremeSnowboarding.Script.Controllers
{
    public class CorridaController : MonoBehaviourPun, IOnEventCallback
    {
        [SerializeField] 
        private MultiplayerInstantiationSettings instantiationSettings;
        [SerializeField]
        private Player.Player playerPrefab;
        [SerializeField]
        private GameObject canvasPauseRef;
    
        public InputAction menuInput;
        public GameCamera camera;
        public GameObject catastrophe { get; set; }
        public List<Player.Player> playersClassificated { get; private set; }
        
        private List<Player.Player> _playersInGame = new List<Player.Player>();
        private  bool _isPaused;
        private PlayerData _playerData;
        private int _alivePlayers;

        private const int CustomManualInstantiationEventCode = 1;
        private List<Player.Player> _playersFinished = new List<Player.Player>();

        public static CorridaController instance { get; private set; }

        /// <summary>
        /// Return the total of player that started the game
        /// </summary>
        /// <returns></returns>
        public int GetPlayersInGameCount()
        {
            return _playersInGame.Count;
        }
        
        ///<summary> 
        ///Get an alive player other than the specified one 
        ///</summary>
        public Player.Player GetOtherPlayerThan(Player.Player player)
        {
            if (_alivePlayers > 1)
            {
                int index = Random.Range(0, _playersInGame.Count);
                Player.Player returnPlayer = _playersInGame[index];

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
            
            foreach (var p in _playersInGame)
            {
                if (p == player)
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
            int realPlace = Mathf.Clamp(place, 1, _playersInGame.Count);
            return _playersInGame[realPlace-1];
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

            playersClassificated = new List<Player.Player>();
            _isPaused = false;
            
            menuInput.Enable();
            menuInput.started += PauseInput;
        }
        
        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        #region Listeners

        private void OnPlayerDeath(Player.Player player)
        {
            _alivePlayers--;
            ChangeDeadPlayerClassification(player);
        }

        private void OnPlayerPass(Player.Player player, int classification)
        {

        }

        #endregion


        private void PauseInput(InputAction.CallbackContext context)
        {
            Debug.Log("KEK");
            if (context.started && !_isPaused)
            {
                Pause();
            }
            else if (context.started && _isPaused)
            {
                UnPause();
            }
        }

        private void ChangeDeadPlayerClassification(Player.Player deadPlayer)
        {
            deadPlayer.playerCamera.ChangeClassificationToDead();

            //Player.PlayerData playerAux = players[players.Length - 1];
            //Player.PlayerData playerAux2 = players[0];
            //players[players.Length - 1] = deadPlayerData;

            Player.Player playerAux = deadPlayer;


            for (int i = 0; i < _playersInGame.Count - 1; i++)
            {
                if (i != 0 && _playersInGame[i] != deadPlayer)
                {
                    playerAux = _playersInGame[i];
                    _playersInGame[i] = _playersInGame[i + 1];
                    _playersInGame[i + 1] = playerAux;
                }
            }

            //players[0] = playerAux2;
        }

        private void Start()
        {
            LoadPlayers();
            for (int i = 0; i < _playersInGame.Count; i++)
            {
                if (_playersInGame[i] != null)
                    PlayerGeneralEvents.onPlayerPass.Invoke(_playersInGame[i], i);
            }
            InvokeRepeating("CheckPlayerClassification",0,0.1f);
        }

        public void InstancePlayer(Vector3 position, PlayerData playerData)
        {
            GameObject playerGO = PhotonNetwork.Instantiate("Player", position, playerPrefab.transform.rotation);
            Player.Player player = playerGO.GetComponent<Player.Player>();
            PhotonView photonView = player.GetComponent<PhotonView>();
            
            object[] data = 
            {
                player.transform.position, photonView.ViewID, PlayerData.Serialize(playerData)
            };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others,
                CachingOption = EventCaching.AddToRoomCache
            };

            SendOptions sendOptions = new SendOptions
            {
                Reliability = true
            };

            PhotonNetwork.RaiseEvent(CustomManualInstantiationEventCode, data, raiseEventOptions, sendOptions);

            player.SetMaterialsAndMeshes(playerData.color1, playerData.color2, playerData.playerMeshes, instantiationSettings);
            StartCoroutine(AddAPlayer(player));
            
            camera.SetInitialPlayer(player);
            player.name = "My player";
            
            if(PlayerGeneralEvents.onPlayerInstantiate != null)
                PlayerGeneralEvents.onPlayerInstantiate.Invoke(player);
        }

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code == CustomManualInstantiationEventCode)
            {
                object[] data = (object[]) photonEvent.CustomData;

                Player.Player player = PhotonView.Find((int)data[1]).GetComponent<Player.Player>();
                PlayerData playerData = (PlayerData) PlayerData.Deserialize((byte[]) data[2]);
                
                player.SetMaterialsAndMeshes(playerData.color1, playerData.color2, playerData.playerMeshes, instantiationSettings);
                StartCoroutine(AddAPlayer(player));
                
                if(PlayerGeneralEvents.onPlayerInstantiate != null)
                    PlayerGeneralEvents.onPlayerInstantiate.Invoke(player);
            }
        }

        private IEnumerator AddAPlayer(Player.Player player)
        {
            _playersInGame.Add(player);

            if (_playersInGame.Count >= PhotonNetwork.CurrentRoom.PlayerCount)
            {
                Time.timeScale = 1;
                yield return new WaitForSeconds(3);
                _alivePlayers = _playersInGame.Count;
                foreach (var playerInGame in _playersInGame)
                    playerInGame.ChangeState(new Grounded());
            }
        }

        private void LoadPlayers()
        {
            _playerData = GameController.gameController.playerData;
            InstancePlayer(transform.position, _playerData);
        }

        private void CheckPlayerClassification()
        {
            bool changed = false;
            Player.Player playerChanged = null;
            Player.Player playerChanged2 = null;
            int playerChangedPosition = 0;

            for (int i = 0; i < _playersInGame.Count - 1; i++)
            {
                if(playersClassificated.Exists(x => x == _playersInGame[i+1]) || playersClassificated.Exists(x => x == _playersInGame[i]))
                    return;
                
                float distanceXPlayer1 = _playersInGame[i].transform.position.x;
                float distanceXPlayer2 = _playersInGame[i + 1].transform.position.x;

                if (_playersInGame[i].GetPlayerState().GetType() == typeof(Dead))
                {
                    distanceXPlayer1 = 0;
                }
                if (_playersInGame[i + 1].GetPlayerState().GetType() == typeof(Dead))
                {
                    distanceXPlayer2 = 0;
                }

                if (distanceXPlayer1 < distanceXPlayer2)
                {
                    playerChanged = _playersInGame[i];
                    playerChanged2 = _playersInGame[i + 1];

                    playerChanged = _playersInGame[i];
                    _playersInGame[i] = _playersInGame[i + 1];
                    _playersInGame[i + 1] = playerChanged;
                    playerChangedPosition = i + 1;
                    changed = true;
                }

                if (changed)
                {
                    if (PlayerGeneralEvents.onPlayerPass != null)
                    {
                        if (playerChanged != null && playerChanged2 != null)
                        {
                            PlayerGeneralEvents.onPlayerPass.Invoke(playerChanged, playerChangedPosition);
                            PlayerGeneralEvents.onPlayerPass.Invoke(playerChanged2, playerChangedPosition - 1);
                        }
                    }

                    changed = false;
                }
            }
        }

        public void Pause()
        {
            _isPaused = true;
            canvasPauseRef.SetActive(true);
            Time.timeScale = 0;
        }
        public void UnPause()
        {
            _isPaused = false;
            canvasPauseRef.SetActive(false);
            Time.timeScale = 1;
        }

        public void ReturnToMainMenu()
        {
            StopAllCoroutines();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuPrincipal");
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "snowboard_icon.png");
        }
    }
}
