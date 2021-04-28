using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Script.Items.Effects
{
    [Serializable]
    public class Conditioning
    {
        [SerializeField] private Object conditionObject;
        [SerializeField] private ConditionBlock<Object> conditions;
        
        public bool IsConditioned(Player player)
        {
            if (!conditions.HasConditions())
                return true;

            return conditions.GetFinalCondition();
        }
    }

    [Serializable]
    public class ConditionBlock<TClass> where TClass : class
    {
        [SerializeField] private List<Condition> conditions;

        private Expression<Func<bool>> finalExpression = null;
        
        private bool isBuilt = false;
        public bool IsBuilt => isBuilt;

        public bool HasConditions()
        {
            return conditions.Count > 0;
        }

        public bool GetFinalCondition()
        {
            if(!isBuilt)
                BuildExpression();

            Func<bool> func = finalExpression.Compile();
            return func();
        }
        
        void BuildExpression()
        {
            Expression returnExpression = Expression.Constant(true);
            var parameter = Expression.Parameter(typeof(TClass), "x");
            foreach (Condition condition in conditions)
            {
                var member = Expression.Property(parameter, condition.propertyName);
                var constant = Expression.Constant(condition.value);

                Expression expression = null;
                switch (condition.operation)
                {
                    case Operation.EQUAL_TO:
                        expression = Expression.Equal(member, constant);
                        break;
                    case Operation.LESS_THAN:
                        expression = Expression.LessThan(member, constant);
                        break;
                    case Operation.GREATER_THAN:
                        expression = Expression.GreaterThan(member, constant);
                        break;
                    case Operation.NOT_EQUAL_TO:
                        expression = Expression.NotEqual(member, constant);
                        break;
                    case Operation.LESS_THAN_OR_EQUAL_TO:
                        expression = Expression.LessThanOrEqual(member, constant);
                        break;
                    case Operation.GREATER_THAN_OR_EQUAL_TO:
                        expression = Expression.GreaterThanOrEqual(member, constant);
                        break;
                }

                returnExpression = Expression.AndAlso(finalExpression, expression);
            }

            finalExpression = (Expression<Func<bool>>) returnExpression;
            isBuilt = true;
        }
    }

    [Serializable]
    public struct Condition
    {
        public string propertyName;
        public Operation operation;
        public float value;
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

}