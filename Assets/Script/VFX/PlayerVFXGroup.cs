using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Script.VFX
{
    [CreateAssetMenu(fileName = "Player VFX Group", menuName = "Player VFX Group", order = 0)]
    public class PlayerVFXGroup : ScriptableObject
    {
        [SerializeField] [OnValueChanged("OnListChanged")]
        private List<PlayerVFX> VFXList;

        private PlayerVFX[] VFXRealArray;

        private bool isHashed = false;

        /// <summary>
        /// This method starts hash on the list and instantiates the particles. It can oly be played once.
        /// </summary>
        public void StartParticles(Transform player)
        {
            StartHash();
            
            for(int i = 0; i<VFXRealArray.Length;i++)
            {
                ParticleSystem instantiatedParticle = Instantiate(VFXRealArray[i].GetParticle().gameObject, player).GetComponent<ParticleSystem>();
                VFXRealArray[i] = new PlayerVFX(instantiatedParticle);
            }
        }

        #region GET_NAME_FUNCTIONS

        /// <summary>
        /// If the StartHash() was already called, it searches by hash. Otherwise, it will search the entire list. 
        /// Returns null when the particle name doesn't exists.
        /// </summary>
        public PlayerVFX GetVFXByName(string name)
        {
            if (isHashed)
                return GetVFXByNameHashed(name);
            else
                return GetVFXByNameNotHashed(name);
        }

        private PlayerVFX GetVFXByNameHashed(string name)
        {
            int index = GetHashedID(name, true);
            if (index == -1)
                return null;
            else
                return VFXRealArray[index];
        }

        private PlayerVFX GetVFXByNameNotHashed(string name)
        {
            foreach(PlayerVFX vfx in VFXList)
            {
                if (vfx.GetParticle().name == name)
                    return vfx;
            }
            return null;
        }

        #endregion

        #region HASH_FUNCTIONS

        private int GetHashedID(string name, bool onlySearchingName = false)
        {
            int sum = 0;
            for(int i = 1; i<=name.Length; i++)
            {
                int operation = i % 3;
                switch (operation)
                {
                    case 1:
                        sum += (int)name[i-1];
                        break;
                    case 2:
                        sum -= (int)name[i-1];
                        break;
                    case 3:
                        sum *= (int)name[i-1];
                        break;
                }
            }

            int initialIndex = Mathf.Abs(sum) % VFXList.Count;
            int finalIndex = (onlySearchingName) ? GetOffset(initialIndex, name) : GetEmptyIndex(initialIndex);

            return finalIndex;
        }

        private int GetOffset(int index, string name)
        {
            return GetOffsetRecursive(index, index, name);
        }

        private int GetOffsetRecursive(int index, int initialValue, string name)
        {
            if (VFXRealArray[index].GetParticle().name == name)
                return index;
            else
            {
                if (index + 1 == initialValue)
                    return index;
                if (index == VFXRealArray.Length - 1)
                    return GetOffsetRecursive(0, initialValue, name);
                else
                    return GetOffsetRecursive(index + 1, initialValue, name);
            }
        }


        private int GetEmptyIndex(int index)
        {
            return GetEmptyIndexRecursive(index, index);
        }

        private int GetEmptyIndexRecursive(int index, int initialValue)
        {
            if (VFXRealArray[index] == null)
                return index;
            else
            {
                if (index + 1 == initialValue)
                    return -1;
                if (index == VFXRealArray.Length - 1)
                    return GetEmptyIndexRecursive(0, index);
                else
                    return GetEmptyIndexRecursive(index + 1, index);
            }
        }

        #endregion
        
        [Button("Start Hash")]
        private void StartHash()
        {
            if (isHashed)
            {
                Debug.LogWarning("Group is already Hashed");
                return;
            }
            
            VFXRealArray = new PlayerVFX[VFXList.Count];
            
            foreach(PlayerVFX vfx in VFXList)
            {
                ParticleSystem part = vfx.GetParticle();
                if (part != null)
                {
                    string name = (vfx.particleName != string.Empty) ? vfx.particleName : part.name;
                    VFXRealArray[GetHashedID(name)] = vfx;
                }
                    
                
            }

            isHashed = true;
            
            Debug.Log("hashed");
        }

        private void OnListChanged()
        {
            isHashed = false;
            Debug.Log("Change");
        }
    }
    
    [System.Serializable]
    public class PlayerVFX
    {
        public string particleName;
        
        [SerializeField] private ParticleSystem particle;

        private bool locked = false;

        public ParticleSystem GetParticle()
        {
            return particle;
        }

        /// <summary>
        /// Will lock the particle from having their state changed.
        /// </summary>
        /// <param name="stopPlaying"> Whether you want that particle locked state is on stop. </param>
        /// <param name="deactivate"> Wheter you want to also deactivate the particle Game Object. </param>
        public void LockParticle(bool stopPlaying = false, bool deactivate = false)
        {
            if (stopPlaying)
                StopParticle(deactivate);

            locked = true;
        }

        /// <summary>
        /// It will unlock the particle from having their state changed.
        /// </summary>
        /// <param name="startPlaying"> Wheter you want to also start the particle </param>
        public void UnlockParticle(bool startPlaying = false)
        {
            if (startPlaying)
                StartParticle();

            locked = false;
        }

        public void StartParticle()
        {
            if (locked)
                return;

            if (particle != null && particle.gameObject.scene.IsValid())
            {
                particle.gameObject.SetActive(true);
                particle.Play();
            }
        }

        public void StopParticle(bool deActivate = false)
        {
            if (locked)
                return;
            if (particle != null && particle.gameObject.scene.IsValid())
            {
                particle.gameObject.SetActive(!deActivate);
                particle.Stop();
            }
        }

        public PlayerVFX(ParticleSystem particle)
        {
            this.particle = particle;
        }
    }
}