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
    private float timeOfChange;

    public IEnumerator StartEffect(Player player)
    {
        PropertyInfo accessProperty = player.SharedValues.GetType().GetProperty(name);

        var initialValue = accessProperty.GetValue(player.SharedValues);

        ChangeObjPropertyValue(accessProperty, player.SharedValues);

        Debug.Log("Started effect: " + name);

        yield return new WaitForSeconds(timeOfChange);

        accessProperty.SetValue(player.SharedValues, initialValue);

        Debug.Log("finished effect: " + name);
    }

    void ChangeObjPropertyValue(PropertyInfo accessProperty, object obj)
    {
        if (accessProperty.PropertyType == typeof(int))
            accessProperty.SetValue(obj, Mathf.RoundToInt(floatValue));

        else if (accessProperty.PropertyType == typeof(float))
            accessProperty.SetValue(obj, floatValue);

        else if (accessProperty.PropertyType == typeof(string))
            accessProperty.SetValue(obj, stringValue);

        else if (accessProperty.PropertyType == typeof(bool))
            accessProperty.SetValue(obj, boolValue);
    }
}
