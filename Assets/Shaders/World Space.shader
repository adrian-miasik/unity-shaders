Shader "AdrianMiasik/Examples/World Space"
{
    Properties
    {
        _MainColor("Color", Color) = (1,1,1,1)
        _AmbientLight("Ambient Light", Color) = (0.0,0.075,0.15, 1)
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
            #include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPosition : TEXCOORD0;
            };

            float4 _MainColor;
            float4 _AmbientLight;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPosition = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return float4(i.worldPosition, 1);
            }
            ENDCG
        }
    }
}
