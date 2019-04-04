using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SG1
{
    public class RootModelView : ModelView
    {
        [InspectorButton(InspectorDiplayMode.DisabledInPlayMode, "SetContextValue")]
        protected override void OnEditorValue()
        {
            base.OnEditorValue();
            UGuiFormPage uGuiFormPage = GetComponent<UGuiFormPage>();

            uGuiFormPage.ModelView = this;

            List<BaseBinding> bindings = new List<BaseBinding>();
            gameObject.GetComponentsInChildren(bindings);

            foreach (var binding in bindings)
            {
                binding.ModelView = binding.enabled ? this : null;
            }
        }
        
        public bool SetContext(IContext context)
        {
            if (context == null)
            {
                Log.Error("{0} : {1} is invalid", gameObject.name, "Context");
                return false;
            }

            Context = context;
            
            return true;
        }
    }
}