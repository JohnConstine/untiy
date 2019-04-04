using System;

namespace SG1
{
    public abstract class ActionBinding : SingleBinding
    {
        protected Action m_Action;

        protected bool m_IsCommandFound;

        protected override void Unbind()
        {
            m_Action = null;
            
            m_IsCommandFound = false;
        }

        protected override bool Bind()
        {
            if (ModelView == null)
            {
                return false;
            }

            m_Action = ModelView.FindAction(Path);

            m_IsCommandFound = m_Action != null;

            return m_IsCommandFound;
        }

        protected override void OnChange()
        {
            
        }
    }
}