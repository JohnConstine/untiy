using System;
using System.Collections.Generic;
using System.Reflection;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SG1
{
    public abstract class Context : IContext
    {
        private readonly Dictionary<string, Property> m_Properties = new Dictionary<string, Property>();

        private readonly Dictionary<string, Collection> m_Collections = new Dictionary<string, Collection>();

        public void SetPropertyValue(string propertyName, object value)
        {
            if (m_Properties.ContainsKey(propertyName))
            {
                if (value is bool)
                {
                    SetPropertyValue(m_Properties[propertyName] as Property<bool>, (bool) value);
                }
                else if (value is Color)
                {
                    SetPropertyValue(m_Properties[propertyName] as Property<Color>, (Color) value);
                }
                else if (value is Color32)
                {
                    SetPropertyValue(m_Properties[propertyName] as Property<Color32>, (Color32) value);
                }
                else if (value is double)
                {
                    SetPropertyValue(m_Properties[propertyName] as Property<double>, (double) value);
                }
                else if (value is float)
                {
                    SetPropertyValue(m_Properties[propertyName] as Property<float>, (float) value);
                }
                else if (value is int)
                {
                    SetPropertyValue(m_Properties[propertyName] as Property<int>, (int) value);
                }
                else if (value is Quaternion)
                {
                    SetPropertyValue(m_Properties[propertyName] as Property<Quaternion>, (Quaternion) value);
                }
                else if (value is string)
                {
                    SetPropertyValue(m_Properties[propertyName] as Property<string>, (string) value);
                }
                else if (value is Vector2)
                {
                    SetPropertyValue(m_Properties[propertyName] as Property<Vector2>, (Vector2) value);
                }
                else if (value is Vector3)
                {
                    SetPropertyValue(m_Properties[propertyName] as Property<Vector3>, (Vector3) value);
                }
            }
            else
            {
                Log.Error("{0} not exist ", propertyName);
            }
        }

        private void SetPropertyValue<T>(Property<T> property, T variable)
        {
            if (property != null)
            {
                property.SetValue(variable);
            }
        }

        public Property FindProperty(string propertyName)
        {
            if (!m_Properties.ContainsKey(propertyName))
            {
                FieldInfo fieldInfo = GetType().GetField(Utility.Text.Format("_private{0}Property", propertyName),
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (fieldInfo != null)
                {
                    m_Properties.Add(propertyName, fieldInfo.GetValue(this) as Property);
                }
                else
                {
                    PropertyInfo propertyInfo = GetType().GetProperty(Utility.Text.Format("{0}Property", propertyName),
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (propertyInfo != null)
                    {
                        m_Properties.Add(propertyName, propertyInfo.GetValue(this, null) as Property);
                    }
                    else
                    {
                        m_Properties.Add(propertyName, null);
                    }
                }
            }

            return m_Properties[propertyName];
        }

        public Collection FindCollection(string collectionName)
        {
            if (!m_Collections.ContainsKey(collectionName))
            {
                var fieldInfo = GetType().GetField(Utility.Text.Format("_private{0}Collection", collectionName),
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (fieldInfo != null)
                {
                    m_Collections.Add(collectionName, fieldInfo.GetValue(this) as Collection);
                }
                else
                {
                    PropertyInfo propertyInfo = GetType().GetProperty(
                        Utility.Text.Format("{0}Collection", collectionName),
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (propertyInfo != null)
                    {
                        m_Collections.Add(collectionName, propertyInfo.GetValue(this, null) as Collection);
                    }
                    else
                    {
                        m_Collections.Add(collectionName, null);
                    }
                }
            }

            return m_Collections[collectionName];
        }

        public void AddPropertyRuntime(string propertyName, Type type)
        {
            if (m_Properties.ContainsKey(propertyName))
            {
                Log.Error(propertyName + " already exist");
                return;
            }

            if (type == typeof(bool))
            {
                m_Properties.Add(propertyName, new Property<bool>());
            }
            else if (type == typeof(Color))
            {
                m_Properties.Add(propertyName, new Property<Color>());
            }
            else if (type == typeof(Color32))
            {
                m_Properties.Add(propertyName, new Property<Color32>());
            }
            else if (type == typeof(double))
            {
                m_Properties.Add(propertyName, new Property<double>());
            }
            else if (type == typeof(float))
            {
                m_Properties.Add(propertyName, new Property<float>());
            }
            else if (type == typeof(int))
            {
                m_Properties.Add(propertyName, new Property<int>());
            }
            else if (type == typeof(Quaternion))
            {
                m_Properties.Add(propertyName, new Property<Quaternion>());
            }
            else if (type == typeof(string))
            {
                m_Properties.Add(propertyName, new Property<string>());
            }
            else if (type == typeof(Vector2))
            {
                m_Properties.Add(propertyName, new Property<Vector2>());
            }
            else if (type == typeof(Vector3))
            {
                m_Properties.Add(propertyName, new Property<Vector3>());
            }
            else
            {
                Log.Error(type.Name + " not support");
            }
        }
    }
}