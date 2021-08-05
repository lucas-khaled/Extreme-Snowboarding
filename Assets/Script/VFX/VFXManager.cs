using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeSnowboarding.Script.VFX
{
    public class VFXManager : MonoBehaviour
    {
        public static VFXManager instance;

        public List<InstantiatedFeedbacksList> InstantiatedList => instantiatedList;

        private List<InstantiatedFeedbacksList> instantiatedList = new List<InstantiatedFeedbacksList>();

        public void AddToList(InstantiatedFeedbacksList instantiatedFeedbacksList)
        {
            instantiatedList.Add(instantiatedFeedbacksList);
        }
        
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            instance = this;
        }
    }
}