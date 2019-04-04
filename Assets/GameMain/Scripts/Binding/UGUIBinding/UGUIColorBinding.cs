using UnityEngine;
using UnityEngine.UI;

namespace SG1
{
    [RequireComponent(typeof(Graphic))]
    [DisallowMultipleComponent]
    public class UGUIColorBinding : ColorBinding
    {
        [InspectorReadOnly] [SerializeField] private Graphic m_Graphic;

        public Graphic Graphic
        {
            get { return m_Graphic; }
            set { m_Graphic = value; }
        }

        protected override void ApplyNewValue(Color newColor)
        {
            m_Graphic.color = newColor;
        }

        protected override void OnEditorValue()
        {
            base.OnEditorValue();
            if (m_Graphic == null)
            {
                m_Graphic = GetComponent<Graphic>();
            }
        }
    }
}