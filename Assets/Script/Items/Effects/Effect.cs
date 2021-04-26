using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Script.EventSystem;
using Script.Items.Effects;
using UnityEngine;
using Application = Script.Items.Effects.Application;

[System.Serializable]
public struct Effect
{
    [SerializeField]
    private float timeOfChange;

    [SerializeField] private Quantification quantification;
    [SerializeField] private Application application;
    [SerializeField] private Recuperation recuperation;
    
    private Player player;

    public int ID { get; }

    public void StartEffect(Player player)
    {
        this.player = player;
        quantification.quantificationCallback += EffectIteration;
        
        if(EffectGeneralEvents.onEffectStarted != null)
            EffectGeneralEvents.onEffectStarted.Invoke(this, player);
        
        quantification.StartQuantification(timeOfChange);
    }

    private void EffectIteration(Quantification.QuantificationCallbackType callback)
    {
        switch (callback)
        {
            case Quantification.QuantificationCallbackType.APPLICATION:
                application.ApplyEffect(player);
                break;
            case Quantification.QuantificationCallbackType.RECUPERATON:
                recuperation.StartRecuperation(player, application.PropertyName, application.InitialValue, application.ChangeValue, application.EffectMode);
                break;
                
        }
    }

    #region Constructors

    public Effect(string name, float floatValue, float timeOfChange, EffectMode effectMode) : this()
    {
        this.application.PropertyName = name;
        this.application.FloatValue = floatValue;
        this.timeOfChange = timeOfChange;
        this.application.EffectMode = effectMode;
    }

    public Effect(string name, string stringValue, float timeOfChange) : this()
    {
        this.application.PropertyName = name;
        this.application.StringValue = stringValue;
        this.timeOfChange = timeOfChange;
        
    }

    public Effect(string name, bool boolValue, float timeOfChange) : this()
    {
        this.application.PropertyName = name;
        this.application.BoolValue = boolValue;
        this.timeOfChange = timeOfChange;
    }

    #endregion
}
