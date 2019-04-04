using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SG1
{
    public class UGUIItemCollectionBinding : ItemCollectionBinding
    {
        private readonly List<ItemModelView> m_ItemModelViews = new List<ItemModelView>();

        protected List<ItemModelView> ItemModelView
        {
            get { return m_ItemModelViews; }
        }

        protected override bool Bind()
        {
            base.Bind();

            if (CollectionFound)
            {
                for (int i = 0; i < Collection.Count; i++)
                {
                    OnItemInsert(i, Collection.GetBaseItem(i));
                }
            }

            return CollectionFound;
        }

        protected override void OnItemInsert(int index, ItemContext itemContext)
        {
            if (Template == null)
            {
                Log.Error("Template is invalid");
                return;
            }

            ItemModelView itemModelView = InstantiateItem(index);
            m_ItemModelViews.Insert(index, itemModelView);

            UpdateContext(index);
        }

        protected override void OnItemRemove(int index)
        {
            GameObject destoryGameObject = m_ItemModelViews[index].gameObject;
            m_ItemModelViews.RemoveAt(index);
            Destroy(destoryGameObject);
            UpdateContext(index);
        }

        protected override void OnItemClear()
        {
            for (int i = 0; i < m_ItemModelViews.Count; i++)
            {
                Destroy(m_ItemModelViews[i].gameObject);
            }

            m_ItemModelViews.Clear();
        }

        private ItemModelView InstantiateItem(int index)
        {
            GameObject itemObject = Instantiate(Template, transform);
            Transform itemTransform = itemObject.transform;
            itemTransform.localScale = Vector3.one;
            itemTransform.localPosition = Vector2.one * 5000;
            itemTransform.transform.SetSiblingIndex(index);
            ItemModelView modelView = itemObject.GetOrAddComponent<ItemModelView>();
            modelView.TemplateName = Template.name;
            return modelView;
        }

        private void UpdateContext(int index)
        {
            for (int i = index; i < m_ItemModelViews.Count; i++)
            {
                m_ItemModelViews[i].SetContext(i, Collection.GetBaseItem(i));
            }
        }
    }
}