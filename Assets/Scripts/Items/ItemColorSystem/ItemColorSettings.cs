using UnityEngine;

namespace GJG.Items.ItemColor
{
    public class ItemColorSettings : MonoBehaviour
    {
        [SerializeField] private ColorData colorData;
        [SerializeField] private Renderer itemRenderer;
        private MaterialPropertyBlock _materialPropertyBlock;

        private void Start()
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
            itemRenderer.GetPropertyBlock(_materialPropertyBlock);

            _materialPropertyBlock.SetFloat("_GrayscaleIntensity", colorData.GrayscaleIntensity);
            _materialPropertyBlock.SetFloat("_Brightness", colorData.Brightness);
            _materialPropertyBlock.SetFloat("_Contrast", colorData.Contrast);
            _materialPropertyBlock.SetFloat("_R", colorData.ColorLuminance.x);
            _materialPropertyBlock.SetFloat("_G", colorData.ColorLuminance.y);
            _materialPropertyBlock.SetFloat("_B", colorData.ColorLuminance.z);
            _materialPropertyBlock.SetColor("_TintColor", colorData.TintColor);

            itemRenderer.SetPropertyBlock(_materialPropertyBlock);
        }
    }
}
