using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using ExtremeSnowboarding.Script.EventSystem;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace ExtremeSnowboarding.Script.Player
{
    public class PlayerData 
    {
        private Mesh[] playerMeshes;
        private Color color1;
        private Color color2;
        private string playerShader;
        private int index;
        private Texture2D mask01;
        private Texture2D mask02;
    
        public Player player;
        
        private const byte PlayerDataEventCode = 1;

        public void InstancePlayer(Vector3 position, int playerCode, GameObject playerPrefab, GameCamera camera)
        {
            GameObject playerGO = PhotonNetwork.Instantiate("Player", position, playerPrefab.transform.rotation);
            playerGO.name = "Player" + index;
        
            player = playerGO.GetComponent<Player>();
            player.SetMaterials(color1, color2, playerMeshes, playerShader);

            player.SharedValues.playerCode = playerCode;
            camera.SetInitialPlayer(player);

            player.playerInput.SwitchCurrentControlScheme("Player" + index);

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(PlayerDataEventCode, TransformToNetworkData(), raiseEventOptions,
                SendOptions.SendReliable);
        
            if(PlayerGeneralEvents.onPlayerInstantiate != null)
                PlayerGeneralEvents.onPlayerInstantiate.Invoke(player);
        }
        
        public PlayerData(Color color1, Color color2, string playerShader, int playerIndex, Mesh[] meshes, Texture2D mask01, Texture2D mask02)
        {
            this.color1 = color1;
            this.color2 = color2;
            this.playerShader = playerShader;
            index = playerIndex + 1;
            playerMeshes = meshes;
            this.mask01 = mask01;
            this.mask02 = mask02;
        }

        private object[] TransformToNetworkData()
        {
            IEnumerable<string> meshNames = from mesh in playerMeshes
                                        select mesh.name;

            foreach (var name in meshNames)
            {
                Debug.Log(name);
            }
            
            List<object> netObject = new List<object>()
            {
                color1.r, color1.g, color1.b, // color1 = 0, 1, 2
                color2.r, color2.g, color2.b, // color2 = 3, 4, 5
                playerShader // shaderName = 6
            };

            netObject.AddRange(meshNames);
            
            return netObject.ToArray();
        }

        
    }
}
