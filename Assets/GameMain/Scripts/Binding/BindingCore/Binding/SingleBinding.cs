using UnityEngine;
using UnityEngine.Serialization;

namespace SG1
{
    public abstract class SingleBinding : BaseBinding
    {
        [InspectorReadOnly(InspectorDiplayMode.EnabledInPlayMode)]
        [SerializeField] private string m_Path = string.Empty;

        public string Path
        {
            get { return m_Path; }
        }
    }
}