using UnityEngine;

#pragma warning disable 0649
namespace SG1
{
    public abstract class ItemCollectionBinding : CollectionBinding
    {
        [InspectorReadOnly(InspectorDiplayMode.EnabledInPlayMode)] [SerializeField]
        private GameObject m_Template;

        public GameObject Template
        {
            get { return m_Template; }
        }
    }
}