namespace SG1
{
    public class ActiveBinding : BooleanBinding
    {
        protected override void ApplyNewValue(bool newValue)
        {
            gameObject.SetActive(newValue);
        }
    }
}