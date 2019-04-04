using System;
using UnityEngine;

namespace SG1
{
    public abstract class BooleanBinding : PropertyBinding
    {
        [SerializeField] private bool m_DefaultValue;

        [SerializeField] private bool m_Invert;

        [SerializeField] private Check_Type m_CheckType = Check_Type.Boolean;

        [SerializeField, HideInInspector] private double m_Reference;
        [SerializeField, HideInInspector] private double m_ReferenceMax;
        [SerializeField, HideInInspector] private double m_ReferenceMin;
        [SerializeField, HideInInspector] private string m_StringReference;

        public bool Invert
        {
            get { return m_Invert; }
            set { m_Invert = value; }
        }

        public Check_Type CheckType
        {
            get { return m_CheckType; }
            set { m_CheckType = value; }
        }
        
//        public double Reference
//        {
//            get { return m_Reference; }
//            set { m_Reference = value; }
//        }
//        
//        public double ReferenceMax
//        {
//            get { return m_ReferenceMax; }
//            set { m_ReferenceMax = value; }
//        }
//        
//        public double ReferenceMin
//        {
//            get { return m_ReferenceMin; }
//            set { m_ReferenceMin = value; }
//        }
//        
//        public string StringReference
//        {
//            get { return m_StringReference; }
//            set { m_StringReference = value; }
//        }

        protected override void OnChange()
        {
            if (IgnoreChanges)
            {
                return;
            }

            bool newValue = m_DefaultValue;

            if (PropertyFound)
            {
                if (m_CheckType == Check_Type.Boolean)
                {
                    if (Property is Property<bool> property)
                    {
                        newValue = property.GetValue();
                    }
                }
                else if (m_CheckType == Check_Type.IsEmpty)
                {
                    if (Property is Property<string> property)
                    {
                        newValue = string.IsNullOrEmpty(property.GetValue());
                    }
                }
                else if (m_CheckType == Check_Type.EqualToString)
                {
                    if (Property is Property<string> property)
                    {
                        newValue = property.GetValue() == m_StringReference;
                    }
                }
                else
                {
                    var val = 0.0;
                    if (Property is Property<int>)
                    {
                        val = ((Property<int>) Property).GetValue();
                    }
                    else if (Property is Property<float>)
                    {
                        val = ((Property<float>) Property).GetValue();
                    }
                    else if (Property is Property<double>)
                    {
                        val = ((Property<double>) Property).GetValue();
                    }
//                    else if (Property is Property<long>)
//                    {
//                        val = ((Property<long>) Property).GetValue();
//                    }
//                    else if (Property is Property<short>)
//                    {
//                        val = ((Property<short>) Property).GetValue();
//                    }
//                    else if (Property is Property<byte>)
//                    {
//                        val = ((Property<byte>) Property).GetValue();
//                    }

                    if (m_CheckType == Check_Type.EqualToReference)
                    {
                        newValue = Math.Abs(val - m_Reference) < Mathf.Epsilon;
                    }
                    else if (m_CheckType == Check_Type.GreaterThanReference)
                    {
                        newValue = val > m_Reference;
                    }
                    else if (m_CheckType == Check_Type.LessThanReference)
                    {
                        newValue = val < m_Reference;
                    }
                    else if (m_CheckType == Check_Type.Between)
                    {
                        newValue = val < m_ReferenceMax && val > m_ReferenceMin;
                    }
                }
            }

            if (m_Invert)
            {
                newValue = !newValue;
            }

#if NOT_USE_UI_THREAD
            ApplyNewValue(newValue);
#else
            MainThreadDispatcher.DispatchToMainThread(() => { ApplyNewValue(newValue); });
#endif
        }

        protected abstract void ApplyNewValue(bool newValue);

        protected virtual void ApplyInputValue(bool inputValue)
        {
            if (!PropertyFound)
            {
                return;
            }

            if (m_CheckType != Check_Type.Boolean)
            {
                return;
            }

            if (m_Invert)
            {
                inputValue = !inputValue;
            }

            if (Property is Property<bool>)
            {
                IgnoreChanges = true;
                ((Property<bool>) Property).SetValue(inputValue);
                IgnoreChanges = false;
            }
        }
    }
}