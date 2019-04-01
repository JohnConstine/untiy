using UnityEditor;
using UnityEngine;

namespace SG1.Editor
{
    [CustomPropertyDrawer(typeof(InspectorReadOnlyAttribute))]
    public sealed class InspectorReadOnlyAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = (InspectorReadOnlyAttribute) attribute;
            switch (attr.Mode)
            {
                case InspectorDiplayMode.AlwaysEnabled:
                    GUI.enabled = false;
                    EditorGUI.PropertyField(position, property, label, true);
                    GUI.enabled = true;
                    break;
                case InspectorDiplayMode.DisabledInPlayMode:
                    if (!Application.isPlaying)
                    {
                        GUI.enabled = false;
                    }
                        EditorGUI.PropertyField(position, property, label, true);
                        GUI.enabled = true;
                    break;
                case InspectorDiplayMode.EnabledInPlayMode:
                    if (Application.isPlaying)
                    {
                        GUI.enabled = false;
                    }
                    EditorGUI.PropertyField(position, property, label, true);
                    GUI.enabled = true;
                    break;
            }
        }
    }
}