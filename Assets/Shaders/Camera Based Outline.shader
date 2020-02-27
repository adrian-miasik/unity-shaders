Shader "AdrianMiasik/Examples/Specular Cartoon With Outline"
{
    Properties
    {
        _MainColor("Color", Color) = (1,1,1,1)
        _AmbientLight("Ambient Light", Color) = (0.0,0.075,0.15, 1)
        _Gloss("Gloss", float) = 1
        _GlossColor("Gloss Color", Color) = (0.2,0.5,1, 1)
        _Color("Outline Color", Color) = (0.5,0.5,0.5, 1)
        _Width("Outline Width", float) = 1
    }
    SubShader
    {
        Tags {
            "RenderPipeline"="LightweightPipeline" 
            "RenderType"="Transparent" 
        }
        
        Pass
        {
            Tags { "LightMode"="LightweightForward" }
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
                
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"
            
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

            float4 _Color;
            float _Width;
            
            // Unsupported? 
            v2f vert (appdata v)
            {
                v2f o;

                VertexPositionInputs vertex = GetVertexPositionInputs(v.vertex);
                o.vertex = vertex.positionCS;
                
                VertexNormalInputs normal = GetVertexNormalInputs(v.normal);
                o.normal = normal.normalWS;
                
                float distanceToCamera = distance(mul((float3x3)unity_ObjectToWorld, o.vertex), _WorldSpaceCameraPos);
                v.vertex.xyz += normalize(o.normal) * _Width * distanceToCamera;
                o.vertex = TransformWorldToHClip(v.vertex);
                return o;
                
            }

            half4 frag (v2f i) : SV_Target
            {
                return _Color;
            }
            ENDHLSL
        }
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

            sampler2D _MainTex;
            float4 _MainColor;
            float4 _AmbientLight;
            float _Gloss;
            float4 _GlossColor;

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
                float3 normal = normalize(i.normal); // Interpolated
                float3 fragmentToCamera = _WorldSpaceCameraPos - i.worldPosition;
                float3 viewDirection = normalize(fragmentToCamera);
                
                // Direct light
                float3 lightSource = _WorldSpaceLightPos0.xyz;
                float lightFalloff = max(0, dot(lightSource, normal)); // 0f to 1f    
                lightFalloff = step(0.1, lightFalloff);             
                float3 directDiffuseLight = _LightColor0 * lightFalloff;
               
                // Phong
                float3 viewReflect = reflect(-viewDirection, normal);
                float4 specularFalloff = max(0,dot(viewReflect, lightSource));
                specularFalloff = pow(specularFalloff, _Gloss) * _GlossColor; // Add gloss
                specularFalloff = step(0.1, specularFalloff);
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
