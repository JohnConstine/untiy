using UnityEngine;

namespace SG1
{
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class AnchorPosition : VectorBinding
    {
        [SerializeField] private Vector2 m_Offset = Vector2.zero;

        [SerializeField, InspectorReadOnly] private RectTransform m_RectTransform;

        public Vector2 Offset
        {
            get { return m_Offset; }
            set { m_Offset = value; }
        }

        public RectTransform RectTransform
        {
            get { return m_RectTransform; }
            set { m_RectTransform = value; }
        }

        protected override void ApplyNewValue(Vector3 newValue)
        {
            m_RectTransform.anchoredPosition3D = newValue;
        }

        protected override void OnEditorValue()
        {
            base.OnEditorValue();
            if (m_RectTransform == null)
            {
                m_RectTransform = GetComponent<RectTransform>();
            }
        }
    }
}