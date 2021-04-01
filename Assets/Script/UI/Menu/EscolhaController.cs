using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EscolhaController
{
    [SerializeField]
    private Transform escolhaPanel;
    [Header("Prefabs")]
    [SerializeField]
    private PlayerMenu playerChoose;
    [Header("Organization Values")]
    [SerializeField]
    private float rowSize = 4;

    List<PlayerMenu> playersModels = new List<PlayerMenu>();

    public void SetPlayers(int numOfPlayers)
    {
        if (playersModels.Count == numOfPlayers)
            return;

        if(playersModels.Count > numOfPlayers)
        {
            for(int i = playersModels.Count-1; i >= numOfPlayers ; i--)
            {
                GameObject player = playersModels[i].gameObject;
                MonoBehaviour.Destroy(player);
                playersModels.RemoveAt(i);
            }
        }
        else if(playersModels.Count < numOfPlayers)
        {
            for (int i = playersModels.Count; i < numOfPlayers; i++)
            {
                GameObject playerGO = MonoBehaviour.Instantiate(playerChoose.gameObject, escolhaPanel);
                PlayerMenu player = playerGO.GetComponent<PlayerMenu>();
                playersModels.Add(player);
            }
        }

        Ordenate();

    }

    private void Ordenate()
    {
        if (playersModels.Count == 1)
        {
            playersModels[0].transform.position = escolhaPanel.transform.position;
            return;
        }

        float offset = rowSize / (playersModels.Count-1);
        for(int i = 0; i<playersModels.Count; i++)
        {
            Vector3 newPosition = escolhaPanel.transform.position + -escolhaPanel.transform.right * (rowSize*0.5f-i*offset);
            playersModels[i].transform.position = newPosition;
        }
    }
}
