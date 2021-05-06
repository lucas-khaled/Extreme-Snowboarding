using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HudControl : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Image refClassificationImage;
    [SerializeField]
    private Image refTurboBar;
    [SerializeField]
    private Image refFuckFriendImage;
    [SerializeField]
    private GameObject refFire;

    [Header ("Values")]
    [SerializeField]
    private Sprite[] classificationSprites;

    Player player;

    private void Awake()
    {
        EventSystem.onPlayerPass += OnPlayerPass;
        EventSystem.onTurboChange += OnTurboChange;
        EventSystem.onFuckFriendChange += OnFuckFriendChange;
    }

    public void setPlayer(Player player)
    {
        this.player = player;
    }

    private void OnPlayerPass(Player player, int classification)
    {
        if (this.player == player)
            refClassificationImage.sprite = classificationSprites[classification];

    }
    private void OnTurboChange(Player player, float turboQuantity)
    {
        if (this.player == player)
        {
            float fill = turboQuantity / 1;
            refTurboBar.fillAmount = fill;
            refFire.transform.localPosition = new Vector2(fill * 312, 0);
        }
    }

    private void OnFuckFriendChange(Player player, Sprite fuckFriendSprite)
    {
        if (this.player == player)
            refFuckFriendImage.sprite = fuckFriendSprite;
    }
}
