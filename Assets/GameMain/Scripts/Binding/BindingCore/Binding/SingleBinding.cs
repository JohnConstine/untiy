using UnityEngine;

namespace SG1
{
    public abstract class SingleBinding : BaseBinding
    {
        [SerializeField] private string m_Paht = string.Empty;

        public string Path
        {
            get { return m_Paht; }
            set { m_Paht = value; }
        }
    }
}