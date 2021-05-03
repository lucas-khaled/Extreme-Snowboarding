using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventSystem
{
    public delegate void OnPlayerDeath(Player player);
    public static OnPlayerDeath onPlayerDeath;

    public delegate void OnPlayerPass(Player player, int classification);
    public static OnPlayerPass onPlayerPass;

    public delegate void OnTurboChange(Player player, float turboQuantity);
    public static OnTurboChange onTurboChange;

    public delegate void OnFuckFriendChange(Player player, Sprite fuckFriendSprite);
    public static OnFuckFriendChange onFuckFriendChange;
}
