using System;
using System.Reflection;
using UnityEngine;

namespace Script.Items.Effects
{
    [System.Serializable]
    public class Recuperation
    {

        [SerializeField] private bool haveRecuperation = true;

        public void StartRecuperation(Player.Player player, string propertyName, object oldValue, object newValue, EffectMode mode)
        {
            if(haveRecuperation)
                RecuperateValues(player, propertyName, oldValue, newValue, mode);
        }
        
        private void RecuperateValues(Player.Player player, string propertyName, object oldValue, object newValue, EffectMode mode)
        {
            object obj = player.SharedValues;
            PropertyInfo property = obj.GetType().GetProperty(propertyName);
            object returnValue = oldValue;
            
            if (property.PropertyType == typeof(float) || property.PropertyType == typeof(int))
            {
                switch (mode)
                {
                    case EffectMode.ADD:
                        returnValue = (property.PropertyType == typeof(int))
                            ? (int) property.GetValue(obj) - (int) newValue
                            : (float) property.GetValue(obj) - (float) newValue;
                        break;
                    case EffectMode.MULTIPLY:
                        returnValue = (property.PropertyType == typeof(int))
                            ? (int) property.GetValue(obj) / (int) newValue
                            : (float) property.GetValue(obj) / (float) newValue;
                        break;
                }
            }

            else if (property.PropertyType == typeof(string) && mode == EffectMode.ADD)
            {
                string actualStringValue = (string) property.GetValue(obj);
                string newStringValue = (string) newValue;
                returnValue = actualStringValue.Replace(newStringValue, string.Empty);
            }

            property.SetValue(obj, returnValue);
        }
    }
}