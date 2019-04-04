namespace SG1
{
    public abstract class NumericBinding : PropertyBinding
    {
        protected override bool IsSuitableProperty()
        {
            return base.IsSuitableProperty() && (Property is Property<int>
                                                 || Property is Property<float>
                                                 || Property is Property<double>);
        }

        protected void SetValue(double value)
        {
            IgnoreChanges = true;
            SetNumericValue(Property, value);
            IgnoreChanges = false;
        }

        protected sealed override void OnChange()
        {
            if (IgnoreChanges)
            {
                return;
            }

            var value = GetNumericValue(Property);


#if NOT_USE_UI_THREAD
            ApplyNewValue(value);
#else
            MainThreadDispatcher.DispatchToMainThread(() => { ApplyNewValue(value); });
#endif
        }

        protected abstract void ApplyNewValue(double value);
    }
}