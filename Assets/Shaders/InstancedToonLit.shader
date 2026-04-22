Shader "Custom/InstancedToonLitOutlined"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Color ("Color Tint", Color) = (1,1,1,1)

        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Glossiness ("Smoothness", Range(0,1)) = 0.5

        _BumpMap ("Normal Map", 2D) = "bump" {}
        _BumpScale ("Normal Strength", Range(0,2)) = 1.0

        _ShadowThreshold ("Shadow Threshold", Range(0,1)) = 0.5
        _ShadowSmoothness ("Shadow Smoothness", Range(0.001,0.2)) = 0.03

        _SpecularThreshold ("Specular Threshold", Range(0,1)) = 0.6
        _SpecularSmoothness ("Specular Smoothness", Range(0.001,0.2)) = 0.03

        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range(0,0.05)) = 0.01
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 250

        // Main toon-lit surface
        CGPROGRAM
        #pragma surface surf Toon fullforwardshadows addshadow
        #pragma target 3.0
        #pragma multi_compile_instancing

        #include "UnityCG.cginc"

        sampler2D _MainTex;
        sampler2D _BumpMap;

        half _Glossiness;
        half _BumpScale;
        half _ShadowThreshold;
        half _ShadowSmoothness;
        half _SpecularThreshold;
        half _SpecularSmoothness;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float3 viewDir;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
            UNITY_DEFINE_INSTANCED_PROP(float, _Metallic)
            UNITY_DEFINE_INSTANCED_PROP(float4, _OutlineColor)
            UNITY_DEFINE_INSTANCED_PROP(float, _OutlineWidth)
        UNITY_INSTANCING_BUFFER_END(Props)

        inline half4 LightingToon(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
        {
            half ndl = dot(s.Normal, lightDir);
            half diffuseBand = smoothstep(
                _ShadowThreshold - _ShadowSmoothness,
                _ShadowThreshold + _ShadowSmoothness,
                ndl
            );

            half3 diffuse = s.Albedo * _LightColor0.rgb * diffuseBand;

            half metallic = UNITY_ACCESS_INSTANCED_PROP(Props, _Metallic);

            half3 h = normalize(lightDir + viewDir);
            half ndh = saturate(dot(s.Normal, h));

            half shininess = lerp(8.0h, 128.0h, _Glossiness);
            half spec = pow(ndh, shininess);

            half specBand = smoothstep(
                _SpecularThreshold - _SpecularSmoothness,
                _SpecularThreshold + _SpecularSmoothness,
                spec
            );

            half3 specColor = lerp(half3(1.0h, 1.0h, 1.0h), s.Albedo, metallic);
            half3 specular = specBand * specColor * _LightColor0.rgb * metallic;

            half4 c;
            c.rgb = (diffuse + specular) * atten;
            c.a = s.Alpha;
            return c;
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 color = UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
            fixed4 c = tex * color;

            o.Albedo = c.rgb;
            o.Alpha = c.a;
            o.Normal = UnpackScaleNormal(tex2D(_BumpMap, IN.uv_BumpMap), _BumpScale);

            o.Specular = 0.0;
            o.Gloss = _Glossiness;
        }
        ENDCG

        // Outline pass
        Pass
        {
            Name "Outline"
            Tags { "LightMode"="Always" }
            Cull Front
            ZWrite On
            ZTest LEqual
            ColorMask RGB

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
                UNITY_DEFINE_INSTANCED_PROP(float, _Metallic)
                UNITY_DEFINE_INSTANCED_PROP(float4, _OutlineColor)
                UNITY_DEFINE_INSTANCED_PROP(float, _OutlineWidth)
            UNITY_INSTANCING_BUFFER_END(Props)

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                fixed4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            v2f vert(appdata v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);

                float outlineWidth = UNITY_ACCESS_INSTANCED_PROP(Props, _OutlineWidth);
                fixed4 outlineColor = UNITY_ACCESS_INSTANCED_PROP(Props, _OutlineColor);

                float3 normalOS = normalize(v.normal);
                float3 expandedPos = v.vertex.xyz + normalOS * outlineWidth;

                o.pos = UnityObjectToClipPos(float4(expandedPos, 1.0));
                o.color = outlineColor;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                return i.color;
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}