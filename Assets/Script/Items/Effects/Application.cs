using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Items.Effects
{
    [System.Serializable]
    public class Application
    {
        [FormerlySerializedAs("name")] [SerializeField]
        private string propertyName;

        [SerializeField]
        private float floatValue;

        [SerializeField]
        private string stringValue;

        [SerializeField]
        private bool boolValue;

        [SerializeField] private object objectValue;
        
        [SerializeField]
        private EffectMode effectMode;
        
        private enum PropertyType
        {
            INT,
            FLOAT,
            STRING,
            BOOL,
            OTHER
        }

        private PropertyType type;

        public object InitialValue { get; private set; }
        public object ChangeValue { get; private set; }
        public object PuttedValue { get; private set; }

        public string PropertyName
        {
            get => propertyName;
            set => propertyName = value;
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

        public object ObjectValue
        {
            get => objectValue;
            set => objectValue = value;
        }

        public EffectMode EffectMode
        {
            get => effectMode;
            set => effectMode = value;
        }

        /// <summary>
        /// Applies the effect and returns the initial value in the specified property
        /// </summary>
        /// <param name="player">The player that the effect must be applied</param>
        public void ApplyEffect(Player player)
        {
            PropertyInfo property = player.SharedValues.GetType().GetProperty(propertyName);
            SetPropertyType(property);
            SetChangeValue();
            InitialValue = property.GetValue(player.SharedValues);

            object putValue = ChangeValue;
            
            if (type == PropertyType.INT || type == PropertyType.FLOAT)
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
            
            else if(type == PropertyType.STRING && effectMode == EffectMode.ADD)
                putValue = (string) InitialValue + (string) ChangeValue;

            PuttedValue = putValue;
            property.SetValue(player.SharedValues, putValue);
        }

        private void SetPropertyType(PropertyInfo property)
        {
            Type tipo = property.PropertyType;
            if (tipo == typeof(int))
                type = PropertyType.INT;
            else if (tipo == typeof(float))
                type = PropertyType.FLOAT;
            else if (tipo == typeof(string))
                type = PropertyType.STRING;
            else if (tipo == typeof(bool))
                type = PropertyType.BOOL;
            else
                type = PropertyType.OTHER;
        }

        private void SetChangeValue()
        {
            switch (type)
            {
                case PropertyType.INT:
                    ChangeValue = (int) floatValue;
                    break;
                case PropertyType.FLOAT:
                    ChangeValue = floatValue;
                    break;
                case PropertyType.STRING:
                    ChangeValue = stringValue;
                    break;
                case PropertyType.BOOL:
                    ChangeValue = boolValue;
                    break;
                case PropertyType.OTHER:
                    ChangeValue = objectValue;
                    break;
            }
        }
    }
    
    public enum EffectMode { ADD, MULTIPLY, REPLACE }
}