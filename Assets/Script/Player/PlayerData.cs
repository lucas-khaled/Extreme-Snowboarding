using Script.EventSystem;
using UnityEngine;

namespace Script.Player
{
    public class PlayerData
    {
        private Mesh[] playerMeshes;
        private Color color1;
        private Color color2;
        private Shader playerShader;
        private int index;
    
        public Player player;

        public PlayerData(Color color1, Color color2, Shader playerShader, int playerIndex, Mesh[] meshes)
        {
            this.color1 = color1;
            this.color2 = color2;
            this.playerShader = playerShader;
            index = playerIndex + 1;
            playerMeshes = meshes;
        }

        public void InstancePlayer(Vector3 position, int playerCode, GameObject playerPrefab, GameCamera camera)
        {
            GameObject playerGO = MonoBehaviour.Instantiate(playerPrefab, position, Quaternion.identity);
            playerGO.name = "Player" + index;
        
            player = playerGO.GetComponent<Player>();

            Material material = new Material(playerShader);
            player.SetPlayerMeshes(material, playerMeshes);
        
            material.SetColor("_PrimaryColor", color1);
            material.SetColor("_SecondaryColor", color2);

            player.SharedValues.playerCode = playerCode;
            camera.SetPlayer(player);

            player.playerInput.SwitchCurrentControlScheme("Player" + index);
        
            if(PlayerGeneralEvents.onPlayerInstantiate != null)
                PlayerGeneralEvents.onPlayerInstantiate.Invoke(player);
        }

        private void SetMeshes()
        {
        
        }
    }
}
