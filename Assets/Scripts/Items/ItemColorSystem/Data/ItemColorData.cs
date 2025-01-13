using UnityEngine;
using System;
using Unity.Mathematics;

namespace GJG.Items.ItemColor
{
    [CreateAssetMenu(fileName = "ColorData", menuName = "ColorData", order = 0)]
    public abstract class ItemColorData : ScriptableObject
    {
        [SerializeField] private ItemColorType itemColorType;
        [SerializeField] private Color tintColor;
        [SerializeField, Range(0, 10)] private float grayscaleIntensity;
        [SerializeField, Range(-10, 10)] private float brightness;
        [SerializeField, Range(-2, 10)] private float contrast;
        [SerializeField] private float3 colorLuminance;

        public ItemColorType ItemColorType => itemColorType;
        public Color TintColor => tintColor;
        public float GrayscaleIntensity => grayscaleIntensity;
        public float Brightness => brightness;
        public float Contrast => contrast;
        public float3 ColorLuminance => colorLuminance;

        public abstract int2 GetUV(int itemType);
        // {
        //     for (int i = 0; i < uVData.Length; i++)
        //     {
        //         if (uVData[i].ItemType != itemType) continue;
        //         return uVData[i].UVs[0];
        //     }

        //     return new int2(-1, -1);
        // }

        // [Serializable]
        // public class UVData
        // {
        //     [SerializeField] private ItemType itemType;
        //     [SerializeField] private int2[] uVs;

        //     public ItemType ItemType => itemType;
        //     public int2[] UVs => uVs;
        // }
    }
}