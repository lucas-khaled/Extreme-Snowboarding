using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using MoreMountains.Feedbacks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

namespace ExtremeSnowboarding.Script.Player
{
    [CreateAssetMenu(fileName = "Player Movimentation Feedback Group", menuName = "PlayerFeedbacks/Movimentation", order = 0)]
    public class PlayerMovimentationFeedbacks : ScriptableObject
    {
        [SerializeField]
        private MMFeedbacks _normalFallFeedback;
        [SerializeField]
        private MMFeedbacks _hardFallFeedback;
        [SerializeField]
        private MMFeedbacks _landingFeedback;
        [SerializeField]
        private MMFeedbacks _jumpingFeedback;
        [SerializeField]
        private MMFeedbacks _skiingFeedback;
        [SerializeField]
        private MMFeedbacks _trickFeedback;
        [SerializeField]
        private MMFeedbacks _victoryFeedback;
        [SerializeField]
        private MMFeedbacks _lostFeedback;
        
        [HideInInspector]
        public MMFeedbacks normalFallFeedback;
        [HideInInspector]
        public MMFeedbacks hardFallFeedback;
        [HideInInspector]
        public MMFeedbacks landingFeedback;
        [HideInInspector]
        public MMFeedbacks jumpingFeedback;
        [HideInInspector]
        public MMFeedbacks skiingFeedback;
        [HideInInspector]
        public MMFeedbacks trickFeedback;
        [HideInInspector]
        public MMFeedbacks victoryFeedback;
        [HideInInspector]
        public MMFeedbacks lostFeedback;

        public void StartFeedbacks(Transform player)
        {
            foreach (var field in typeof(PlayerMovimentationFeedbacks).GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if(field.Name[0] != '_') continue;

                MMFeedbacks feedbacks = (MMFeedbacks) field.GetValue(this);
                MMFeedbacks instance = Instantiate(feedbacks.gameObject, player).GetComponent<MMFeedbacks>();

                this.GetType().GetField(field.Name.Remove(0, 1)).SetValue(this, instance);
            }
        }
    }
}
