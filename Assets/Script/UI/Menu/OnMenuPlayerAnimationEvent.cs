using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ExtremeSnowboarding.Script.UI.Menu
{
    public class OnMenuPlayerAnimationEvent : MonoBehaviour
    {
        [SerializeField] private List<MenuAnimationEvent> menuAnimations;
        
        public void CallAnimationEvent(string animationName)
        {
            List<MenuAnimationEvent> animations = menuAnimations.FindAll((x) => x.AnimationName == animationName);
            
            if(animations.Count == 0 || animations == null)
                return;

            foreach (MenuAnimationEvent animation in animations)
            {
                animation.CallEvent();
            }
        }
    }

    [System.Serializable]
    public struct MenuAnimationEvent
    {
        [SerializeField] private string animationName;
        [SerializeField] private UnityEvent<string> onAnimationEventEnter;

        public string AnimationName => animationName;

        public void CallEvent()
        {
            onAnimationEventEnter.Invoke(animationName);
        }
    }
}