using UnityEngine;
using UnityEngine.UI;

namespace SG1
{
    [RequireComponent(typeof(Button))]
    public class UGUIOnClickBinding : ActionBinding
    {
        private float m_LastClickTime;

        [SerializeField] private bool m_Block;

        [SerializeField] private float m_ClickInterval = 0.5f;

        public Button Button;

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

        private void Reset()
        {
            OnValidate();
        }
        
        private void OnValidate()
        {
            if (Button == null)
            {
                Button = GetComponent<Button>();
            }
        }
        
        private void Awake()
        {
            OnValidate();
            if (Button != null)
            {
                Button.onClick.AddListener(OnClick);
            }
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
    }
}