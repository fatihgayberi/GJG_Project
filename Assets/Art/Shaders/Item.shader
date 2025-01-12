Shader "GJG/Surface/Item"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _GridSize ("Grid Size", Vector) = (1, 1, 1, 1)
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
		Tags{ 
			"RenderType"="Transparent" 
			"Queue"="Transparent"
		}

		Blend SrcAlpha OneMinusSrcAlpha

		ZWrite off
		Cull off
        
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        float4 _GridSize;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        #pragma instancing_options assumeuniformscaling

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

        float2 GetTileUV(float2 uv, float x, float y)
        {
            float2 tileSize = float2(1.0 / _GridSize.x, 1.0 / _GridSize.y);
            float2 tileOffset = float2(x * tileSize.x, y * tileSize.y);
            return uv * tileSize + tileOffset;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 uv = GetTileUV(IN.uv_MainTex, UNITY_ACCESS_INSTANCED_PROP(_Column_arr, _Column), UNITY_ACCESS_INSTANCED_PROP(_Row_arr, _Row));

            fixed4 texColor = tex2D(_MainTex, uv);

            float gray = dot(texColor.rgb, float3(UNITY_ACCESS_INSTANCED_PROP(_R_arr, _R), UNITY_ACCESS_INSTANCED_PROP(_G_arr, _G), UNITY_ACCESS_INSTANCED_PROP(_B_arr, _B)));

            float finalGray = lerp(texColor.r, gray, UNITY_ACCESS_INSTANCED_PROP(_GrayscaleIntensity_arr, _GrayscaleIntensity));

            finalGray = (finalGray - 0.5) * UNITY_ACCESS_INSTANCED_PROP(_Contrast_arr, _Contrast) + 0.5; 
            finalGray += UNITY_ACCESS_INSTANCED_PROP(_Brightness_arr, _Brightness); 

            float4 lastCol = fixed4(finalGray, finalGray, finalGray, texColor.a) * UNITY_ACCESS_INSTANCED_PROP(_TintColor_arr, _TintColor);

            clip(lastCol.a - 0.5);
                
            o.Albedo = lastCol.rgb;

        }
        ENDCG
    }
    FallBack "Diffuse"
}
