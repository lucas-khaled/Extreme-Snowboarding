using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    private Mesh playerMesh;
    private Color color1;
    private Color color2;
    private Color snowboardColor;
    public Player player;

    public void InstancePlayer(Vector3 position, int playerCode, GameObject playerPrefab, Camera_Test camera)
    {
        GameObject playerGO = MonoBehaviour.Instantiate(playerPrefab, position, Quaternion.identity);
        player = playerGO.GetComponent<Player>();

        player.SharedValues.playerCode = playerCode;
        camera.SetPlayer(player);
    }
}
