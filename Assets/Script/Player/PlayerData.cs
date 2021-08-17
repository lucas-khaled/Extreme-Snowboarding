using ExtremeSnowboarding.Script.EventSystem;
using Photon.Pun;
using UnityEngine;

namespace ExtremeSnowboarding.Script.Player
{
    public class PlayerData
    {
        private Mesh[] playerMeshes;
        private Color color1;
        private Color color2;
        private Shader playerShader;
        private int index;
        private Texture2D mask01;
        private Texture2D mask02; 
    
        public Player player;

        public void InstancePlayer(Vector3 position, int playerCode, GameObject playerPrefab, GameCamera camera)
        {
            GameObject playerGO = PhotonNetwork.Instantiate("Player", position, playerPrefab.transform.rotation);
            playerGO.name = "Player" + index;
        
            player = playerGO.GetComponent<Player>();

            Material material = new Material(playerShader);
            player.SetPlayerMeshes(material, playerMeshes);
        
            material.SetColor("_PrimaryColor", color1);
            material.SetColor("_SecondaryColor", color2);
            
            material.SetTexture("_Color1Mask", mask01);
            material.SetTexture("_Color2Mask", mask02);

            player.SharedValues.playerCode = playerCode;
            camera.SetInitialPlayer(player);

            player.playerInput.SwitchCurrentControlScheme("Player" + index);
        
            if(PlayerGeneralEvents.onPlayerInstantiate != null)
                PlayerGeneralEvents.onPlayerInstantiate.Invoke(player);
        }
        
        public PlayerData(Color color1, Color color2, Shader playerShader, int playerIndex, Mesh[] meshes, Texture2D mask01, Texture2D mask02)
        {
            this.color1 = color1;
            this.color2 = color2;
            this.playerShader = playerShader;
            index = playerIndex + 1;
            playerMeshes = meshes;
            this.mask01 = mask01;
            this.mask02 = mask02;
        }
    }
}
