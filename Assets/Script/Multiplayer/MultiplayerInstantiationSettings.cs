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
        
    }

    [System.Serializable]
    public struct PlayerMesh
    {
        public Mesh mesh;
        public string meshName;
    }
}