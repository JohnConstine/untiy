namespace SG1
{
    public class UGuiFormModel<T1, T2> : UGuiFormModel where T1 : UGuiFormPage<T1, T2> where T2 : UGuiFormModel<T1, T2>, new()
    {
        private T1 m_UGuiFormPage;

        public T1 UGuiFormPage
        {
            get { return m_UGuiFormPage; }
        }

        public void SetUGuiFormPage(T1 uGuiFormPage)
        {
            m_UGuiFormPage = uGuiFormPage;
        }
    }
}