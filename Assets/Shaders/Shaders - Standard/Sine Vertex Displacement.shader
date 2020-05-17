Shader "AdrianMiasik/Examples/Sine Vertex Displacement"
{
     Properties
    {
        _Frequency("Frequency", float) = 20
        _Speed("Speed", float) = 0.5
        _Amplitude("Amplitude", float) = 0.1
        _Axis("Axis", Vector) = (0.1, 1, 0.1)
        _Color("Axis", Color) = (0.23, 0.95, 0.33, 1)
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float _Frequency;
            float _Speed;
            float _Amplitude;
            float3 _Axis;
            float4 _Color;

                float4 Unity_Combine_float(float R, float G, float B, float A)
            {
                return float4(R, G, B, A);
            }

            v2f vert (appdata v)
            {
                v2f o;
                float time = _Speed * _Time * 200;
                float4 sineWave = sin(time + v.vertex * _Frequency) * _Amplitude;

                float3 container1 = _Axis.r * sineWave;
                float3 container2 = _Axis.g * sineWave;
                float3 container3 = _Axis.b * sineWave;

                float3 container4 = container1 + v.vertex.r;
                float3 container5 = container2 + v.vertex.g;
                float3 container6 = container3 + v.vertex.b;

                float4 results = Unity_Combine_float(container4, container5, container6, 1);

                o.vertex = UnityObjectToClipPos(results);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _Color;
            }
            ENDCG
        }
    }
}