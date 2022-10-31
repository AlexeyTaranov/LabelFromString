using DefaultNamespace;
using UnityEditor;
using UnityEngine;

namespace LabelFromString.Editor
{
    [CustomPropertyDrawer(typeof(LabelFromStringAttribute))]
    public class LabelFromStringPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var propertyNameFromString = GetPropertyName(property);
            label.text = propertyNameFromString ?? label.text;

            EditorGUI.PropertyField(rect, property, label, true);

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        private string GetPropertyName(SerializedProperty property)
        {
            var propertyValue = GetValueFromProperty(property);
            return propertyValue?.ToString();
        }

        private static object GetValueFromProperty(SerializedProperty property)
        {
#if UNITY_2021_2_OR_NEWER
            if (property.propertyType == SerializedPropertyType.ManagedReference)
            {
                return property.managedReferenceValue;
            }

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                return property.objectReferenceValue;
            }

            if (property.propertyType == SerializedPropertyType.Generic)
            {
                return property.boxedValue;
            }

            return null;
#else
            return property.GetTarget();
#endif
        }
    }
}
