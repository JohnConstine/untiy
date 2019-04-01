using System;
using UnityEngine;

namespace SG1
{
    public abstract class UGuiFormPage : UGuiForm, IContext
    {
        [SerializeField] private ModelView m_ModelView;

        public ModelView ModelView
        {
            get { return m_ModelView; }
            set { m_ModelView = value; }
        }

        private IContext m_Context;

        public IContext Context
        {
            get { return m_Context; }
            set { m_Context = value; }
        }

        public void SetPropertyValue(string propertyName, object value)
        {
            if (m_Context != null)
            {
                m_Context.SetPropertyValue(propertyName, value);
            }
        }

        public Property FindProperty(string propertyName)
        {
            return m_Context == null ? null : m_Context.FindProperty(propertyName);
        }

        public void AddPropertyRuntime(string propertyName, Type type)
        {
            if (m_Context != null)
            {
                m_Context.AddPropertyRuntime(propertyName, type);
            }
        }
    }
}