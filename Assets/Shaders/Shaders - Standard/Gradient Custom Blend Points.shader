Shader "AdrianMiasik/Examples/Gradient Custom Blend Points"
{
    Properties
    {
        _TopColor("Top Color", Color) = (1,1,1,1)
        _BottomColor("Bottom Color", Color) = (0,0,0,1)
        _MaxValue("Max Value", Range(0,1)) = 0.75
        _MinValue("Min Value", Range(0,1)) = 0.25
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 uv : TEXCOORD0;
            };

            float4 _TopColor;
            float4 _BottomColor;
            float _MaxValue;
            float _MinValue;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = smoothstep(_MinValue, _MaxValue,  i.uv.y);
                float3 blend = lerp(_BottomColor, _TopColor, t);
                return float4(blend, 1);
            }
            ENDCG
        }
    }
}