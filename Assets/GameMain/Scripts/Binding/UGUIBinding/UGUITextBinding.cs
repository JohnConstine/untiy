using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace SG1
{
    [RequireComponent(typeof(Text))]
    [DisallowMultipleComponent]
    public class UGUITextBinding : TextBinding
    {
        [SerializeField, InspectorReadOnly] private Text m_Text;

        public Text Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        protected override bool Bind()
        {
            if (!base.Bind())
            {
                m_Text.text = string.Empty;
            }
            return PropertyFound;
        }

        protected override void ApplyNewValue(string newValue)
        {
            m_Text.text = newValue;
        }

        protected override void OnEditorValue()
        {
            base.OnEditorValue();
            if (m_Text == null)
            {
                m_Text = GetComponent<Text>();
            }
        }
    }
}