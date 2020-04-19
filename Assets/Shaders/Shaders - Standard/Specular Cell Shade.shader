Shader "AdrianMiasik/Examples/Specular Cell Shade"
{
    Properties
    {
        _MainColor("Color", Color) = (1,1,1,1)
        _LightStepFalloff("Light Step Falloff", Range(0, 1)) = 0.1
        _AmbientLight("Ambient Light", Color) = (0.0,0.075,0.15, 1)
        _Gloss("Gloss", float) = 1
        _GlossStepFalloff("Gloss Step Falloff", Range(0, 1)) = 0.1
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
                float3 worldPosition : TEXCOORD0;
            };

            float4 _MainColor;
            float4 _AmbientLight;
            float _Gloss;
            float _LightStepFalloff;
            float _GlossStepFalloff;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = v.normal;
                o.worldPosition = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {            
                // General
                float3 normal = normalize(i.normal);
                float3 fragmentToCamera = _WorldSpaceCameraPos - i.worldPosition;
                float3 viewDirection = normalize(fragmentToCamera);
                
                // Direct light
                float3 lightSource = _WorldSpaceLightPos0.xyz;
                float lightFalloff = max(0, dot(lightSource, normal)); // 0f to 1f    
                lightFalloff = step(_LightStepFalloff, lightFalloff);             
                float3 directDiffuseLight = _LightColor0 * lightFalloff;
               
                // Phong
                float3 viewReflect = reflect(-viewDirection, normal);
                float specularFalloff = max(0,dot(viewReflect, lightSource));
                specularFalloff = pow(specularFalloff, _Gloss); // Add gloss
                specularFalloff = step(_GlossStepFalloff, specularFalloff);  
                float4 directSpecularLight = specularFalloff * _LightColor0;
                
                // Composite
                float3 diffuseLight = _AmbientLight + directDiffuseLight;
                float3 result = diffuseLight * _MainColor + directSpecularLight;
                               
                return float4(result, 1);
            }
            ENDCG
        }
    }
}