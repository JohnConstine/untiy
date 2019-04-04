using System;
using System.Collections.Generic;
using UnityEngine.Experimental.XR;

namespace SG1
{
    public abstract class Collection
    {
        public abstract bool HasItems { get; }

        public abstract int Count { get; }

        public abstract ItemContext GetBaseItem(int index);

        public abstract IEnumerable<ItemContext> BaseItemContexts { get; }

        public abstract bool Remove(int index);

        private event Action<int> m_OnItemRemove;

        private event Action<ItemContext> m_OnItemSelect;

        private event Action<int, ItemContext> m_OnItemInsert;

        private event Action m_OnItemClear;

        public event Action<int> OnItemRemove
        {
            add { m_OnItemRemove += value; }
            remove { m_OnItemRemove -= value; }
        }

        public event Action<ItemContext> OnItemSelect
        {
            add { m_OnItemSelect += value; }
            remove { m_OnItemSelect -= value; }
        }

        public event Action<int, ItemContext> OnItemInsert
        {
            add { m_OnItemInsert += value; }
            remove { m_OnItemInsert -= value; }
        }

        public event Action OnItemClear
        {
            add { m_OnItemClear += value; }
            remove { m_OnItemClear -= value; }
        }

        protected void InvokeOnItemClear()
        {
            if (m_OnItemClear != null)
            {
                m_OnItemClear.Invoke();
            }
        }

        protected void InvokeOnItemSelect(ItemContext itemContext)
        {
            if (m_OnItemSelect != null)
            {
                m_OnItemSelect.Invoke(itemContext);
            }
        }

        protected void InvokeOnItemInsert(int index, ItemContext itemContext)
        {
            if (m_OnItemInsert != null)
            {
                m_OnItemInsert.Invoke(index, itemContext);
            }
        }

        protected void InvokeOnItemRemove(int index)
        {
            if (m_OnItemRemove != null)
            {
                m_OnItemRemove.Invoke(index);
            }
        }
    }
}