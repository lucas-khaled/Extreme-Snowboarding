using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventSystem
{
    public delegate void OnPlayerDeath(Player player);
    public static OnPlayerDeath onPlayerDeath;

    public delegate void OnPlayerPass(Player player);
    public static OnPlayerPass onPlayerPass;
}
