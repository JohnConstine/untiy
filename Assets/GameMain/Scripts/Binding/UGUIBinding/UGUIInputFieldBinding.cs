using UnityEngine;
using UnityEngine.UI;

namespace SG1
{
    [RequireComponent(typeof(InputField))]
    [DisallowMultipleComponent]
    public class UGUIInputFieldBinding : TextBinding
    {
        [SerializeField, InspectorReadOnly] private InputField m_InputField;

        public InputField InputField
        {
            get { return m_InputField; }
            set { m_InputField = value; }
        }

        protected override bool Bind()
        {
            m_InputField.onValueChanged.AddListener(ApplyInputValue);
            return base.Bind();
        }

        protected override void Unbind()
        {
            base.Unbind();
            m_InputField.onValueChanged.RemoveListener(ApplyInputValue);
        }

        protected override void ApplyNewValue(string newValue)
        {
            m_InputField.text = newValue;
        }

        protected override void OnEditorValue()
        {
            base.OnEditorValue();
            if (m_InputField == null)
            {
                m_InputField = gameObject.GetComponent<InputField>();
            }
        }
    }
}