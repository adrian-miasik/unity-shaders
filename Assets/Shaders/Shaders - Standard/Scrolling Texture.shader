Shader "AdrianMiasik/Examples/Scrolling Texture"
{
    Properties
    {
        _Texture("Texture", 2D) = "white" {}
        _VerticalSpeed("Vertical Speed", Float) = 0.2
        _HorizontalSpeed("Horizontal Speed", Float) = 0.2
        _Scale("Scale", Float) = 1
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

            sampler2D _Texture;
            float4 _Texture_ST; // Needed for tiling and offset
            float _Scale;
            float _VerticalSpeed;
            float _HorizontalSpeed;

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

                return tex2D(_Texture, float2(x,y));
            }
            ENDCG
        }
    }
}