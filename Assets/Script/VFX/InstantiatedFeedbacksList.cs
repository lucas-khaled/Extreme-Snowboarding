using System.Collections;
using System.Collections.Generic;
using ExtremeSnowboarding.Script.VFX;
using UnityEngine;

namespace ExtremeSnowboarding.Script.VFX
{
    [System.Serializable]
    public class InstantiatedFeedbacksList
    {
        public List<PlayerFeedbacks> playerVfxes = new List<PlayerFeedbacks>();
        public int playerCode { get; set; }
    }
}
