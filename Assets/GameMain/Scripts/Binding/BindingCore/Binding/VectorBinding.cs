using System.Configuration;
using UnityEngine;

namespace SG1
{
    public abstract class VectorBinding : PropertyBinding
    {
        protected override Property FindProperty()
        {
            Property findProperty = base.FindProperty();
            if (findProperty is Property<Vector2> || findProperty is Property<Vector3>)
            {
                return findProperty;
            }

            return null;
        }

        protected override void OnChange()
        {
            if (IgnoreChanges)
            {
                return;
            }

            Vector3 newValue = Vector3.zero;
            if (PropertyFound)
            {
                if (Property is Property<Vector2>)
                {
                    newValue = (Property as Property<Vector2>).GetValue();
                }
                else if (Property is Property<Vector3>)
                {
                    newValue = (Property as Property<Vector3>).GetValue();
                }
            }

#if NOT_USE_UI_THREAD
            ApplyNewValue(newValue);
#else
            MainThreadDispatcher.DispatchToMainThread(() => { ApplyNewValue(newValue); });
#endif
        }

        protected virtual void ApplyInputValue(Vector3 inputValue)
        {
            if (!PropertyFound)
            {
                return;
            }

            IgnoreChanges = true;
            if (Property is Property<Vector2>)
            {
                (Property as Property<Vector2>).SetValue(inputValue);
            }
            else if (Property is Property<Vector3>)
            {
                (Property as Property<Vector3>).SetValue(inputValue);
            }
            IgnoreChanges = false;
        }

        protected abstract void ApplyNewValue(Vector3 newValue);
    }
}