using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SG1
{
    public class RootModelView : ModelView
    {
        [InspectorButton(InspectorDiplayMode.DisabledInPlayMode,"SetContextValue")]
        public void SetValueInEditor()
        {
            var bindings = new List<BaseBinding>();
            gameObject.GetComponentsInChildren(bindings);

            foreach (var binding in bindings)
            {
                binding.ModelView = binding.enabled ? this : null;
            }
        }

        private void OnValidate()
        {
            SetValueInEditor();
        }

        private void Reset()
        {
            OnValidate();
        }
    }
}