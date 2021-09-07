using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using ExtremeSnowboarding.Script.EstadosPlayer;
using ExtremeSnowboarding.Script.EventSystem;
using ExtremeSnowboarding.Script.Player;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.InputSystem;
using UnityEngine;

namespace ExtremeSnowboarding.Script.Controllers
{
    [RequireComponent(typeof(PhotonView))]
    public class CorridaController : MonoBehaviourPun
    {
        [SerializeField] 
        private MultiplayerInstantiationSettings instantiationSettings;
        [SerializeField]
        private Player.Player playerPrefab;
        [SerializeField]
        private GameObject canvasPauseRef;
    
        public InputAction menuInput;
        public GameCamera[] cameras;
        public GameObject catastrophe { get; set; }
        public List<Player.Player> playersClassificated { get; private set; }
        
        private List<Player.Player> _playersInGame = new List<Player.Player>();
        private PhotonView _photonView;
        private  bool _isPaused;
        private PlayerData _playerData;
        private int _alivePlayers;

        private const int CustomManualInstantiationEventCode = 1;

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

            _photonView = GetComponent<PhotonView>();
            instance = this;

            playersClassificated = new List<Player.Player>();
            _isPaused = false;
            
            menuInput.Enable();
            menuInput.started += PauseInput;
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
        
        private void InstantiatePlayers()
        {
            _photonView.RPC("RPC_PlayerInstantiate", RpcTarget.All, transform.position, PlayerData.Serialize(_playerData));//InstancePlayer(transform.position + Vector3.forward * (i - 1), 1, _playerData, cameras[i]);
            
            ChangeCameraByPlayers();
        }
        
        /*public void InstancePlayer(Vector3 position, int playerCode, PlayerData playerData, GameCamera camera)
        {
            GameObject playerGO = 
            playerGO.name = "Player" + playerData.index;
        
            Player.Player player = playerGO.GetComponent<Player.Player>();
            player.SetMaterials(playerData.color1, playerData.color2, playerData.playerMeshes, playerData.playerShader);

            player.SharedValues.playerCode = playerCode;
            camera.SetInitialPlayer(playerData.player);

            player.playerInput.SwitchCurrentControlScheme("Player" + playerData.index);

            if(PlayerGeneralEvents.onPlayerInstantiate != null)
                PlayerGeneralEvents.onPlayerInstantiate.Invoke(player);
        }*/

        [PunRPC]
        private void RPC_PlayerInstantiate(Vector3 position, byte[] data)
        {
            PlayerData playerData = (PlayerData) PlayerData.Deserialize(data);
            Player.Player player = Instantiate(playerPrefab, position, playerPrefab.transform.rotation);
            PhotonView photonView = player.GetComponent<PhotonView>();
            PhotonNetwork.AllocateViewID(photonView);

            /*if ()
            {
                object[] sendData = 
                {
                    player.transform.position, player.transform.rotation, photonView.ViewID
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

                PhotonNetwork.RaiseEvent(CustomManualInstantiationEventCode, sendData, raiseEventOptions, sendOptions);
            }*/
            
            player.SetMaterials(playerData.color1, playerData.color2, playerData.playerMeshes, instantiationSettings);

            _playersInGame.Add(player);
            player.name = "Player" + _playersInGame.Count;
            
            if(PlayerGeneralEvents.onPlayerInstantiate != null)
                PlayerGeneralEvents.onPlayerInstantiate.Invoke(player);
        }

        private void ChangeCameraByPlayers()
        {
            if (_playersInGame.Count == 1)
            {
                cameras[0].gameObject.GetComponent<Camera>().rect = new Rect(0, 0, 1, 1);
                cameras[1].gameObject.GetComponent<Camera>().rect = new Rect(0, 0, 0, 0);
                cameras[2].gameObject.GetComponent<Camera>().rect = new Rect(0, 0, 0, 0);
                cameras[3].gameObject.GetComponent<Camera>().rect = new Rect(0, 0, 0, 0);
            }
            else if (_playersInGame.Count == 2)
            {
               cameras[0].gameObject.GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 1);
               cameras[1].gameObject.GetComponent<Camera>().rect = new Rect(0.5f, 0, 1, 1);
               cameras[2].gameObject.GetComponent<Camera>().rect = new Rect(0, 0, 0, 0);
               cameras[3].gameObject.GetComponent<Camera>().rect = new Rect(0, 0, 0, 0);
            }
            else if (_playersInGame.Count == 3)
            {
               cameras[0].gameObject.GetComponent<Camera>().rect = new Rect(0, 0, 0.3333333f, 1);
               cameras[1].gameObject.GetComponent<Camera>().rect = new Rect(0.33f, 0, 0.34f, 1);
               cameras[2].gameObject.GetComponent<Camera>().rect = new Rect(0.67f, 0, 0.3333333f, 1);
               cameras[3].gameObject.GetComponent<Camera>().rect = new Rect(0, 0, 0, 0);
            }
            else if (_playersInGame.Count == 4)
            {
               cameras[0].gameObject.GetComponent<Camera>().rect = new Rect(0, 0.5f, 0.5f, 1);
               cameras[1].gameObject.GetComponent<Camera>().rect = new Rect(0.5f, 0.5f, 1, 1);
               cameras[2].gameObject.GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 0.5f);
               cameras[3].gameObject.GetComponent<Camera>().rect = new Rect(0.5f, 0, 1, 0.5f);
            }

        }

        private void LoadPlayers()
        {
            _playerData = GameController.gameController.playerData[0];
            /*alivePlayers = playersDatas.Length;*/
            InstantiatePlayers();
        }

        private void CheckPlayerClassification()
        {
            bool changed = false;
            Player.Player playerChanged = null;
            Player.Player playerChanged2 = null;
            int playerChangedPosition = 0;

            for (int i = 0; i < _playersInGame.Count - 1; i++)
            {
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
