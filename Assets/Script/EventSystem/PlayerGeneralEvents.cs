using UnityEngine;

namespace Script.EventSystem
{
    public static class PlayerGeneralEvents
    {
        public delegate void OnPlayerDeath(Player.Player player);
        public static OnPlayerDeath onPlayerDeath;

        public delegate void OnPlayerPass(Player.Player player, int classification);
        public static OnPlayerPass onPlayerPass;

        public delegate void OnTurboChange(Player.Player player, float turboQuantity);
        public static OnTurboChange onTurboChange;

        public delegate void OnFuckFriendChange(Player.Player player, Sprite fuckFriendSprite);
        public static OnFuckFriendChange onFuckFriendChange;

        public delegate void OnPlayerInstantiate(Player.Player player);

        public static OnPlayerInstantiate onPlayerInstantiate;
    }
}
