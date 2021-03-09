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

    public void InstancePlayer(Vector3 position)
    {
        Instantiate(player, position, Quaternion.identity);
    }
}
