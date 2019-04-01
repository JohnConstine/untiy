using System;
using System.Collections.Generic;
using GameFramework;
using UnityGameFramework.Runtime;

namespace SG1
{
    public class Collection<T> : Collection where T : ItemContext
    {
        private readonly List<T> m_Items = new List<T>();

        private EventHandler<int> m_OnItemRemove;

        private EventHandler<T> m_OnItemSelect;

        private EventHandler<T> m_OnItemInsert;

        private EventHandler<Collection<T>> m_OnItemClear;

        private T m_SelectedItem;

        private T m_LastSelectedItem;

        public T SelectedItem
        {
            get { return m_SelectedItem; }
        }

        public event EventHandler<int> OnItemRemove
        {
            add { m_OnItemRemove += value; }
            remove { m_OnItemRemove -= value; }
        }


        public event EventHandler<T> OnItemSelect
        {
            add { m_OnItemSelect += value; }
            remove { m_OnItemSelect -= value; }
        }

        public event EventHandler<T> OnItemInsert
        {
            add { m_OnItemInsert += value; }
            remove { m_OnItemInsert -= value; }
        }

        public event EventHandler<Collection<T>> OnItemClear
        {
            add { m_OnItemClear += value; }
            remove { m_OnItemClear -= value; }
        }


        public IEnumerable<T> Items
        {
            get { return m_Items; }
        }

        public override bool HasItems
        {
            get { return m_Items.Count > 0; }
        }

        public override int Count
        {
            get { return m_Items.Count; }
        }

        public T GetItem(int index)
        {
            if (!IsEffectiveIndex(index))
            {
                return (T) null;
            }

            return m_Items[index];
        }

        public bool Remove(int index)
        {
            if (!IsEffectiveIndex(index))
            {
                return false;
            }

            m_Items[index].OnItemClick -= Select;

            m_OnItemRemove.Invoke(this, index);

            m_Items.RemoveAt(index);

            UpdateIndex();

            return true;
        }

        public bool Remove(T item)
        {
            for (int i = 0; i < m_Items.Count; i++)
            {
                if (m_Items[i] != item)
                {
                    continue;
                }

                Remove(i);
                return true;
            }

            return false;
        }

        public void Add(T item)
        {
            Insert(m_Items.Count, item);
        }

        public void Insert(int index, T item)
        {
            if (!IsEffectiveIndex(index))
            {
                index = m_Items.Count;
            }

            item.OnItemClick += Select;
            item.Index = index;
            m_Items.Insert(index, item);
            m_OnItemInsert.Invoke(this, item);
        }

        public bool IsEffectiveIndex(int index)
        {
            return index > 0 || index <= m_Items.Count;
        }

        private void Select(object sender, ItemContext e)
        {
            if (m_SelectedItem == m_LastSelectedItem)
            {
                return;
            }

            m_SelectedItem = (T) e;

            m_LastSelectedItem = m_SelectedItem;

            m_OnItemSelect.Invoke(this, m_SelectedItem);
        }

        private void UpdateIndex()
        {
            for (int i = 0; i < m_Items.Count; i++)
            {
                m_Items[i].Index = i;
            }
        }

        public void Clear()
        {
            m_OnItemClear.Invoke(this, this);
            m_Items.Clear();
        }

        public override string ToString()
        {
            return Utility.Text.Format("Type:{0} Count:{1}", typeof(T), m_Items.Count.ToString());
        }
    }
}