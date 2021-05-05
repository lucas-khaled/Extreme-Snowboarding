using System;
using System.Reflection;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Script.Items.Effects
{
    [System.Serializable]
    public class Application
    {
        [FormerlySerializedAs("name")] [SerializeField] [OnValueChanged("OnPropertyValueChanged")]
        private ExposedPlayerProperty propertyName;

        [SerializeField] [ShowIf("propertyType", PropertyType.FLOAT)] [AllowNesting]
        private float floatValue;

        [SerializeField] [ShowIf("propertyType", PropertyType.STRING)] [AllowNesting]
        private string stringValue;

        [SerializeField] [ShowIf("propertyType", PropertyType.BOOL)] [AllowNesting]
        private bool boolValue;

        [SerializeField] [ShowIf("propertyType", PropertyType.OBJECT)] [AllowNesting]
        private Object objectValue;
        
        [SerializeField]
        private EffectMode effectMode;

        private PropertyType propertyType;

        public object InitialValue { get; private set; }

        public object ChangeValue
        {
            get
            {
                switch (propertyType)
                {
                    case PropertyType.FLOAT:
                        return floatValue;
                        break;
                    case PropertyType.BOOL:
                        return boolValue;
                        break;
                    case PropertyType.STRING:
                        return stringValue;
                        break;
                    case PropertyType.OBJECT:
                        return objectValue;
                        break;
                    default:
                        return null;
                        break;
                }
            }
        }
        
        public object PuttedValue { get; private set; }

        public string PropertyName
        {
            get => propertyName.name;
            set => propertyName.name = value;
        }

        public float FloatValue
        {
            get => floatValue;
            set => floatValue = value;
        }

        public string StringValue
        {
            get => stringValue;
            set => stringValue = value;
        }

        public bool BoolValue
        {
            get => boolValue;
            set => boolValue = value;
        }

        public Object ObjectValue
        {
            get => objectValue;
            set => objectValue = value;
        }

        public EffectMode EffectMode
        {
            get => effectMode;
            set => effectMode = value;
        }

        
        void OnPropertyValueChanged()
        {
            Type t = typeof(PlayerSharedValues);
            PropertyInfo p = t.GetProperty(propertyName.name);
            
            if (p != null)
            {
                if (p.PropertyType == typeof(float) || p.PropertyType == typeof(int))
                    propertyType = PropertyType.FLOAT;
                else if (p.PropertyType == typeof(string))
                    propertyType = PropertyType.STRING;
                else if (p.PropertyType == typeof(bool))
                    propertyType = PropertyType.BOOL;
                else
                    propertyType = PropertyType.OBJECT;
            }
            else
            {
                propertyType = PropertyType.NULL;
            }
        }
        
        /// <summary>
        /// Applies the effect and returns the initial value in the specified property
        /// </summary>
        /// <param name="player">The player that the effect must be applied</param>
        public void ApplyEffect(Player player)
        {
            PropertyInfo property = player.SharedValues.GetType().GetProperty(propertyName.name);
            InitialValue = property.GetValue(player.SharedValues);

            object putValue = ChangeValue;
            
            if (propertyType == PropertyType.FLOAT)
            {
                switch (effectMode)
                {
                    case EffectMode.ADD:
                        putValue = (property.PropertyType == typeof(int)) ? (int)InitialValue + (int)ChangeValue : (float)InitialValue + (float)ChangeValue;
                        break;
                    case EffectMode.MULTIPLY:
                        putValue = (property.PropertyType == typeof(int)) ? (int)InitialValue * (int)ChangeValue : (float)InitialValue * (float)ChangeValue;
                        break;
                }
            }
            
            else if(propertyType == PropertyType.STRING && effectMode == EffectMode.ADD)
                putValue = (string) InitialValue + (string) ChangeValue;

            PuttedValue = putValue;
            property.SetValue(player.SharedValues, putValue);
        }
        
        
    }
    
    public enum EffectMode { ADD, MULTIPLY, REPLACE }
}