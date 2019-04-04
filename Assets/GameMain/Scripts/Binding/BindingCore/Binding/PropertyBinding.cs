using UnityEngine;

namespace SG1
{
    public abstract class PropertyBinding : SingleBinding
    {
        private Property m_Property;

        [InspectorReadOnly(InspectorDiplayMode.AlwaysEnabled)] [SerializeField]
        private bool m_PropertyFound;

        protected Property Property
        {
            get { return m_Property; }
            set { m_Property = value; }
        }

        public bool PropertyFound
        {
            get { return m_PropertyFound; }
            protected set { m_PropertyFound = value; }
        }

        protected override bool Bind()
        {
            m_Property = FindProperty();

            if (IsSuitableProperty())
            {
                m_Property.OnValueChange += OnChange;
                m_PropertyFound = true;
            }
            else
            {
                m_PropertyFound = false;
            }

            return m_PropertyFound;
        }

        protected override void Unbind()
        {
            if (m_Property != null)
            {
                m_Property.OnValueChange -= OnChange;
                m_Property = null;
                PropertyFound = false;
            }
        }

        protected virtual Property FindProperty()
        {
            return ModelView == null ? null : ModelView.FindProperty(Path);
        }

        protected virtual bool IsSuitableProperty()
        {
            return m_Property != null;
        }
    }
}