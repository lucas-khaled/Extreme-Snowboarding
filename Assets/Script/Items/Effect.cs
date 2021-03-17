using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;



[System.Serializable]
public struct Effect
{
    [SerializeField]
    private string name;

    [SerializeField]
    private float floatValue;

    [SerializeField]
    private string stringValue;

    [SerializeField]
    private bool boolValue;

    [SerializeField]
    private EffectMode effectMode;



    [SerializeField]
    private float timeOfChange;

    public IEnumerator StartEffect(Player player)
    {
        Debug.Log("Applying Effect");
        PropertyInfo accessProperty = player.SharedValues.GetType().GetProperty(name);

        var initialValue = accessProperty.GetValue(player.SharedValues);

        ChangeObjPropertyValue(accessProperty, player.SharedValues);

        Debug.Log("Started effect: " + name);

        yield return new WaitForSeconds(timeOfChange);

        ReturnValue(accessProperty, player.SharedValues, initialValue);

        Debug.Log("finished effect: " + name);
    }

    void ChangeObjPropertyValue(PropertyInfo accessProperty, object obj)
    {
        if (accessProperty.PropertyType == typeof(int))
            accessProperty.SetValue(obj, Mathf.RoundToInt(floatValue));

        else if (accessProperty.PropertyType == typeof(float))
        {
            switch (effectMode)
            {
                case EffectMode.REPLACE:
                    accessProperty.SetValue(obj, floatValue);
                    break;
                case EffectMode.ADD:
                    accessProperty.SetValue(obj, (float)accessProperty.GetValue(obj) + floatValue);
                    break;
                case EffectMode.MULTIPLY:
                    accessProperty.SetValue(obj, (float)accessProperty.GetValue(obj) * floatValue);
                    break;
            }
        }

        else if (accessProperty.PropertyType == typeof(string))
            accessProperty.SetValue(obj, stringValue);

        else if (accessProperty.PropertyType == typeof(bool))
            accessProperty.SetValue(obj, boolValue);
    }

    void ReturnValue(PropertyInfo accessProperty, object obj, object initialValue)
    {
        if (accessProperty.PropertyType == typeof(int))
            accessProperty.SetValue(obj, (int)initialValue);

        else if (accessProperty.PropertyType == typeof(float))
        {
            switch (effectMode)
            {
                case EffectMode.REPLACE:
                    accessProperty.SetValue(obj, initialValue);
                    break;
                case EffectMode.ADD:
                    accessProperty.SetValue(obj, (float)accessProperty.GetValue(obj) - floatValue);
                    break;
                case EffectMode.MULTIPLY:
                    accessProperty.SetValue(obj, (float)accessProperty.GetValue(obj) / floatValue);
                    break;
            }
        }

        else if (accessProperty.PropertyType == typeof(string))
            accessProperty.SetValue(obj, initialValue);

        else if (accessProperty.PropertyType == typeof(bool))
            accessProperty.SetValue(obj, initialValue);
    }

    #region Constructors

    public Effect(string name, float floatValue, float timeOfChange, EffectMode effectMode) : this()
    {
        this.name = name;
        this.floatValue = floatValue;
        this.timeOfChange = timeOfChange;
        this.effectMode = effectMode;
    }

    public Effect(string name, string stringValue, float timeOfChange) : this()
    {
        this.name = name;
        this.stringValue = stringValue;
        this.timeOfChange = timeOfChange;
        
    }

    public Effect(string name, bool boolValue, float timeOfChange) : this()
    {
        this.name = name;
        this.boolValue = boolValue;
        this.timeOfChange = timeOfChange;
    }

    #endregion

    public enum EffectMode { ADD, MULTIPLY, REPLACE }
}
