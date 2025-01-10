using System;
using UnityEngine;

namespace GJG.Items.ItemColor
{
    public class ItemColorSettings : MonoBehaviour
    {
        [Serializable]
        public class AAA
        {
            public ColorData colorData;
            public Renderer itemRenderer;
        }

        public AAA[] data;

        private void Start()
        {
            Material mat = data[0].itemRenderer.sharedMaterial;
            MaterialPropertyBlock _materialPropertyBlock = new MaterialPropertyBlock();
            data[0].itemRenderer.GetPropertyBlock(_materialPropertyBlock);

            for (int i = 0; i < data.Length; i++)
            {
                data[i].itemRenderer.sharedMaterial = mat;

                _materialPropertyBlock.SetFloat(ShaderPopertyIDData.GrayscaleIntensity, data[i].colorData.GrayscaleIntensity);
                _materialPropertyBlock.SetFloat(ShaderPopertyIDData.Brightness, data[i].colorData.Brightness);
                _materialPropertyBlock.SetFloat(ShaderPopertyIDData.Contrast, data[i].colorData.Contrast);
                _materialPropertyBlock.SetFloat(ShaderPopertyIDData.R, data[i].colorData.ColorLuminance.x);
                _materialPropertyBlock.SetFloat(ShaderPopertyIDData.G, data[i].colorData.ColorLuminance.y);
                _materialPropertyBlock.SetFloat(ShaderPopertyIDData.B, data[i].colorData.ColorLuminance.z);
                _materialPropertyBlock.SetColor(ShaderPopertyIDData.TintColor, data[i].colorData.TintColor);
                _materialPropertyBlock.SetInt(ShaderPopertyIDData.Row, UnityEngine.Random.Range(0, 4));
                _materialPropertyBlock.SetInt(ShaderPopertyIDData.Column, UnityEngine.Random.Range(0, 4));

                data[i].itemRenderer.SetPropertyBlock(_materialPropertyBlock);
            }
        }
    }
}
