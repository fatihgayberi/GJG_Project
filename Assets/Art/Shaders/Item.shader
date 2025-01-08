Shader "GJG/Sprite/Item"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        [PerRendererData] _GrayscaleIntensity ("Grayscale Intensity", Range(0, 1)) = 1
        [PerRendererData] _Brightness ("Brightness", Range(-10, 10)) = 0
        [PerRendererData] _Contrast ("Contrast", Range(0, 20)) = 1
        [PerRendererData] _R ("_R", Range(-2, 10)) = 1
        [PerRendererData] _G ("_G", Range(-2, 10)) = 1
        [PerRendererData] _B ("_B", Range(-2, 10)) = 1
        [PerRendererData] _TintColor ("Tint Color", Color) = (1, 1, 1, 1)
        [PerRendererData] _Column ("Column Index (X)", Float) = 0
        [PerRendererData] _Row ("Row Index (Y)", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Tags { "LightMode"="ForwardBase" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
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

                UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float, _GrayscaleIntensity)
                #define _GrayscaleIntensity_arr Props
                UNITY_DEFINE_INSTANCED_PROP(float, _Brightness)
                #define _Brightness_arr Props
                UNITY_DEFINE_INSTANCED_PROP(float, _Contrast)
                #define _Contrast_arr Props
                UNITY_DEFINE_INSTANCED_PROP(float, _R)
                #define _R_arr Props
                UNITY_DEFINE_INSTANCED_PROP(float, _G)
                #define _G_arr Props
                UNITY_DEFINE_INSTANCED_PROP(float, _B)
                #define _B_arr Props
                UNITY_DEFINE_INSTANCED_PROP(fixed4, _TintColor)
                #define _TintColor_arr Props
                UNITY_DEFINE_INSTANCED_PROP(float, _Column)
                #define _Column_arr Props
                UNITY_DEFINE_INSTANCED_PROP(float, _Row)
                #define _Row_arr Props
   
                UNITY_INSTANCING_BUFFER_END(Props)

            sampler2D _MainTex;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float2 GetTileUV(float2 uv, float x, float y)
            {
                float2 tileSize = float2(1.0 / 3, 1.0 / 3);
                float2 tileOffset = float2(x * tileSize.x, y * tileSize.y);
                return uv * tileSize + tileOffset;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = GetTileUV(i.uv, UNITY_ACCESS_INSTANCED_PROP(_Column_arr, _Column), UNITY_ACCESS_INSTANCED_PROP(_Row_arr, _Row));

                fixed4 texColor = tex2D(_MainTex, uv);

                float gray = dot(texColor.rgb, float3(UNITY_ACCESS_INSTANCED_PROP(_R_arr, _R), UNITY_ACCESS_INSTANCED_PROP(_G_arr, _G), UNITY_ACCESS_INSTANCED_PROP(_B_arr, _B)));

                float finalGray = lerp(texColor.r, gray, UNITY_ACCESS_INSTANCED_PROP(_GrayscaleIntensity_arr, _GrayscaleIntensity));

                finalGray = (finalGray - 0.5) * UNITY_ACCESS_INSTANCED_PROP(_Contrast_arr, _Contrast) + 0.5; 
                finalGray += UNITY_ACCESS_INSTANCED_PROP(_Brightness_arr, _Brightness); 

                float4 lastCol = fixed4(finalGray, finalGray, finalGray, texColor.a) * UNITY_ACCESS_INSTANCED_PROP(_TintColor_arr, _TintColor);

                clip(lastCol .a -0.5);
               
                return lastCol;
            }
            ENDCG
        }
    }
}
