using UnityEngine;
using UnityGameFramework.Runtime;

namespace SG1
{
    public class Item : ItemContext
    {
        #region nameProperty

        private readonly Property<string> _privatenameProperty = new Property<string>();

        public Property<string> nameProperty
        {
            get { return _privatenameProperty; }
        }

        public string name
        {
            get { return _privatenameProperty.GetValue(); }
            set { _privatenameProperty.SetValue(value); }
        }

        #endregion

        
    }

    public class GoodsItem : ItemContext
    {
        #region GoodsProperty

        private readonly Property<string> _privateGoodsProperty = new Property<string>();

        public Property<string> GoodsProperty
        {
            get { return _privateGoodsProperty; }
        }

        public string Goods
        {
            get { return _privateGoodsProperty.GetValue(); }
            set { _privateGoodsProperty.SetValue(value); }
        }

        #endregion

        
    }

    public class MainPageModel : UGuiFormModel<MainPage, MainPageModel>
    {
        #region ItemCollect

        private readonly Collection<Item> _privateItemCollection = new Collection<Item>();

        public Collection<Item> Item
        {
            get { return _privateItemCollection; }
        }

        #endregion

        #region strProperty

        private readonly Property<string> _privatestrProperty = new Property<string>();

        public Property<string> strProperty
        {
            get { return _privatestrProperty; }
        }

        public string str
        {
            get { return _privatestrProperty.GetValue(); }
            set { _privatestrProperty.SetValue(value); }
        }

        #endregion

        #region ActiveProperty

        private readonly Property<bool> _privateActiveProperty = new Property<bool>();

        public Property<bool> ActiveProperty
        {
            get { return _privateActiveProperty; }
        }

        public bool Active
        {
            get { return _privateActiveProperty.GetValue(); }
            set { _privateActiveProperty.SetValue(value); }
        }

        #endregion

        #region ColorProperty

        private readonly Property<Color> _privateColorProperty = new Property<Color>();

        public Property<Color> ColorProperty
        {
            get { return _privateColorProperty; }
        }

        public Color Color
        {
            get { return _privateColorProperty.GetValue(); }
            set { _privateColorProperty.SetValue(value); }
        }

        #endregion

        #region SpriteProperty

        private readonly Property<string> _privateSpriteProperty = new Property<string>();

        public Property<string> SpriteProperty
        {
            get { return _privateSpriteProperty; }
        }

        public string Sprite
        {
            get { return _privateSpriteProperty.GetValue(); }
            set { _privateSpriteProperty.SetValue(value); }
        }

        #endregion

        #region RawImageProperty

        private readonly Property<string> _privateRawImageProperty = new Property<string>();

        public Property<string> RawImageProperty
        {
            get { return _privateRawImageProperty; }
        }

        public string RawImage
        {
            get { return _privateRawImageProperty.GetValue(); }
            set { _privateRawImageProperty.SetValue(value); }
        }

        #endregion

        #region TestIntProperty

        private readonly Property<int> _privateTestIntProperty = new Property<int>();

        public Property<int> TestIntProperty
        {
            get { return _privateTestIntProperty; }
        }

        public int TestInt
        {
            get { return _privateTestIntProperty.GetValue(); }
            set { _privateTestIntProperty.SetValue(value); }
        }

        #endregion

        #region GoodsItemCollect

        private readonly Collection<GoodsItem> _privateGoodsItemCollection = new Collection<GoodsItem>();

        public Collection<GoodsItem> GoodsItem
        {
            get { return _privateGoodsItemCollection; }
        }

        #endregion
        
        public void Test()
        {
            Debug.Log("Test Succeed");
            Active = !Active;
            Color = new Color(Random.value, Random.value, Random.value, Random.value);
            TestInt++;
        }
    }

    public class MainPage : UGuiFormPage<MainPage, MainPageModel>
    {
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            //UGuiFormModel.FindProperty("str").OnValueChange += (sender, property) => Debug.Log(property.ToString());

            Model.str = "Test";

            Model.Active = false;

            for (int i = 0; i < 10; i++)
            {
//                Model.Item.Add(new Item() {name = i.ToString()});
                  GoodsItem goodsItem = new GoodsItem()
                  {
                      Goods = i.ToString()
                  };
                  
                  Model.GoodsItem.Add(goodsItem);
                  
            }

            Model.ActiveProperty.OnValueChange += OnActiveChange;
        }

        protected override void OnClose(object userData)
        {
            Model.ActiveProperty.OnValueChange -= OnActiveChange;
            
            base.OnClose(userData);
        }

        private void OnActiveChange()
        {
            Log.Info(Model.Active);
        }

        [InspectorButton()]
        public void ReName(int index, string name)
        {
            Model.Item.GetItem(index).name = name;
        }

        [InspectorButton()]
        public void AddItem(int index, string name)
        {
            Model.Item.Insert(index, new Item() {name = name});
        }

        [InspectorButton()]
        public void Delete(int index)
        {
            Model.Item.Remove(index);
        }

        [InspectorButton("Test")]
        public void Test()
        {
            Model.Test();
        }

        [InspectorButton()]
        public void SetImage(string Path)
        {
            Model.Sprite = Path;
        }

        [InspectorButton()]
        public void SetRawImage(string path)
        {
            Model.RawImage = path;
        }
    }
}