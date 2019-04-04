using System;
using GameFramework;
using UnityEngine;

namespace SG1
{
    public class Property<T> : Property
    {
        private readonly bool m_IsValue = typeof(T).IsValueType;

        private bool m_Changing = false;

        protected T m_Value;

        public Property()
        {
        }

        public Property(T value) : this()
        {
            m_Value = value;
        }

        public bool IsOfType(Type type)
        {
            return type.IsAssignableFrom(typeof(T));
        }

        public T GetValue()
        {
            return m_Value;
        }

        public virtual void SetValue(T value)
        {
            if (m_Changing)
            {
                return;
            }

            m_Changing = true;

            bool changed = false;

            if (m_IsValue)
            {
                changed = IsValueDifferent(value);
            }
            else
            {
                changed = value == null && m_Value != null || value != null && m_Value == null ||
                          m_Value != null && IsClassDifferent(value);
            }

            if (changed)
            {
                m_Value = value;
                OnValueChanged();
            }

            m_Changing = false;
        }

        protected virtual bool IsValueDifferent(T value)
        {
            return !m_Value.Equals(value);
        }

        private bool IsClassDifferent(T value)
        {
            return !m_Value.Equals(value);
        }

        public override string ToString()
        {
            return Utility.Text.Format("Type:{0} Value:{1}", typeof(T), m_Value.ToString());
        }

        public override object GetRowValue()
        {
            return m_Value;
        }
    }
}