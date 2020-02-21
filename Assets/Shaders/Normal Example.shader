Shader "AdrianMiasik/Examples/Normals"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainColor("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normals : TEXCOORD1;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normals : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.normals = v.normals;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return float4(i.normals, 0);
            }
            ENDCG
        }
    }
}
