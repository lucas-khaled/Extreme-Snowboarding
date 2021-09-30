using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeSnowboarding
{
    public class AnimatorOverrider : MonoBehaviour
    {
        private Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        public void SetAnimations(AnimatorOverrideController overriderController)
        {
            anim.runtimeAnimatorController = overriderController;
        }
    }
}
