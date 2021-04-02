using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIController : MonoBehaviour
{
    [SerializeField]
    private MenuCameraPointController menuCameraPointController;

    [Header("Escolha Panel")]
    [SerializeField]
    private EscolhaController escolhaController;

    private void Awake()
    {
        escolhaController.SetPlayers(1);
    }

    public void PressPlay()
    {
        escolhaController.SendPlayerData();
        GameController.gameController.Play();
    }

    public void GoNext()
    {
        menuCameraPointController.NextPoint();
    }

    public void ChangeNumOfPlayers(int num)
    {
        GameController.gameController.ChangeNumOfPlayers(num);
        escolhaController.SetPlayers(num);
    }
}
