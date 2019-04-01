using UnityEngine;

namespace SG1
{
    public class MainPageModel : UGuiFormModel<MainPage, MainPageModel>
    {
        private Property<string> _privatestrProperty = new Property<string>();

        public string str
        {
            get { return _privatestrProperty.GetValue(); }
            set { _privatestrProperty.SetValue(value); }
        }

        public void Test()
        {
            Debug.Log("Test Succeed");
        }
    }

    public class MainPage : UGuiFormPage<MainPage, MainPageModel>
    {
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            UGuiFormModel.FindProperty("str").OnValueChange += (sender, property) => Debug.Log(property.ToString());

            UGuiFormModel.str = "Test";
        }

        [InspectorButton()]
        public void Test()
        {
            UGuiFormModel.Test();
        }
    }
}