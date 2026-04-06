Shader "Custom/InstancedLit"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Color ("Color Tint", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _BumpMap ("Normal Map", 2D) = "bump" {}
        _BumpScale ("Normal Strength", Range(0,2)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0
        #pragma multi_compile_instancing

        #include "UnityCG.cginc"

        sampler2D _MainTex;
        sampler2D _BumpMap;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
        UNITY_INSTANCING_BUFFER_END(Props)

        half _Glossiness;
        half _Metallic;
        half _BumpScale;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float4 tex = tex2D(_MainTex, IN.uv_MainTex);
            float4 color = UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
            tex *= color;

            o.Albedo = tex.rgb;
            o.Alpha = tex.a;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;

            // Normal mapping (compatible across Unity versions)
            float3 n = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            n.xy *= _BumpScale;
            n = normalize(n);
            o.Normal = n;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
