using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController gameController;

    private int numOfPlayer = 1;

    [SerializeField] private bool isTest = false;

    public PlayerData[] playerData { get; set; }

    private void Awake()
    {
        if (gameController == null)
        {
            gameController = this;
            DontDestroyOnLoad(this);
        }
        else if (GameController.gameController != this)
        {
            Destroy(this);
        }
    }
    
    public void StartPlayerData(PlayerData[] players)
    {
        playerData = players;
    }

    public void ChangeNumOfPlayers(int num)
    {
        numOfPlayer = num;
    }
    public int GetNumberOfPlayers()
    {
        return numOfPlayer;
    }

    public void Play()
    {
        if(isTest)
            ChangeScene("Test_Scene");
        else
            ChangeScene("Fase1");
    }

    public void ChangeScene(string cena)    
    {
        SceneManager.LoadScene(cena);
    }
    public void SairDoJogo()
    {
        Application.Quit();
    }
}
