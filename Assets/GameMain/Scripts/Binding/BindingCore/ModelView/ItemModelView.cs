using System.Collections.Generic;
using UnityEngine;

namespace SG1
{
    public class ItemModelView : ModelView
    {
        [SerializeField] [InspectorReadOnly(InspectorDiplayMode.DisabledInPlayMode)]
        private List<BaseBinding> m_CacheBindings = new List<BaseBinding>();

        [SerializeField] [InspectorReadOnly] private int m_Index;

        [SerializeField] [InspectorReadOnly] private string m_TemplateName;

        public List<BaseBinding> CacheBindings
        {
            get { return m_CacheBindings; }
        }

        public int Index
        {
            get { return m_Index; }
        }

        public string TemplateName
        {
            get { return m_TemplateName; }
            set { m_TemplateName = value; }
        }

        public void SetContext(int index, ItemContext itemContext)
        {
            Context = itemContext;
            foreach (var binding in m_CacheBindings)
            {
                binding.ModelView = this;
                binding.OnContextChange();
            }
            gameObject.name = string.Format("{0}_{1}", m_TemplateName, index.ToString());
            m_Index = index;
        }

        protected override void OnEditorValue()
        {
            base.OnEditorValue();
            GetComponentsInChildren<BaseBinding>(true, m_CacheBindings);
        }
    }
}