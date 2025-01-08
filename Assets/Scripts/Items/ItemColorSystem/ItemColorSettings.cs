using System;
using Unity.Mathematics;
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

                _materialPropertyBlock.SetFloat("_GrayscaleIntensity", data[i].colorData.GrayscaleIntensity);
                _materialPropertyBlock.SetFloat("_Brightness", data[i].colorData.Brightness);
                _materialPropertyBlock.SetFloat("_Contrast", data[i].colorData.Contrast);
                _materialPropertyBlock.SetFloat("_R", data[i].colorData.ColorLuminance.x);
                _materialPropertyBlock.SetFloat("_G", data[i].colorData.ColorLuminance.y);
                _materialPropertyBlock.SetFloat("_B", data[i].colorData.ColorLuminance.z);
                _materialPropertyBlock.SetColor("_TintColor", data[i].colorData.TintColor);
                _materialPropertyBlock.SetInt("_Row", UnityEngine.Random.Range(0, 4));
                _materialPropertyBlock.SetInt("_Column", UnityEngine.Random.Range(0, 4));

                data[i].itemRenderer.SetPropertyBlock(_materialPropertyBlock);
            }
        }
    }
}
