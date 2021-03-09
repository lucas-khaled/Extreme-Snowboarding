using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField]
    private Mesh playerMesh;
    [SerializeField]
    private Color color1;
    [SerializeField]
    private Color color2;
    [SerializeField]
    private Color snowBoardColor;
    [SerializeField]
    private Player player;

    public void InstancePlayer(Vector3 position, int playerCode, Camera_Test camera)
    {
        var instantiatedPlayer = Instantiate(player, position, Quaternion.identity);
        camera.SetPlayer(instantiatedPlayer.GetComponent<Player>());
        instantiatedPlayer.GetComponent<Player>().SharedValues.playerCode = playerCode;
    }
}
