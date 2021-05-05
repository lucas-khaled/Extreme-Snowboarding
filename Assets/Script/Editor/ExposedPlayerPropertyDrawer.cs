using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NaughtyAttributes.Editor;
using Script.Attributes;
using Script.Items.Effects;
using UnityEditor;
using UnityEngine;

namespace Script.Editor
{
    [CustomPropertyDrawer(typeof(ExposedPlayerProperty))]
    public class ExposedPlayerPropertyDrawer : PropertyDrawer
    {
        private List<PropertyInfo> exposedProperties = new List<PropertyInfo>();
        private List<string> exposedNames = new List<string>();
        private bool gotProperties = false;
        private int index;
        
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            if(!gotProperties) GetExposedMembers();

            SerializedProperty serializedProperty = property.FindPropertyRelative("name");
            string beforeValue = serializedProperty.stringValue;
            
            index = GetPropertyIndex(serializedProperty.stringValue);
            index = EditorGUI.Popup(position, index, exposedNames.ToArray());
            
            serializedProperty.stringValue = exposedProperties[index].Name;
            
            if(beforeValue != exposedProperties[index].Name)
                PropertyUtility.CallOnValueChangedCallbacks(property);
        }

        int GetPropertyIndex(string name)
        {
            int count = 0;
            foreach (var property in exposedProperties)
            {
                if (property.Name == name)
                {
                    return count;
                }

                count++;
            }

            return 0;
        }
        
        void GetExposedMembers()
        {
            Assembly[] assembies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assembies)
            {
                Type[] types = assembly.GetTypes();

                foreach (Type type in types)
                {
                    BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                    MemberInfo[] members = type.GetMembers(flags);

                    foreach (MemberInfo member in members)
                    {
                        if (member.CustomAttributes.ToArray().Length > 0)
                        {
                            ExposedPropertyAttribute attribute = member.GetCustomAttribute<ExposedPropertyAttribute>();
                            if (attribute != null)
                            {
                                exposedProperties.Add((PropertyInfo)member);
                                exposedNames.Add(attribute.displayName);
                            }
                            
                        }
                    }
                }
            }

            gotProperties = true;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}