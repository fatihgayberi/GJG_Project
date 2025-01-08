using UnityEngine;
using System;
using Unity.Mathematics;

namespace GJG.Items.ItemColor
{
    [CreateAssetMenu(fileName = "ColorData", menuName = "ColorData", order = 0)]
    public class ColorData : ScriptableObject
    {
        [SerializeField, ColorUsage(true, true)] private Color tintColor;
        [SerializeField, Range(0, 10)] private float grayscaleIntensity;
        [SerializeField, Range(-10, 10)] private float brightness;
        [SerializeField, Range(-2, 10)] private float contrast;
        [SerializeField] private float3 colorLuminance;

        public Color TintColor => tintColor;
        public float GrayscaleIntensity => grayscaleIntensity;
        public float Brightness => brightness;
        public float Contrast => contrast;
        public float3 ColorLuminance => colorLuminance;
    }
}