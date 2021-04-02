using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    private Mesh playerMesh;
    private Color color1;
    private Color color2;
    private Shader playerShader;

    public Player player;

    public PlayerData(Color color1, Color color2, Shader playerShader)
    {
        this.color1 = color1;
        this.color2 = color2;
        this.playerShader = playerShader;
    }

    public void InstancePlayer(Vector3 position, int playerCode, GameObject playerPrefab, GameCamera camera)
    {
        GameObject playerGO = MonoBehaviour.Instantiate(playerPrefab, position, Quaternion.identity);
        player = playerGO.GetComponent<Player>();

        Material material = new Material(playerShader);
        player.GetMeshRenderer().material = material;
        material.SetColor("_PrimaryColor", color1);
        material.SetColor("_SecondaryColor", color2);

        player.SharedValues.playerCode = playerCode;
        camera.SetPlayer(player);
    }
}
