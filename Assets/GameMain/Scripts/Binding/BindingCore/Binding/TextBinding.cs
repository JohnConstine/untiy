using System;
using UnityEngine;

namespace SG1
{
    public abstract class TextBinding : PropertyBinding
    {
        [InspectorReadOnly(InspectorDiplayMode.EnabledInPlayMode)] [SerializeField]
        private string m_Format = @"{0}";

        [InspectorReadOnly(InspectorDiplayMode.EnabledInPlayMode)] [SerializeField]
        private Modifier m_Modifier = SG1.Modifier.None;

        private Func<string, string> m_CustomModifier = null;

        public Modifier Modifier
        {
            get { return m_Modifier; }
            set { m_Modifier = value; }
        }

        public string Format
        {
            get { return m_Format; }
            set { m_Format = value; }
        }

        public Func<string, string> CustomModifier
        {
            get { return m_CustomModifier; }
            set { m_CustomModifier = value; }
        }

        protected void ApplyInputValue(string value)
        {
            if (!PropertyFound)
            {
                return;
            }

            IgnoreChanges = true;
            SetTextValue(Property, value);
            IgnoreChanges = false;
        }

        protected override void OnChange()
        {
            if (IgnoreChanges)
            {
                return;
            }

            string newValue = string.Format(m_Format, GetRowValue(Property));

            switch (m_Modifier)
            {
                case Modifier.ToUppercase:
                    newValue = newValue.ToUpper();
                    break;
                case Modifier.ToLowercase:
                    newValue = newValue.ToLower();
                    break;
                case Modifier.Custom:
                    if (m_CustomModifier != null)
                    {
                        newValue = m_CustomModifier(newValue);
                    }

                    break;
            }

#if NOT_USE_UI_THREAD
            ApplyNewValue(newValue);
#else
            MainThreadDispatcher.DispatchToMainThread(() => { ApplyNewValue(newValue); });
#endif
        }

        protected abstract void ApplyNewValue(string newValue);
    }
}