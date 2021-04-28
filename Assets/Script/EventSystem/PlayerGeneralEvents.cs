using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerGeneralEvents
{
    public delegate void OnPlayerDeath(Player player);
    public static OnPlayerDeath onPlayerDeath;

    public delegate void OnPlayerPass(Player player);
    public static OnPlayerPass onPlayerPass;

    public delegate void OnPlayerInstantiate(Player player);
    public static OnPlayerInstantiate onPlayerInstantiate;
}
