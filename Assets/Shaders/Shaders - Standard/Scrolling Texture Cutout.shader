Shader "AdrianMiasik/Examples/Scrolling Texture Cutout"
{
    Properties
    {
        _Texture("Texture", 2D) = "white" {}
        _VerticalSpeed("Vertical Speed", Float) = 0.2
        _HorizontalSpeed("Horizontal Speed", Float) = 0.2
        _Scale("Scale", Float) = 1
        _Background("Background", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

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

            Texture2D _Texture;

            // The sampler state forces our texture to use filtering mode 'linear' and wrap mode 'repeat' regardless of texture settings
            SamplerState sampler_linear_repeat;

            // Needed for tiling and offset
            float4 _Texture_ST;

            float _Scale;
            float _VerticalSpeed;
            float _HorizontalSpeed;
            float4 _Background;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float2 scaledUVs = v.uv * _Scale;
                o.uv = TRANSFORM_TEX(scaledUVs, _Texture);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float x = i.uv.x;
                float y = i.uv.y;

                y += _Time * _VerticalSpeed * 20;
                x += _Time * _HorizontalSpeed * 20;

                float4 tex = _Texture.Sample(sampler_linear_repeat, float2(x,y)); // Sampler State Source: https://docs.unity3d.com/Manual/SL-SamplerStates.html

                // Invert
                float4 invertedColors = float4(1.0f - tex.aaa, 1.0f);

                // Replace color
                float dis = distance(float4(1,1,1,1), invertedColors.x);
                float colorMask = lerp(float4(1,1,1,1), invertedColors.x, saturate((dis - 1) / 1));

                return colorMask * _Background;
            }
            ENDCG
        }
    }
}