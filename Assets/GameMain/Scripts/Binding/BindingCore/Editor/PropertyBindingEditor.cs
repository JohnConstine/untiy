using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Reflection;
using GameFramework;

namespace SG1.Editor
{
    [CustomEditor(typeof(PropertyBinding), editorForChildClasses: true)]
    [CanEditMultipleObjects]
    public class PropertyBindingEditor : SingleBindingEditor
    {
        private static Dictionary<string, List<string>> Properties = new Dictionary<string, List<string>>();

        private int m_SelectedModelType = 0;

        private int m_SelectedIndex = 0;

        private bool m_Select = false;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawPathSelect();
        }

        private void InitProperties()
        {
            Properties.Clear();

            Properties.Add(string.Empty, new List<string> {string.Empty});

            foreach (var type in (from o in typeof(UGuiFormModel).Assembly.GetTypes()
                where o.IsClass && !o.IsAbstract && (o.IsSubclassOf(typeof(UGuiFormModel)) ||
                                                     o.IsSubclassOf(typeof(ItemContext))) && o.IsPublic
                select o))
            {
                List<string> names =
                    (from property in type.GetProperties()
                        where property.Name.EndsWith("Property")
                        select property.Name.Substring(0, property.Name.IndexOf("Property"))).ToList();

                string key = string.Empty;
                if (type.IsSubclassOf(typeof(UGuiFormModel)))
                {
                    key = type.Name + " (UGuiFormModel)";
                }
                else
                {
                    key = type.Name + " (ItemContext)";
                }

                Properties[key] = names;
            }
        }

        protected void DrawPathSelect()
        {
            if (Application.isPlaying)
            {
                return;
            }

            GUILayout.BeginVertical("box");
            {
                if (EditorGUILayout.DropdownButton(new GUIContent(m_Select ? "Close Select" : "Select Property"),
                    m_Select ? FocusType.Passive : FocusType.Keyboard))
                {
                    m_Select = !m_Select;
                }

                if (m_Select)
                {
                    InitProperties();

                    var keys = Properties.Keys.ToArray();

                    m_SelectedModelType = EditorGUILayout.Popup("Model", m_SelectedModelType, keys);

                    if (keys.Length > m_SelectedModelType)
                    {
                        m_SelectedIndex = EditorGUILayout.Popup("Property", m_SelectedIndex,
                            Properties[keys[m_SelectedModelType]].ToArray());
                    }

                    if (GUILayout.Button("Confirm"))
                    {
                        m_Path.stringValue = Properties[keys[m_SelectedModelType]][m_SelectedIndex];

                        serializedObject.ApplyModifiedProperties();
                    }
                }
            }
            GUILayout.EndVertical();
        }
    }
}