using UnityEngine;
using UnityEngine.UI;

namespace SG1
{
    [RequireComponent(typeof(Selectable))]
    [DisallowMultipleComponent]
    public class UGUIInteractableBinding : BooleanBinding
    {
        [SerializeField, InspectorReadOnly] private Selectable m_Selectable;

        public Selectable Selectable
        {
            get { return m_Selectable; }
            set { m_Selectable = value; }
        }

        protected override void ApplyNewValue(bool newValue)
        {
            m_Selectable.interactable = newValue;
        }

        protected override void OnEditorValue()
        {
            base.OnEditorValue();
            if (m_Selectable == null)
            {
                m_Selectable = gameObject.GetComponent<Selectable>();
            }
        }
    }
}