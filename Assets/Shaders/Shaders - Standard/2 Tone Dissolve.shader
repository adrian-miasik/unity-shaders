Shader "AdrianMiasik/Examples/2 Tone Dissolve"
{
     Properties
    {
        _Resolution("Resolution", float) = 30
        _Main("Main", Color) = (0.55, 0.84, 0.27, 1)
        _Edge("Edge", Color) = (0, 0.33, 0.61, 0.5)
        _EdgePercent("Edge Percent", Range(0, -1)) = -0.3333333
        _ProgressPercent("Progress Percent", Range(0, 1)) = 0.5
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

            // Unity noise functions source: https://docs.unity3d.com/Packages/com.unity.shadergraph@7.1/manual/Simple-Noise-Node.html
            inline float unity_noise_randomValue(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453);
            }

            inline float unity_noise_interpolate(float a, float b, float t)
            {
                return (1.0-t)*a + (t*b);
            }

            inline float unity_valueNoise(float2 uv)
            {
                float2 i = floor(uv);
                float2 f = frac(uv);
                f = f * f * (3.0 - 2.0 * f);

                uv = abs(frac(uv) - 0.5);
                float2 c0 = i + float2(0.0, 0.0);
                float2 c1 = i + float2(1.0, 0.0);
                float2 c2 = i + float2(0.0, 1.0);
                float2 c3 = i + float2(1.0, 1.0);
                float r0 = unity_noise_randomValue(c0);
                float r1 = unity_noise_randomValue(c1);
                float r2 = unity_noise_randomValue(c2);
                float r3 = unity_noise_randomValue(c3);

                float bottomOfGrid = unity_noise_interpolate(r0, r1, f.x);
                float topOfGrid = unity_noise_interpolate(r2, r3, f.x);
                float t = unity_noise_interpolate(bottomOfGrid, topOfGrid, f.y);
                return t;
            }

            float Unity_SimpleNoise_float(float2 UV, float Scale)
            {
                float t = 0.0;

                float freq = pow(2.0, float(0));
                float amp = pow(0.5, float(3-0));
                t += unity_valueNoise(float2(UV.x*Scale/freq, UV.y*Scale/freq))*amp;

                freq = pow(2.0, float(1));
                amp = pow(0.5, float(3-1));
                t += unity_valueNoise(float2(UV.x*Scale/freq, UV.y*Scale/freq))*amp;

                freq = pow(2.0, float(2));
                amp = pow(0.5, float(3-2));
                t += unity_valueNoise(float2(UV.x*Scale/freq, UV.y*Scale/freq))*amp;

                return t;
            }

            // Unity remap source: https://docs.unity3d.com/Packages/com.unity.shadergraph@7.2/manual/Remap-Node.html
            float4 Remap(float4 input, float2 inMinMax, float2 outMinMax)
            {
                return outMinMax.x + (input - inMinMax.x) * (outMinMax.y - outMinMax.x) / (inMinMax.y - inMinMax.x);
            }

            float _Resolution;
            float4 _Main;
            float4 _Edge;
            float _EdgePercent;
            float _ProgressPercent;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Generate noise
                float noise = Unity_SimpleNoise_float(i.uv, _Resolution);

                // Remap
                float alpha = Remap(noise, float2(-1, 1), float2(0, 1));
                float progress = Remap(_ProgressPercent, float2(-1.25, 1.25), float2(0, 1));

                // Edge
                float offset = alpha + _EdgePercent;
                float offsetStep = step(offset, progress);
                float4 edge = offsetStep * _Edge;

                // Inner
                float inner = abs(float4(1,1,1,1) - offsetStep);
                float4 inside = inner * _Main;

                // Alpha Clip Threshold
                clip(alpha-progress);

                // Composition
                return inside + edge;
            }
            ENDCG
        }
    }
}