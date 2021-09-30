using ExtremeSnowboarding.Script;
using ExtremeSnowboarding.Script.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ExtremeSnowboarding
{
    public class MMFeedbackTestMeu : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private Color color1;
        [SerializeField] private Color color2;
        [SerializeField] private Shader shader;
        [SerializeField] private Mesh[] meshes;
        [SerializeField] private Texture2D mask1;
        [SerializeField] private Texture2D mask2;
        [SerializeField] private AnimatorOverrider AnimatorToOverride;

        void Start()
        {
            PlayerData playerData = new PlayerData(color1, color2, shader, 0, meshes, mask1, mask2);
            playerData.InstancePlayer(Vector3.zero, 0, player, Camera.main.GetComponent<GameCamera>());
        }
    }
}
