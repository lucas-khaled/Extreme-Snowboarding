using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController gameController;

    public PlayerData[] playerData;

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
    
    public void ChangeScene(string cena)    
    {
        SceneManager.LoadScene(cena);
    }
    public void SairDoJogo()
    {
        Application.Quit();
    }
}
