using System;
using UnityEngine;

namespace Script.Items.Effects
{
    public class EffectsController : MonoBehaviour
    {
        public static EffectsController instance;
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }
        }
        
    }
}