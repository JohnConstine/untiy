namespace SG1
{
    public abstract class UGuiFormPage<T1, T2> : UGuiFormPage
        where T1 : UGuiFormPage<T1, T2>
        where T2 : UGuiFormModel<T1, T2>, new()
    {
        private T2 m_Model;

        public T2 Model
        {
            get { return m_Model; }
        }

        private void Awake()
        {
            m_Model = new T2();
            m_Model.SetUGuiFormPage((T1) this);
            Context = m_Model;
            ModelView.SetContext(Context);
        }
    }
}