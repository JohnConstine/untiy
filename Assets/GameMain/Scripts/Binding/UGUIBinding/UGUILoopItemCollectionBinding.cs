using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SG1
{
    public class UGUILoopItemCollectionBinding : ItemCollectionBinding
    {
        private readonly Dictionary<int,ItemContext> m_ItemContexts = new Dictionary<int, ItemContext>();
        private readonly List<ItemModelView> m_ItemModelViews = new List<ItemModelView>();

        private float Height = 171;
        private int AllNum = 30; 
        private float ContentHeight = 0;
        [SerializeField]
        private ScrollRect ScrollRect;
        
        public float Offset_Top = 0;
        
        public float Offset_Bottom = 0;
        private int CollectionLength = 0;

        private RectTransform _rectTransform;
//        public ScrollRect scrollRect;

        protected override bool Bind()
        {
           base.Bind();
           if (CollectionFound)
           {
               CollectionLength = Collection.Count;
               _rectTransform = transform.GetComponent<RectTransform>();
               Vector2 cur = _rectTransform.sizeDelta;
               cur.y = AllNum*(Height+Offset_Top+Offset_Bottom);
               ContentHeight = cur.y;
               _rectTransform.sizeDelta = cur;
               
               for (int i = 0; i < Collection.Count; i++)
               {
                   OnItemInsert(i,Collection.GetBaseItem(i));
               }
               
           }

           Log.Info("ContentHeight:"+ContentHeight);
           ScrollRect.onValueChanged.AddListener(ListLoop);
           return CollectionFound;
        }

        protected override void OnItemClear()
        {
            for (int i = 0; i < m_ItemModelViews.Count; i++)
            {
                Destroy(m_ItemModelViews[i].gameObject);
            }

            m_ItemModelViews.Clear();
        }

        protected override void OnItemRemove(int index)
        {
            GameObject destoryGameObject = m_ItemModelViews[index].gameObject;
            m_ItemModelViews.RemoveAt(index);
            Destroy(destoryGameObject);
            UpdateContext(index);
        }

        protected override void OnItemInsert(int index, ItemContext itemContext)
        {
            if (Template == null)
            {
                return;
            }
            Log.Info("Template:" + Template);
            
            ItemModelView itemModelView = InstantiateItem(index);
            m_ItemModelViews.Insert(index,itemModelView);

            UpdateContext(index);
        }

        private ItemModelView InstantiateItem(int index)
        {
            GameObject itemObject = Instantiate(Template, transform);
            Transform itemTransform = itemObject.transform;
            itemTransform.localScale = Vector3.one;
            
            float ChildPositionY = (ContentHeight / 2 - Height / 2) - index * Height - Offset_Top - Offset_Bottom;//子元素Y轴设置              
            itemTransform.GetComponent<RectTransform>().anchoredPosition=new Vector2(1,ChildPositionY);//设置每个子组件的位置
            
            itemTransform.transform.SetSiblingIndex(index);
            ItemModelView modelView = itemObject.GetOrAddComponent<ItemModelView>();
            modelView.TemplateName = Template.name;
            
            return modelView;
        }
        
        private void UpdateContext(int index)
        {
            for (int i = index; i < m_ItemContexts.Count; i++)
            {
              m_ItemModelViews[i].SetContext(i,Collection.GetBaseItem(i));
            }
              Log.Info(index);
        }

        private int temp = 0;
        private int changeTemp = 0;

        private void ListLoop(Vector2 vector2)
        {
            if (transform.localPosition.y > (Height + Offset_Top + Offset_Bottom)*(changeTemp+1))
            {
                Log.Info("开始添加");
                Log.Info("Temp："+temp);
                Log.Info("Collection.Count："+Collection.Count);
                if (temp < Collection.Count)
                {
                    //设置以第一个元素
                    
                    UpdateRectsize(m_ItemModelViews[temp]);
                    CollectionLength++;
                    //在最后添加一个元素
                    m_ItemModelViews[temp].SetContext(CollectionLength, Collection.GetBaseItem(temp));
                    Log.Info("content的位置："+_rectTransform.transform.localPosition);
                    temp++;
                }
                else
                {
                    temp = 0;
                }
                changeTemp++;
            }

            
        }

        private void UpdateRectsize(ItemModelView item)
        {
            Vector3 curV = item.transform.GetComponent<RectTransform>().anchoredPosition;
            Log.Info("Collection"+Collection.Count);
            Log.Info("CollectionLength"+CollectionLength);
            curV.y = curV.y - Height * Collection.Count;
          
            item.transform.GetComponent<RectTransform>().anchoredPosition= curV;
        }
    }
}