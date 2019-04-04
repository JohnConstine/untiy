using UnityEngine.EventSystems;

namespace SG1
{
    public abstract class UICustomBehaviour : UIBehaviour
    {
        protected override void OnValidate()
        {
            OnEditorValue();
        }

        protected override void Reset()
        {
            OnValidate();
        }

        protected virtual void OnEditorValue()
        {
        }
    }
}