using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ExtremeSnowboarding.Multiplayer
{
    [CreateAssetMenu(fileName = "Game Scenes", menuName = "Multyplayer Settings/Game Scenes", order = 0)]
    public class GameScenes : ScriptableObject
    {
        [SerializeField] [Scene] private string[] playableScenes;

        public string GetRandomScene()
        {
            int i = Random.Range(0, playableScenes.Length);
            return playableScenes[i];
        }
    }
}
