using UnityEngine;

namespace GJG.Items.ItemColor
{
    public static class ShaderPopertyIDData
    {
        public static readonly int GrayscaleIntensity = Shader.PropertyToID("_GrayscaleIntensity");
        public static readonly int Brightness = Shader.PropertyToID("_Brightness");
        public static readonly int Contrast = Shader.PropertyToID("_Contrast");
        public static readonly int R = Shader.PropertyToID("_R");
        public static readonly int G = Shader.PropertyToID("_G");
        public static readonly int B = Shader.PropertyToID("_B");
        public static readonly int TintColor = Shader.PropertyToID("_TintColor");
        public static readonly int Row = Shader.PropertyToID("_Row");
        public static readonly int Column = Shader.PropertyToID("_Column");
    }
}