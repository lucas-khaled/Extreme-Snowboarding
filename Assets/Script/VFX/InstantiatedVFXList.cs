using System.Collections;
using System.Collections.Generic;
using ExtremeSnowboarding.Script.VFX;
using UnityEngine;

namespace ExtremeSnowboarding
{
    [System.Serializable]
    public class InstantiatedVFXList
    {
        public List<PlayerVFX> playerVfxes = new List<PlayerVFX>();
        public int playerCode { get; set; }
    }
}
