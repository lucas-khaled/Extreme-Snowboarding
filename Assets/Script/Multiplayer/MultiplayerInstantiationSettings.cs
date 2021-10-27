using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExtremeSnowboarding.Multiplayer
{
    [CreateAssetMenu(fileName = "Instatiation Settings", menuName = "Multyplayer Settings/Instantiation Settings", order = 0)]
    public class MultiplayerInstantiationSettings : ScriptableObject
    {
        [SerializeField]
        private List<PlayerMesh> playerMeshes;

        [SerializeField] 
        private List<PlayerAnimatorOverrider> playerAnimatorOverriders;

        public Shader playerShader;
        public Texture playerMask01;
        public Texture playerMask02;

        public Mesh GetMeshByName(string name)
        {
            return playerMeshes.Find(x => x.meshName == name).mesh;
        }

        public Mesh[] GetMeshesByNames(params string[] names)
        {
            return (from playerMesh in playerMeshes
                where names.Any(x => x == playerMesh.meshName)
                    select playerMesh.mesh).ToArray();
        }

        public AnimatorOverrideController GetOverriderByName(string name)
        {
            return playerAnimatorOverriders.Find(x => x.name == name).animatorOverriderController;
        }

        public string GetNameByAnimator(AnimatorOverrideController animatorOverrideController)
        {
            return playerAnimatorOverriders.Find(x => x.animatorOverriderController == animatorOverrideController).name;
        }

    }

    [System.Serializable]
    public struct PlayerMesh
    {
        public Mesh mesh;
        public string meshName;
    }

    [System.Serializable]
    public struct PlayerAnimatorOverrider
    {
        public AnimatorOverrideController animatorOverriderController;
        public string name;
    }
}