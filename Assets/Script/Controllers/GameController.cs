using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController gameController;

    [SerializeField, Min(0)]
    private int numOfPlayer = 4;

    public PlayerData[] playerData { get; set; }

    private void Awake()
    {
        StartPlayerData();

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
    
    private void StartPlayerData()
    {
        playerData = new PlayerData[numOfPlayer];

        for(int i = 0; i<numOfPlayer; i++)
        {
            playerData[i] = new PlayerData();
        }
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
