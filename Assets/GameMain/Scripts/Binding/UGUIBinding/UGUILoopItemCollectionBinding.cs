using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

#pragma warning disable 0649
namespace SG1
{
    public class UGUILoopItemCollectionBinding : ItemCollectionBinding
    {
        private readonly Dictionary<int,ItemContext> m_ItemContexts = new Dictionary<int, ItemContext>();
        private readonly List<ItemModelView> m_ItemModelViews = new List<ItemModelView>();
        
        
        //固定item的高
        private float Height = 100;
        private int AllNum = 30; //总共有30个元素
        private float ContentHeight = 0;
        [SerializeField]
        private ScrollRect m_ScrollRect;
        [SerializeField]
        private GameObject m_ViewPort;
        /// <summary>
        /// 子元素上偏移量
        /// </summary>
        public float Offset_Top = 0;
        /// <summary>
        /// 子元素下偏移量
        /// </summary>
        public float Offset_Bottom = 0;
        private int CollectionLength = 0;
        protected override bool Bind()
        {
           
            base.Bind();
            if (CollectionFound)
            {
                CollectionLength = Collection.Count;
                //设置content的高度
                RectTransform _rectTransform = transform.GetComponent<RectTransform>();
                Vector2 cur = _rectTransform.sizeDelta;
                cur.y = AllNum*(Height+Offset_Top+Offset_Bottom);
                ContentHeight = cur.y;
                _rectTransform.sizeDelta = cur;
                for (int i = 0; i < Collection.Count; i++)
                {
                    OnItemInsert(i, Collection.GetBaseItem(i));             
                }
            }
            m_ScrollRect.onValueChanged.AddListener(ScrollValueChange); //为滑动绑定事件
            return CollectionFound;
        }

        //临时变量
        private int temp=0; //获取collection数组中的哪个数据移动到最后
        private int changeTemp = 0;
        private Vector2 StartScrollPos = Vector2.one ; //记录ScrollRect位置

        private void ScrollValueChange(Vector2 pos)
        {        
             
            //超过最后一个数据后不再增加数据  或者小于最小数据的时候也不进行操作
            if (CollectionLength > AllNum || CollectionLength<Collection.Count)
            {
                return;
            }
            //判断是向上滑 还是向下滑
            if (StartScrollPos.y > pos.y)
            {
                //向下滑
                StartScrollPos = pos;
                if (transform.localPosition.y > (Height + Offset_Top + Offset_Bottom)*(changeTemp+2))
                {
                    if (temp < Collection.Count)
                    {
                        //设置当前排头的元素移动到最后一位
                        SetToLast(m_ItemModelViews[temp]);
                        CollectionLength++;
                        //在最后添加一个元素
                        m_ItemModelViews[temp].SetContext(CollectionLength-1, Collection.GetBaseItem(temp));
                        temp++;
                    }
                    else
                    {
                        temp = 0;
                    }
                    changeTemp++;
                }
            }
            else
            {
                //向上滑
                StartScrollPos = pos;
                if (transform.localPosition.y < (Height + Offset_Top + Offset_Bottom) * (changeTemp ))
                {
                    Log.Debug("temp"+temp);
                    if (temp > 0)
                    {             
                        //将最后一个元素移动到第一位
                        SetToFirst(m_ItemModelViews[temp-1]);
                        CollectionLength--;
                        //在最后添加一个元素
                        m_ItemModelViews[temp-1].SetContext(CollectionLength-Collection.Count, Collection.GetBaseItem(temp-1));
                        temp--;
                    }
                    else
                    {
                        temp = Collection.Count;
                    }
                    changeTemp--;
                }
            }

        }
                

        private void SetToLast(ItemModelView item)
        {
            Vector3 curV = item.transform.GetComponent<RectTransform>().anchoredPosition;
            curV.y = (ContentHeight / 2 - Height / 2) - (CollectionLength) * Height - Offset_Top - Offset_Bottom;
            item.transform.GetComponent<RectTransform>().anchoredPosition= curV;
        }

        private void SetToFirst(ItemModelView item)
        {
            Vector3 curV = item.transform.GetComponent<RectTransform>().anchoredPosition;
            curV.y = (ContentHeight / 2 - Height / 2) - (CollectionLength-Collection.Count-1) * Height - Offset_Top - Offset_Bottom;
            item.transform.GetComponent<RectTransform>().anchoredPosition= curV;
        }

        protected override void OnItemClear()
        {
            
        }

        protected override void OnItemRemove(int index)
        {
            
        }

        
        //将元素写入
        protected override void OnItemInsert(int index, ItemContext itemContext)
        {
            m_ItemContexts[index] = itemContext;
            ItemModelView itemModelView = InstantiateItem(index);
            m_ItemModelViews.Insert(index, itemModelView);
            UpdateContext(index);
        }
        
        
        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private ItemModelView InstantiateItem(int index)
        {
            //在写入元素的时候 y值是计算出来的
            GameObject itemObject = Instantiate(Template, transform);
            Transform itemTransform = itemObject.transform;
            itemTransform.localScale = Vector3.one;
//            itemTransform.localPosition = Vector2.one ;
            float ChildPositionY = (ContentHeight / 2 - Height / 2) - index * Height - Offset_Top - Offset_Bottom;//子元素Y轴设置              
            itemTransform.GetComponent<RectTransform>().anchoredPosition=new Vector2(1,ChildPositionY);//设置每个子组件的位置
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