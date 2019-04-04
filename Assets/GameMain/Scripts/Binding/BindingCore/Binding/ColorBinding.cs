using UnityEngine;

namespace SG1
{
    public abstract class ColorBinding : PropertyBinding
    {
        private static readonly Color DefaultColor = Color.white;

        protected override Property FindProperty()
        {
            return base.FindProperty() as Property<Color>;
        }

        protected override bool Bind()
        {
            bool isbind = base.Bind();
            if (isbind)
            {
                (Property as Property<Color>).SetValue(DefaultColor);
            }

            return isbind;
        }

        protected sealed override void OnChange()
        {
            if (IgnoreChanges) return;
            var newColor = Property == null ? DefaultColor : (Property as Property<Color>).GetValue();
#if NOT_USE_UI_THREAD
            ApplyNewValue(newColor);
#else
            MainThreadDispatcher.DispatchToMainThread(() => { ApplyNewValue(newColor); });
#endif
        }

        protected abstract void ApplyNewValue(Color newColor);
    }
}