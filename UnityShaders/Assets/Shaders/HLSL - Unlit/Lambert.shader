Shader "AdrianMiasik/Examples/Lambert"
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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            float4 _MainColor;
            float4 _AmbientLight;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = v.normal;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Direct light
                float3 lightSource = _WorldSpaceLightPos0.xyz;
                float lightFalloff = max(0, dot(lightSource, i.normal)); // 0f to 1f     
                float3 directDiffuseLight = _LightColor0 * lightFalloff;
                
                // Composite
                float3 diffuseLight = directDiffuseLight + _AmbientLight;
                float3 result = diffuseLight * _MainColor;
                               
                return float4(result, 1);
            }
            ENDCG
        }
    }
}
