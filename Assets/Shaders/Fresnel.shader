Shader "Unlit/Fresnel"
{
    Properties
    {
        _TimeSpeed("TimeSpeed",Range(0,15))=5
        _Color("Color",Color)=(1,1,1,1)
        _InnerColor("Inner Color", Color) = (0, 0, 0, 1)
        _Intensity("Intensity",Range(0,5))=2
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 wPos: TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            float _TimeSpeed;
            float3 _Color;
            float3 _InnerColor;
            float _Intensity;
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.wPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float3 V = normalize(_WorldSpaceCameraPos - i.wPos);
                float3 N = normalize(i.normal);

                float fresnel = (1 - dot(V, N)) * ((cos(_Time.y * _TimeSpeed)) * 0.5 + 0.5);
                //float3 outColor = _Color * fresnel;
                float innerFresnel = 1-fresnel;
                float3 outColor = (_Color * fresnel) + (_InnerColor * innerFresnel);
                return float4(outColor, 1);
            }
            ENDCG
        }
    }
}