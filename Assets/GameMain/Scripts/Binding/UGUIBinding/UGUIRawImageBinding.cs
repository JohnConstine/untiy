using GameFramework.Resource;
using UnityEngine;
using UnityEngine.UI;

namespace SG1
{
    [RequireComponent(typeof(RawImage))]
    [DisallowMultipleComponent]
    public class UGUIRawImageBinding : TextLoadAssetBinding
    {
        [SerializeField, InspectorReadOnly] private RawImage m_RawImage;

        public RawImage RawImage
        {
            get { return m_RawImage; }
            set { m_RawImage = value; }
        }

        protected override void ApplyNewValue(string newValue)
        {
            if (!string.IsNullOrEmpty(newValue))
            {
                GameEntry.Resource.LoadAsset(newValue, typeof(Texture2D), LoadAssetCallbacks);
            }
        }

        protected override void OnLoadAssetSuccess(string assetname, object asset, float duration, object userdata)
        {
            Texture2D texture2D = asset as Texture2D;
            if (texture2D != null && m_RawImage != null)
            {
                m_RawImage.texture = texture2D;
            }
        }

        protected override void OnEditorValue()
        {
            base.OnEditorValue();
            if (m_RawImage == null)
            {
                m_RawImage = GetComponent<RawImage>();
            }
        }
    }
}