﻿using System;
using System.Collections.Generic;
using System.IO;
using ExitGames.Client.Photon;
using MoreMountains.Feedbacks;
using NaughtyAttributes;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace ExtremeSnowboarding.Script.VFX
{
    [CreateAssetMenu(fileName = "Player VFX Feedback Group", menuName = "PlayerFeedbacks/VFX", order = 0)]
    public class PlayerFeedbacksGroup : ScriptableObject
    {
        [SerializeField] [Scene]
        private int restartScene;
        
        [FormerlySerializedAs("VFXList")] [SerializeField] [OnValueChanged("OnListChanged")]
        private List<PlayerFeedbacks> vfxList;

        private PlayerFeedbacks[] hashedArray;
        private bool isHashed = false;

        /// <summary>
        /// This method starts hash on the list and instantiates the feedbacks. It can oly be played once.
        /// </summary>
        public void StartFeedbacks(Transform player, int ID)
        {
            StartHash();

            InstantiatedFeedbacksList instantiated = new InstantiatedFeedbacksList();
            
            for(int i = 0; i<hashedArray.Length;i++)
            {
                if (hashedArray[i] == null || hashedArray[i].GetFeedback() == null)
                {
                    instantiated.playerVfxes.Add(new PlayerFeedbacks(null));
                    continue;
                }

                GameObject hashedObj = hashedArray[i].GetFeedback().gameObject;
                GameObject particleGO = PhotonNetwork.Instantiate(Path.Combine("Feedbacks",hashedObj.name), hashedObj.transform.position, hashedObj.transform.rotation);

                MMFeedbacks instantiatedFeedback = particleGO.GetComponent<MMFeedbacks>();
                instantiatedFeedback.name = hashedArray[i].ParticleName;
                instantiatedFeedback.SetPlayerParent(player);

                PlayerFeedbacks feedbacks = new PlayerFeedbacks(instantiatedFeedback);
                instantiated.playerVfxes.Add(feedbacks);
            }
            
            instantiated.playerCode = ID;
            VFXManager.instance.AddToList(instantiated);
        }
        
        
        #region GET_NAME_FUNCTIONS

        /// <summary>
        /// If the StartHash() was already called, it searches by hash. Otherwise, it will search the entire list. 
        /// Returns null when the particle name doesn't exists.
        /// </summary>
        public PlayerFeedbacks GetFeedbackByName(string name, int playerCode)
        {
            if (isHashed)
                return GetFeedbackByNameHashed(name, playerCode);
            else
                return GetFeedbackByNameNotHashed(name, playerCode);
        }

        private PlayerFeedbacks GetFeedbackByNameHashed(string name, int playerCode)
        {
            int index = GetHashedID(name, true);
            if (index == -1)
                return null;

            return VFXManager.instance.InstantiatedList.Find(x => x.playerCode == playerCode).playerVfxes[index];
        }

        private PlayerFeedbacks GetFeedbackByNameNotHashed(string name, int playerCode)
        {
            return VFXManager.instance.InstantiatedList.Find(x => x.playerCode == playerCode).playerVfxes
                .Find(x => x.ParticleName == name);
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

            int initialIndex = Mathf.Abs(sum) % hashedArray.Length;
            int finalIndex = (onlySearchingName) ? GetOffset(initialIndex, name) : GetEmptyIndex(initialIndex);

            return finalIndex;
        }

        private int GetOffset(int index, string name)
        {
            return GetOffsetRecursive(index, index, name);
        }

        private int GetOffsetRecursive(int index, int initialValue, string name)
        {
            if (hashedArray[index].ParticleName == name)
                return index;
            
                if (index + 1 == initialValue)
                    return index;
                if (index == hashedArray.Length - 1)
                    return GetOffsetRecursive(0, initialValue, name);
                
                    return GetOffsetRecursive(index + 1, initialValue, name);
        }


        private int GetEmptyIndex(int index)
        {
            return GetEmptyIndexRecursive(index, index);
        }

        private int GetEmptyIndexRecursive(int index, int initialValue)
        {
            if (hashedArray[index] == null)
                return index;
            else
            {
                if (index + 1 == initialValue)
                    return -1;
                if (index == hashedArray.Length - 1)
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

            hashedArray = new PlayerFeedbacks[vfxList.Count];
            
            foreach(PlayerFeedbacks vfx in vfxList)
            {
                MMFeedbacks part = vfx.GetFeedback();
                
                string name = (!string.IsNullOrEmpty(vfx.ParticleName)) ? vfx.ParticleName : vfx.GetFeedback().name;
                hashedArray[GetHashedID(name)] = vfx;
            }

            isHashed = true;
        }

        private void OnListChanged()
        {
            isHashed = false;
            Debug.Log("Change");
        }
    }
    
    [System.Serializable]
    public class PlayerFeedbacks
    {
        [SerializeField] private string particleName;
        [SerializeField] private MMFeedbacks feedbacks;
        
        private bool locked = false;

        public string ParticleName => ( string.IsNullOrEmpty(particleName)) ? feedbacks.name : particleName;
        
        public MMFeedbacks GetFeedback()
        {
            return feedbacks;
        }

        /// <summary>
        /// Will lock the feedback from having their state changed.
        /// </summary>
        /// <param name="stopPlaying"> Whether you want that feedback locked state is on stop. </param>
        /// <param name="deactivate"> Wheter you want to also deactivate the feedback Game Object. </param>
        public void LockFeedback(bool stopPlaying = false, bool deactivate = false)
        {
            if (stopPlaying)
                StopFeedback(deactivate);

            locked = true;
        }

        /// <summary>
        /// It will unlock the feedback from having their state changed.
        /// </summary>
        /// <param name="startPlaying"> Wheter you want to also start the feedback </param>
        public void UnlockFeedback(bool startPlaying = false)
        {
            if (startPlaying)
                StartFeedback();

            locked = false;
        }

        public void StartFeedback(bool netCall = true)
        {
            if (locked)
                return;

            if (feedbacks != null && feedbacks.gameObject.scene.IsValid())
            {
                feedbacks.gameObject.SetActive(true);
                feedbacks.PlayFeedbacks();
                
            }
        }

        public void StopFeedback(bool deActivate = false, bool netCall = true)
        {
            if (locked)
                return;
            if (feedbacks != null && feedbacks.gameObject.scene.IsValid())
            {
                feedbacks.gameObject.SetActive(!deActivate);
                feedbacks.StopFeedbacks();
            }
        }

        public PlayerFeedbacks(MMFeedbacks feedbacks)
        {
            this.feedbacks = feedbacks;
            particleName = feedbacks.name;
        }
    }
}