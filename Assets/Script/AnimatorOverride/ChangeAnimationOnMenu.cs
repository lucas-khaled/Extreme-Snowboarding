using ExtremeSnowboarding.Script.UI.Menu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeSnowboarding
{
    public class ChangeAnimationOnMenu : MonoBehaviour
    {
        [SerializeField] private PlayerMenu playerMenu;

        public void ChangeAnimation(AnimatorOverrideController overriderRef)
        {
            playerMenu.ChangeOverrider(overriderRef);
        }
    }
}
