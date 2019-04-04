using UnityEditor;

namespace SG1.Editor
{
    [CustomEditor(typeof(SingleBinding), editorForChildClasses: true)]
    public class SingleBindingEditor : BaseBindingEditor
    {
        protected SerializedProperty m_Path;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Path = serializedObject.FindProperty("m_Path");
        }
    }
}