using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ExtremeSnowboarding.Script.UI.General
{
    public class ExtremeToggleGroup : ToggleGroup
    {
        public UnityEvent<string> OnToggleChanged;
        
        public Toggle ReturnActiveToggle()
        {
            return m_Toggles.Find(x => x.isOn);
        }

        protected override void Start()
        {
            base.Start();
            foreach (var toggle in m_Toggles)
            {
                Debug.Log(name+" - Toggle: "+toggle
                    .name);
                toggle.onValueChanged.AddListener(delegate(bool arg0) { if(toggle.isOn) OnToggleChanged?.Invoke(toggle.name); });
            }
        }
        
    }
}