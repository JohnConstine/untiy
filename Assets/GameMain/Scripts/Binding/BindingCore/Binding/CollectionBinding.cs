using UnityEngine;

namespace SG1
{
    public abstract class CollectionBinding : SingleBinding
    {
        private Collection m_Collection;

        [InspectorReadOnly(InspectorDiplayMode.AlwaysEnabled)] [SerializeField]
        private bool m_CollectionFound;

        protected Collection Collection
        {
            get { return m_Collection; }
            set { m_Collection = value; }
        }

        public bool CollectionFound
        {
            get { return m_CollectionFound; }
            protected set { m_CollectionFound = value; }
        }

        protected override bool Bind()
        {
            m_Collection = FindCollection();

            if (m_Collection != null)
            {
                m_Collection.OnItemInsert += OnItemInsert;
                m_Collection.OnItemRemove += OnItemRemove;
                m_Collection.OnItemSelect += OnItemSelect;
                m_Collection.OnItemClear += OnItemClear;

                m_CollectionFound = true;
            }
            else
            {
                m_CollectionFound = false;
            }

            return m_CollectionFound;
        }


        protected override void Unbind()
        {
            if (m_Collection != null)
            {
                m_Collection.OnItemInsert -= OnItemInsert;
                m_Collection.OnItemRemove -= OnItemRemove;
                m_Collection.OnItemSelect -= OnItemSelect;
                m_Collection.OnItemClear -= OnItemClear;
                m_Collection = null;
                m_CollectionFound = false;
            }
        }

        protected abstract void OnItemClear();

        protected abstract void OnItemRemove(int index);

        protected abstract void OnItemInsert(int index, ItemContext itemContext);

        protected virtual void OnItemSelect(ItemContext itemContext)
        {
        }
        
        protected override void OnChange()
        {
        }

        protected virtual Collection FindCollection()
        {
            return ModelView == null ? null : ModelView.FindCollection(Path);
        }

        
    }
}