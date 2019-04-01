using System;
using UnityEngine;

namespace SG1
{
    public abstract class BaseBinding : MonoBehaviour, IBaseBinding
    {
        [SerializeField] private ModelView m_ModelView;

        public ModelView ModelView
        {
            get { return m_ModelView; }
            set { m_ModelView = value; }
        }

        private bool m_Binded;

        protected bool m_IgnoreChanges;

        protected abstract bool Bind();

        protected abstract void Unbind();

        public void OnContextChange()
        {
            UpdateBinding();
        }

        public virtual void OnChange()
        {
        }

        internal void UpdateBinding()
        {
            Unbind();
            m_Binded = Bind();
            if (m_Binded)
            {
                OnChange();
            }
        }

        protected virtual void Start()
        {
            if (!m_Binded)
            {
                UpdateBinding();
            }
            else
            {
                OnChange();
            }
        }

        protected virtual void OnDestroy()
        {
            Unbind();
        }

        protected ModelView GetModelView()
        {
            return m_ModelView != null ? m_ModelView : m_ModelView = GetComponentInParent<ModelView>();
        }

        public string GetTextValue(Property property)
        {
            if (property == null)
            {
                return string.Empty;
            }
            else if (property is Property<string>)
            {
                return ((Property<string>) property).GetValue();
            }
            else if (property is Property<int>)
            {
                return ((Property<int>) property).GetValue().ToString();
            }
            else if (property is Property<float>)
            {
                return ((Property<float>) property).GetValue().ToString("R");
            }
            else if (property is Property<double>)
            {
                return ((Property<double>) property).GetValue().ToString("R");
            }
//            else if (property is Property<long>)
//            {
//                return ((Property<long>) property).GetValue();
//            }
//            else if (property is Property<short>)
//            {
//                return ((Property<short>) property).GetValue();
//            }
//            else if (property is Property<byte>)
//            {
//                return ((Property<byte>) property).GetValue();
//            }
            else
            {
                return string.Empty;
            }
        }

        public void SetTextValue(Property property, string value)
        {
            if (property is Property<string>)
            {
                ((Property<string>) property).SetValue(value);
            }
            else if (property is Property<int>)
            {
                if (int.TryParse(value, out var v))
                {
                    ((Property<int>) property).SetValue(v);
                }
            }
            else if (property is Property<float>)
            {
                if (float.TryParse(value, out var v))
                {
                    ((Property<float>) property).SetValue(v);
                }
            }
            else if (property is Property<double>)
            {
                if (double.TryParse(value, out var v))
                {
                    ((Property<double>) property).SetValue(v);
                }
            }
            else if (property is Property<long>)
            {
                if (long.TryParse(value, out var v))
                {
                    ((Property<long>) property).SetValue(v);
                }
            }
            else if (property is Property<short>)
            {
                if (short.TryParse(value, out var v))
                {
                    ((Property<short>) property).SetValue(v);
                }
            }
            else if (property is Property<byte>)
            {
                if (byte.TryParse(value, out var v))
                {
                    ((Property<byte>) property).SetValue(v);
                }
            }
        }

        public double GetNumericValue(Property property)
        {
            if (property == null)
            {
                return 0;
            }
            else if (property is Property<int>)
            {
                return ((Property<int>) property).GetValue();
            }
            else if (property is Property<float>)
            {
                return ((Property<float>) property).GetValue();
            }
            else if (property is Property<double>)
            {
                return ((Property<double>) property).GetValue();
            }
//            else if (property is Property<long>)
//            {
//                return ((Property<long>) property).GetValue();
//            }
//            else if (property is Property<short>)
//            {
//                return ((Property<short>) property).GetValue();
//            }
//            else if (property is Property<byte>)
//            {
//                return ((Property<byte>) property).GetValue();
//            }
            return 0;
        }

        public void SetNumericValue(Property property, double val)
        {
            if (property is Property<int>)
            {
                ((Property<int>) property).SetValue((int) val);
            }
            else if (property is Property<float>)
            {
                ((Property<float>) property).SetValue((float) val);
            }
            else if (property is Property<double>)
            {
                ((Property<double>) property).SetValue(val);
            }
//            else if (property is Property<long> _)
//            {
//                ((Property<long>) property).SetValue((long) val);
//            }
//            else if (property is Property<short> _)
//            {
//                ((Property<short>) property).SetValue((short) val);
//            }
//            else if (property is Property<byte> _)
//            {
//                ((Property<byte>) property).SetValue((byte) val);
//            }
        }
    }
}