using GameFramework;
using GameFramework.Resource;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SG1
{
    [RequireComponent(typeof(Image))]
    [DisallowMultipleComponent]
    public class UGUIImageBinding : TextLoadAssetBinding
    {
        [SerializeField, InspectorReadOnly] private Image m_Image;

        public Image Image
        {
            get { return m_Image; }
            set { m_Image = value; }
        }

        protected override void ApplyNewValue(string newValue)
        {
            if (!string.IsNullOrEmpty(newValue))
            {
                GameEntry.Resource.LoadAsset(newValue, typeof(Sprite), LoadAssetCallbacks);
            }
        }
        
        protected override void OnLoadAssetSuccess(string assetname, object asset, float duration, object userdata)
        {
            Sprite sprite = asset as Sprite;
            if (sprite != null && m_Image != null)
            {
                m_Image.sprite = sprite;
            }
        }

        protected override void OnEditorValue()
        {
            base.OnEditorValue();
            if (m_Image == null)
            {
                m_Image = GetComponent<Image>();
            }
        }
    }
}