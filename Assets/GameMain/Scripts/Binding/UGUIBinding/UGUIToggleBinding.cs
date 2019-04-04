using UnityEngine;
using UnityEngine.UI;

namespace SG1
{
    [RequireComponent(typeof(Toggle))]
    [DisallowMultipleComponent]
    public class UGUIToggleBinding : BooleanBinding
    {
        [SerializeField, InspectorReadOnly] private Toggle m_Toggle;

        public Toggle Toggle
        {
            get { return m_Toggle; }
            set { m_Toggle = value; }
        }


        protected override bool Bind()
        {
            m_Toggle.onValueChanged.AddListener(ApplyInputValue);
            return base.Bind();
        }

        protected override void Unbind()
        {
            base.Unbind();
            m_Toggle.onValueChanged.RemoveListener(ApplyInputValue);
        }

        protected override void ApplyNewValue(bool newValue)
        {
            m_Toggle.isOn = newValue;
        }

        protected override void OnEditorValue()
        {
            base.OnEditorValue();
            if (m_Toggle == null)
            {
                m_Toggle = GetComponent<Toggle>();
            }
        }
    }
}