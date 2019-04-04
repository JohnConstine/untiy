using System;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

#pragma warning disable 0649
namespace SG1
{
    public class UGUILoopCollectionBinding : ItemCollectionBinding
    {
        [SerializeField] private Vector2 m_ItemSize = new Vector2(100f, 100f);

        [SerializeField] private ScrollRect m_ScrollRect;

        [SerializeField] private RectTransform m_Viewport;

        [SerializeField] private RectTransform.Axis m_Axis;

        [SerializeField] private RectOffset m_Padding;

        [SerializeField] private float m_Spacing;

        //[SerializeField] private TextAnchor m_ChildAlignment;

        [SerializeField] private int m_CacheCount;


        [NonSerialized] private RectTransform m_Rect;

        public RectTransform RectTransform
        {
            get
            {
                if (m_Rect == null)
                {
                    m_Rect = GetComponent<RectTransform>();
                }

                return m_Rect;
            }
        }

        protected override bool Bind()
        {
            if (base.Bind())
            {
                m_ScrollRect.onValueChanged.AddListener(UpdateContext);
                SetContent();
                return true;
            }

            return false;
        }

        protected override void Unbind()
        {
            m_ScrollRect.onValueChanged.RemoveListener(UpdateContext);
            base.Unbind();
        }

        private void UpdateContext(Vector2 normalizedPosition)
        {
            Log.Info(normalizedPosition);
        }

        protected override void OnItemClear()
        {
            // todo 子项删除
        }

        protected override void OnItemRemove(int index)
        {
            // todo 子项移除
        }

        protected override void OnItemInsert(int index, ItemContext itemContext)
        {
            // todo 子项插入
        }

        private ItemModelView InstantiateItem(int index)
        {
            GameObject itemObject = Instantiate(Template, transform);
            RectTransform itemTransform = itemObject.GetComponent<RectTransform>();
            itemTransform.localScale = Vector3.one;
            itemTransform.anchorMax = itemTransform.anchorMin = Vector2.one / 2f;
            itemTransform.sizeDelta = m_ItemSize;
            if (m_Axis == RectTransform.Axis.Horizontal)
            {
                Vector2 localPosition =
                    Vector2.right * (m_Padding.left + (index + 0.5f) * m_ItemSize[(int) m_Axis] + index * m_Spacing);
                itemTransform.localPosition = localPosition;
            }
            else if (m_Axis == RectTransform.Axis.Vertical)
            {
                Vector2 localPosition =
                    Vector2.down * (m_Padding.top + (index + 0.5f) * m_ItemSize[(int) m_Axis] + index * m_Spacing);
                itemTransform.localPosition = localPosition;
            }

            itemTransform.transform.SetSiblingIndex(index);
            ItemModelView modelView = itemObject.GetOrAddComponent<ItemModelView>();
            modelView.TemplateName = Template.name;
            return modelView;
        }

        private void SetContent()
        {
            float size = 0;

            if (m_Axis == RectTransform.Axis.Horizontal)
            {
                size += m_Padding.horizontal;
            }
            else if (m_Axis == RectTransform.Axis.Vertical)
            {
                size += m_Padding.vertical;
            }

            for (int i = 0; i < m_CacheCount; i++)
            {
                size += m_ItemSize[(int) m_Axis];
                size += m_Spacing;
            }

            // 减少单个间隔
            size -= m_Spacing;

            RectTransform.SetSizeWithCurrentAnchors(m_Axis, size);

            for (int i = 0; i < m_CacheCount; i++)
            {
                InstantiateItem(i);
            }
        }

        protected override void OnEditorValue()
        {
            base.OnEditorValue();

            if (m_ScrollRect == null)
            {
                m_ScrollRect = GetComponentInParent<ScrollRect>();
            }

            if (m_Padding == null)
            {
                m_Padding = new RectOffset();
                //m_ChildAlignment = TextAnchor.UpperCenter;
            }

            if (m_Viewport == null && m_ScrollRect != null)
            {
                m_Viewport = m_ScrollRect.viewport;
            }
        }
    }
}