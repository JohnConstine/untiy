namespace SG1
{
    public abstract class UGuiFormModel<T1, T2> : UGuiFormModel
        where T1 : UGuiFormPage<T1, T2>
        where T2 : UGuiFormModel<T1, T2>, new()
    {
        private T1 m_Page;

        public T1 Page
        {
            get { return m_Page; }
        }

        public void SetUGuiFormPage(T1 uGuiFormPage)
        {
            m_Page = uGuiFormPage;
        }
    }
}