using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeSnowboarding
{
    public class PlayAnimation : MonoBehaviour
    {

        // Daniboy Code starts >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        [SerializeField]
        private ParticleSystem portalParticle;

        public void PlayPortalParticle()
        {
            portalParticle.Play();
        }
        // Daniboy Code ends >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    }
}
