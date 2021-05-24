using ExtremeSnowboarding.Script.EventSystem;
using NaughtyAttributes;
using UnityEngine;

namespace ExtremeSnowboarding.Script.Items.Effects
{
    [System.Serializable]
    public struct Effect
    {
        [SerializeField]
        private float timeOfChange;

        [HorizontalLine(color: EColor.Black)]
        [SerializeField] private Quantification quantification;
        [HorizontalLine(color: EColor.Black)]
        [SerializeField] private Application application;
        [HorizontalLine(color: EColor.Black)]
        [SerializeField] private Recuperation recuperation;
        [HorizontalLine(color: EColor.Black)]
        [SerializeField] private Conditioning conditioning;
    
        private Player.Player player;
        private bool wasApplied;

        public void StartEffect(Player.Player player)
        {
            this.player = player;
            quantification.quantificationCallback += EffectIteration;
        
            if(EffectGeneralEvents.onEffectStarted != null)
                EffectGeneralEvents.onEffectStarted.Invoke(this, player);
        
            quantification.StartQuantification(timeOfChange);
        }

        public void OnEnable()
        {
            application.OnEnable();
        }

        private void EffectIteration(Quantification.QuantificationCallbackType callback)
        {
            switch (callback)
            {
                case Quantification.QuantificationCallbackType.APPLICATION:
                    if (conditioning.IsConditioned(player))
                    {
                        application.ApplyEffect(player);
                        wasApplied = true;
                    }
                    else
                        wasApplied = false;
                    
                    break;
            
                case Quantification.QuantificationCallbackType.RECUPERATON:
                    if(wasApplied)
                        recuperation.StartRecuperation(player, application.PropertyName, application.InitialValue, application.ChangeValue, application.EffectMode);
                    break;
            }
        }

        #region Constructors

        public Effect(Quantification quantification, Application application, Conditioning conditioning,
            Recuperation recuperation, Player.Player player, float timeOfChange)
        {
            this.application = application;
            this.recuperation = recuperation;
            this.conditioning = conditioning;
            this.quantification = quantification;
            this.timeOfChange = timeOfChange;
            this.player = player;

            wasApplied = false;
        }
    
        public Effect(string name, float floatValue, float timeOfChange, EffectMode effectMode, Player.Player player)
        {
            application = new Application(name, floatValue, effectMode);
            this.timeOfChange = timeOfChange;
            this.player = player;
        
            wasApplied = false;
            quantification = new Quantification();
            conditioning = new Conditioning();
            recuperation = new Recuperation();
        }

        public Effect(string name, string stringValue, float timeOfChange, Player.Player player)
        {
            application = new Application(name, stringValue, EffectMode.REPLACE);
            this.timeOfChange = timeOfChange;
            this.player = player;
        
            wasApplied = false;
            quantification = new Quantification();
            conditioning = new Conditioning();
            recuperation = new Recuperation();
        }

        public Effect(string name, bool boolValue, float timeOfChange, Player.Player player)
        {
            application = new Application(name, boolValue);
            this.timeOfChange = timeOfChange;
            this.player = player;
        
            wasApplied = false;
            quantification = new Quantification();
            conditioning = new Conditioning();
            recuperation = new Recuperation();
        }

        #endregion
    }
}