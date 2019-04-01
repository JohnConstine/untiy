namespace SG1
{
    public class UGuiFormPage<T1,T2> : UGuiFormPage where T1:UGuiFormPage<T1,T2> where T2 : UGuiFormModel<T1,T2>, new()
    {
        private T2 m_UGuiFormModel;

        public T2 UGuiFormModel
        {
            get { return m_UGuiFormModel; }
        }
                
        private void Awake()
        {
            m_UGuiFormModel = new T2();
            m_UGuiFormModel.SetUGuiFormPage((T1) this);
            Context = m_UGuiFormModel;
            ModelView.SetContext(Context);
        }
    }
}