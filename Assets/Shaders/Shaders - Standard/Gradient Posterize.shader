Shader "AdrianMiasik/Examples/Gradient Posterize"
{
    Properties
    {
        _TopColor("Top Color", Color) = (1,1,1,1)
        _BottomColor("Bottom Color", Color) = (0,0,0,1)
        _MaxValue("Max Value", Range(0,1)) = 0.75
        _MinValue("Min Value", Range(0,1)) = 0.25
        _Bands("Bands", Range(0,100)) = 10
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
            float _Bands;

            float Posterize(float numberOfBands, float target)
            {
                return round(target * numberOfBands) / numberOfBands;
            }

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
                t = Posterize(_Bands + 1, t);
                float3 blend = lerp(_BottomColor, _TopColor, t);
                return float4(blend, 1);
            }
            ENDCG
        }
    }
}