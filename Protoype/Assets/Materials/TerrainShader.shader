Shader "Custom/TerrainShader"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        const static int maxColourCount = 8;
        float minY;
        float maxY;
        float baseColourCount;
        float3 baseColours[maxColourCount];
        float baseStartHeights[maxColourCount];
        float colourTransitionRate;

        struct Input
        {
            float3 worldPos;
        };

        float invLerp(float minVal, float maxVal, float val) {
            return (val - minVal) / (maxVal - minVal);
        }

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float height = invLerp(minY, maxY, IN.worldPos.y);
            for (int i = 0; i < baseColourCount; i++) {
                float d = saturate(sign(height - (baseStartHeights[i])));
                // if this height is in ith types of height, and it is not the lowest type of height
                if (i != 0 && (height > baseStartHeights[baseColourCount - 1] || 
                    ((height - (baseStartHeights[i])) < (baseStartHeights[i+1] - baseStartHeights[i]) &&
                    (height > (baseStartHeights[i]))))) {

                    float mixingFactor = min(1, saturate(height - baseStartHeights[i]) * colourTransitionRate);
                    o.Albedo = o.Albedo * (1 - d) + lerp(baseColours[i-1], baseColours[i], mixingFactor) * d;
                } else {
                    o.Albedo = o.Albedo * (1 - d) + baseColours[i] * d;
                }

            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}
