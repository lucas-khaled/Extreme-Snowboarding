using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeSnowboarding.Script.VFX
{
    public class VFXManager : MonoBehaviour
    {
        public static VFXManager instance;

        public List<InstantiatedVFXList> InstantiatedList => instantiatedList;

        private List<InstantiatedVFXList> instantiatedList = new List<InstantiatedVFXList>();

        public void AddToList(InstantiatedVFXList instantiatedVFXList)
        {
            instantiatedList.Add(instantiatedVFXList);
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