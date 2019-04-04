using System;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace SG1.Editor
{
    [CustomEditor(typeof(BooleanBinding), editorForChildClasses: true)]
    public class BooleanBindingEditor : PropertyBindingEditor
    {
        protected SerializedProperty m_Reference;
        protected SerializedProperty m_ReferenceMin;
        protected SerializedProperty m_ReferenceMax;
        protected SerializedProperty m_StringReference;
        protected SerializedProperty m_CheckType;
        AnimBool m_ReferenceAnimBool;
        AnimBool m_Reference_Min_Max_AnimBool;
        AnimBool m_StringReferenceAnimBool;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Reference = serializedObject.FindProperty("m_Reference");
            m_ReferenceMin = serializedObject.FindProperty("m_ReferenceMin");
            m_ReferenceMax = serializedObject.FindProperty("m_ReferenceMax");
            m_StringReference = serializedObject.FindProperty("m_StringReference");
            m_CheckType = serializedObject.FindProperty("m_CheckType");

            var typeEnum = (Check_Type) m_CheckType.enumValueIndex;

            m_ReferenceAnimBool = new AnimBool(!m_Reference.hasMultipleDifferentValues
                                               && (typeEnum == Check_Type.EqualToReference
                                                   || typeEnum == Check_Type.LessThanReference
                                                   || typeEnum == Check_Type.GreaterThanReference));

            m_Reference_Min_Max_AnimBool = new AnimBool(!m_Reference.hasMultipleDifferentValues
                                                        && (typeEnum == Check_Type.Between));

            m_StringReferenceAnimBool = new AnimBool(!m_Reference.hasMultipleDifferentValues
                                                     && (typeEnum == Check_Type.EqualToString));

            m_ReferenceAnimBool.valueChanged.AddListener(Repaint);
            m_Reference_Min_Max_AnimBool.valueChanged.AddListener(Repaint);
        }

        protected override void OnDisable()
        {
            m_ReferenceAnimBool.valueChanged.RemoveListener(Repaint);
            m_Reference_Min_Max_AnimBool.valueChanged.RemoveListener(Repaint);
            base.OnDisable();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawReference();
        }

        protected void DrawReference()
        {
            serializedObject.Update();

            Check_Type typeEnum = (Check_Type) m_CheckType.enumValueIndex;

            m_ReferenceAnimBool.target = !m_Reference.hasMultipleDifferentValues
                                         && (typeEnum == Check_Type.EqualToReference
                                             || typeEnum == Check_Type.LessThanReference
                                             || typeEnum == Check_Type.GreaterThanReference);

            m_Reference_Min_Max_AnimBool.target = !m_Reference.hasMultipleDifferentValues
                                                  && (typeEnum == Check_Type.Between);

            m_StringReferenceAnimBool.target = !m_Reference.hasMultipleDifferentValues
                                               && (typeEnum == Check_Type.EqualToString);

            if ((Check_Type) m_CheckType.enumValueIndex == Check_Type.EqualToReference
                || (Check_Type) m_CheckType.enumValueIndex == Check_Type.GreaterThanReference
                || (Check_Type) m_CheckType.enumValueIndex == Check_Type.LessThanReference)
            {
                if (EditorGUILayout.BeginFadeGroup(m_ReferenceAnimBool.faded))
                {
                    EditorGUILayout.PropertyField(m_Reference);
                }

                EditorGUILayout.EndFadeGroup();
            }
            else if ((Check_Type) m_CheckType.enumValueIndex == Check_Type.Between)
            {
                if (EditorGUILayout.BeginFadeGroup(m_Reference_Min_Max_AnimBool.faded))
                {
                    EditorGUILayout.PropertyField(m_ReferenceMin);
                    EditorGUILayout.PropertyField(m_ReferenceMax);
                }

                EditorGUILayout.EndFadeGroup();
            }
            else if ((Check_Type) m_CheckType.enumValueIndex == Check_Type.EqualToString)
            {
                if (EditorGUILayout.BeginFadeGroup(m_StringReferenceAnimBool.faded))
                {
                    EditorGUILayout.PropertyField(m_StringReference);
                }

                EditorGUILayout.EndFadeGroup();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}