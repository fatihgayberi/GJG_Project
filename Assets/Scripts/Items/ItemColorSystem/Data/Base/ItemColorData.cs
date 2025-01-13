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
    }
}