using System;
using System.Collections.Generic;
using GameFramework;
using JetBrains.Annotations;
using UnityGameFramework.Runtime;

namespace SG1
{
    public class Collection<T> : Collection where T : ItemContext
    {
        private readonly List<T> m_Items = new List<T>();

        private T m_SelectedItem;

        private T m_LastSelectedItem;

        public T SelectedItem
        {
            get { return m_SelectedItem; }
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

        public override ItemContext GetBaseItem(int index)
        {
            return GetItem(index);
        }

        public override IEnumerable<ItemContext> BaseItemContexts
        {
            get { return m_Items; }
        }

        public T GetItem(int index)
        {
            if (!(index >= 0 && index < m_Items.Count))
            {
                return (T) null;
            }

            return m_Items[index];
        }

        public override bool Remove(int index)
        {
            if (!(index >= 0 && index < m_Items.Count))
            {
                return false;
            }

            m_Items[index].OnItemClick -= Select;

            m_Items.RemoveAt(index);

            UpdateIndex();

            InvokeOnItemRemove(index);

            return true;
        }

        public bool Remove(T item)
        {
            if (item == null)
            {
                return false;
            }

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

        public void Insert(int index, [NotNull] T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (!(index >= 0 && index < m_Items.Count))
            {
                index = m_Items.Count;
            }

            item.OnItemClick += Select;
            item.Index = index;
            m_Items.Insert(index, item);
            UpdateIndex();
            InvokeOnItemInsert(index, item);
        }

        private void Select(ItemContext itemContext)
        {
            if (m_SelectedItem == m_LastSelectedItem)
            {
                return;
            }

            m_SelectedItem = itemContext as T;

            m_LastSelectedItem = m_SelectedItem;

            InvokeOnItemSelect(m_SelectedItem);
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
            m_Items.Clear();
            InvokeOnItemClear();
        }

        public override string ToString()
        {
            return Utility.Text.Format("Type:{0} Count:{1}", typeof(T), m_Items.Count.ToString());
        }
    }
}