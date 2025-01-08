Shader "GJG/Sprite/Item"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}  // Ana dokumuz
        [PerRendererData] _GrayscaleIntensity ("Grayscale Intensity", Range(0, 1)) = 1  // Gri tonlama yoğunluğu
        [PerRendererData] _Brightness ("Brightness", Range(-10, 10)) = 0  // Parlaklık ayarı
        [PerRendererData] _Contrast ("Contrast", Range(0, 20)) = 1  // Kontrast ayarı
        [PerRendererData] _R ("_R", Range(-2, 10)) = 1  // Kontrast ayarı
        [PerRendererData] _G ("_G", Range(-2, 10)) = 1  // Kontrast ayarı
        [PerRendererData] _B ("_B", Range(-2, 10)) = 1  // Kontrast ayarı
        [PerRendererData] [HDR]_TintColor ("Tint Color", Color) = (1, 1, 1, 1)  // Renk çarpanı (Basılacak renk)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _GrayscaleIntensity;
            float _Brightness;
            float _Contrast;
            float _R;
            float _G;
            float _B;
            fixed4 _TintColor;  // Renk çarpanı (Tint)

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Base texture rengini al
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // RGB'yi gri tona çevirmek için luminance hesaplama
                float gray = dot(texColor.rgb, float3(_R, _G, _B));

                // Gri tonlama yoğunluğunu uygula
                float finalGray = lerp(texColor.r, gray, _GrayscaleIntensity);

                // Parlaklık ve kontrast hesaplama
                finalGray = (finalGray - 0.5) * _Contrast + 0.5;  // Kontrast
                finalGray += _Brightness;  // Parlaklık

                // Final renk (gri ton üzerine renk çarpanı ekleme)
                fixed4 grayscaleColor = fixed4(finalGray, finalGray, finalGray, texColor.a);
                return grayscaleColor * _TintColor;  // Renk çarpanı uygula
            }
            ENDCG
        }
    }
}
