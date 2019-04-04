using System;
using UnityEditor;

namespace SG1.Editor
{
    [CustomEditor(typeof(BaseBinding), editorForChildClasses: true)]
    public class BaseBindingEditor : UnityEditor.Editor
    {
        private bool m_IsCompiling = false;
        
        protected SerializedProperty m_ModelView;

        protected virtual void OnEnable()
        {
            m_ModelView = serializedObject.FindProperty("m_ModelView");
        }

        protected virtual void OnDisable()
        {
        }


        /// <summary>
        /// 绘制事件。
        /// </summary>
        public override void OnInspectorGUI()
        {
            if (m_IsCompiling && !EditorApplication.isCompiling)
            {
                m_IsCompiling = false;
                OnCompileComplete();
            }
            else if (!m_IsCompiling && EditorApplication.isCompiling)
            {
                m_IsCompiling = true;
                OnCompileStart();
            }

            DrawDefaultInspector();
        }

        /// <summary>
        /// 编译开始事件。
        /// </summary>
        protected virtual void OnCompileStart()
        {
        }

        /// <summary>
        /// 编译完成事件。
        /// </summary>
        protected virtual void OnCompileComplete()
        {
        }
    }
}