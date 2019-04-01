using System;
using UnityEngine;

namespace SG1
{
    public sealed class InspectorReadOnlyAttribute : PropertyAttribute
    {
        private InspectorDiplayMode m_Mode = InspectorDiplayMode.AlwaysEnabled;

        public InspectorDiplayMode Mode
        {
            get { return m_Mode; }
        }

        public InspectorReadOnlyAttribute() : this(InspectorDiplayMode.AlwaysEnabled)
        {
        }

        public InspectorReadOnlyAttribute(InspectorDiplayMode mode)
        {
            m_Mode = mode;
        }
    }
}