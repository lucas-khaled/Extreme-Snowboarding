using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeSnowboarding.Script.Controllers
{
    [RequireComponent(typeof(AudioSource))]
    public class SceneAudioSource : MonoBehaviour
    {
        public static SceneAudioSource Instance { get; private set; }

        private AudioSource source;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            source = GetComponent<AudioSource>();
            source.playOnAwake = false;
            source.spatialBlend = 0;
        }

        public void PlayClip(AudioClip clip, float delay = 0)
        {
            source.clip = clip;
            source.PlayDelayed(delay);
        }
    }
}
