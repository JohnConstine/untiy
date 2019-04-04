using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SG1
{
    [RequireComponent(typeof(Button))]
    public class UGUIOnClickBinding : ActionBinding
    {
        private float m_LastClickTime;

        [SerializeField] private bool m_Block;

        [SerializeField] private float m_ClickInterval = 0.5f;

        [SerializeField, InspectorReadOnly] private Button m_Button;

        public bool Block
        {
            get { return m_Block; }
            set { m_Block = value; }
        }

        public float ClickInterval
        {
            get { return m_ClickInterval; }
            set { m_ClickInterval = value; }
        }

        public Button Button
        {
            get { return m_Button; }
            set { m_Button = value; }
        }

        protected override void Awake()
        {
            base.Awake();
            m_Button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (m_Action != null && !Block)
            {
                var interval = Time.realtimeSinceStartup - m_LastClickTime;
                if (interval < ClickInterval) return;
                m_LastClickTime = Time.realtimeSinceStartup;
                m_Action.Invoke();
            }
        }

        protected override void OnEditorValue()
        {
            base.OnEditorValue();
            if (m_Button == null)
            {
                m_Button = GetComponent<Button>();
            }
        }
    }
}