using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SG1.Editor
{
    [CustomEditor(typeof(ActionBinding), editorForChildClasses: true)]
    [CanEditMultipleObjects]
    public class ActionBindingEditor : SingleBindingEditor
    {
        private static Dictionary<string, List<string>> Actions = new Dictionary<string, List<string>>();

        private int m_SelectedModelType = 0;

        private int m_SelectedIndex = 0;

        private bool m_Select = false;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawActionSelect();
        }

        private void InitActions()
        {
            Actions.Clear();

            Actions.Add(string.Empty, new List<string> {string.Empty});

            foreach (var type in (from o in typeof(UGuiFormModel).Assembly.GetTypes()
                where o.IsClass && !o.IsAbstract &&
                      (o.IsSubclassOf(typeof(UGuiFormModel)) || o.IsSubclassOf(typeof(ItemContext))) && o.IsPublic
                select o))
            {
                List<string> names =
                    (from method in type.GetMethods()
                        where method.GetParameters().Length == 0 && !method.IsSpecialName &&
                              method.ReturnType == typeof(void)
                        select method.Name).ToList();

                string key = string.Empty;
                if (type.IsSubclassOf(typeof(UGuiFormModel)))
                {
                    key = type.Name + " (UGuiFormModel)";
                }
                else
                {
                    key = type.Name + " (ItemContext)";
                }

                Actions[key] = names;
            }
        }

        protected void DrawActionSelect()
        {
            if (Application.isPlaying)
            {
                return;
            }

            GUILayout.BeginVertical("box");
            {
                if (Application.isPlaying)
                {
                    return;
                }

                GUILayout.BeginVertical("box");
                {
                    if (EditorGUILayout.DropdownButton(new GUIContent(m_Select ? "Close Select" : "Select Action"),
                        m_Select ? FocusType.Passive : FocusType.Keyboard))
                    {
                        m_Select = !m_Select;
                    }

                    if (m_Select)
                    {
                        InitActions();

                        var keys = Actions.Keys.ToArray();

                        m_SelectedModelType = EditorGUILayout.Popup("Model", m_SelectedModelType, keys);

                        if (keys.Length > m_SelectedModelType)
                        {
                            m_SelectedIndex = EditorGUILayout.Popup("Action", m_SelectedIndex,
                                Actions[keys[m_SelectedModelType]].ToArray());
                        }

                        if (GUILayout.Button("Confirm"))
                        {
                            m_Path.stringValue = Actions[keys[m_SelectedModelType]][m_SelectedIndex];

                            serializedObject.ApplyModifiedProperties();
                        }
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
        }
    }
}