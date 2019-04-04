using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SG1
{
    [DisallowMultipleComponent]
    public abstract class ModelView : ModelViewCore
    {
        private readonly Dictionary<string, Collection> m_CacheCollections = new Dictionary<string, Collection>();

        private readonly Dictionary<string, Action> m_CacheActions = new Dictionary<string, Action>();

        private readonly Dictionary<string, Property> m_CacheProperties = new Dictionary<string, Property>();

        private IContext m_Context;

        protected IContext Context
        {
            get { return m_Context; }
            set { m_Context = value; }
        }

        public Property FindProperty(string path)
        {
            if (m_Context == null)
            {
                return (Property) null;
            }

            if (!m_CacheProperties.ContainsKey(path))
            {
                m_CacheProperties[path] = Find(m_Context, path, NodeToProperty);
            }

            return m_CacheProperties[path];
        }

        public Action FindAction(string path)
        {
            if (m_Context == null)
            {
                return (Action) null;
            }

            if (!m_CacheActions.ContainsKey(path))
            {
                m_CacheActions[path] = Find(m_Context, path, NodeToAction);
            }

            return m_CacheActions[path];
        }

        public Collection FindCollection(string path)
        {
            if (m_Context == null)
            {
                return (Collection) null;
            }

            if (!m_CacheCollections.ContainsKey(path))
            {
                m_CacheCollections[path] = Find(m_Context, path, NodeToCollection);
            }

            return m_CacheCollections[path];
        }

        public void ClearAllCache()
        {
            m_CacheCollections.Clear();
            m_CacheProperties.Clear();
            m_CacheActions.Clear();
        }
    }
}