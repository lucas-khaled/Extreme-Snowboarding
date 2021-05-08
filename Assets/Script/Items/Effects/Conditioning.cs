using System;
using System.Collections.Generic;
using System.Reflection;
using ExtremeSnowboarding.Script.Player;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace ExtremeSnowboarding.Script.Items.Effects
{
    [Serializable]
    public class Conditioning
    {

        [FormerlySerializedAs("conditions")] [SerializeField] private ConditionBlock<PlayerSharedValues> conditionBlock;
        
        public bool IsConditioned(Player.Player player)
        {
            return conditionBlock.GetFinalCondition(player.SharedValues);
        }
    }

    [Serializable]
    public class ConditionBlock<TClass> where TClass : class
    {
        [SerializeField] private List<Condition> conditions = new List<Condition>();
        

        public bool HasConditions()
        {
            return conditions.Count > 0;
        }

        private TClass myObject;

        public bool GetFinalCondition(TClass myObject)
        {
            if (!HasConditions())
                return true;
            
            this.myObject = myObject;
            return BuildExpression();
        }
        
        bool BuildExpression()
        {
            bool returnExpression = true;
            bool first = true;
            foreach (Condition condition in conditions)
            {
                var member = myObject.GetType().GetProperty(condition.propertyName.name);

                bool expression = false;
                switch (condition.operation)
                {
                    case Operation.EQUAL_TO:
                        expression = (member.GetValue(myObject) == condition.Value);
                        break;
                    case Operation.LESS_THAN:
                        expression = ((float)member.GetValue(myObject) < (float)condition.Value);
                        break;
                    case Operation.GREATER_THAN:
                        expression =((float)member.GetValue(myObject) > (float)condition.Value);
                        break;
                    case Operation.NOT_EQUAL_TO:
                        expression = (member.GetValue(myObject) != condition.Value);
                        break;
                    case Operation.LESS_THAN_OR_EQUAL_TO:
                        expression = ((float)member.GetValue(myObject) <= (float)condition.Value);
                        break;
                    case Operation.GREATER_THAN_OR_EQUAL_TO:
                        expression = ((float)member.GetValue(myObject) >= (float)condition.Value);
                        break;
                }

                Debug.Log(member.GetValue(myObject));
                
                if (first)
                {
                    returnExpression = expression;
                    first = false;
                }
                else
                {
                    if (condition.conector == Conector.AND)
                        returnExpression &= expression;
                    else
                        returnExpression |= expression;
                }
            }
            return returnExpression;
        }
    }

    [Serializable]
    public class Condition
    {
        [OnValueChanged("OnPropertyValueChanged")] 
        public ExposedPlayerProperty propertyName;
        public Operation operation;
        
        [SerializeField] [FormerlySerializedAs("value")] [ShowIf("propertyType", PropertyType.FLOAT)] [AllowNesting]
        private float floatValue;
        [SerializeField] [ShowIf("propertyType", PropertyType.STRING)] [AllowNesting]
        private string stringValue;
        [SerializeField] [ShowIf("propertyType", PropertyType.BOOL)] [AllowNesting]
        private bool boolValue;
        [SerializeField] [ShowIf("propertyType", PropertyType.OBJECT)] [AllowNesting]
        private Object objectValue;
        
        public Conector conector;
        
        PropertyType propertyType = PropertyType.NULL;

        public object Value
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
        
        void OnPropertyValueChanged()
        {
            Type t = typeof(PlayerSharedValues);
            PropertyInfo p = t.GetProperty(propertyName.name);
            
            if (p != null)
            {
                Debug.Log(p.PropertyType);
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
            Debug.Log(propertyType.ToString());
        }
    }

    [System.Serializable]
    public class ExposedPlayerProperty
    {
        public string name;
    }

    public enum PropertyType
    {
        FLOAT,
        BOOL,
        STRING,
        OBJECT,
        NULL
    }
    
    public enum Operation
    {
        EQUAL_TO,
        NOT_EQUAL_TO,
        GREATER_THAN,
        LESS_THAN,
        GREATER_THAN_OR_EQUAL_TO,
        LESS_THAN_OR_EQUAL_TO
    }

    public enum Conector
    {
        AND, 
        OR
    }
}