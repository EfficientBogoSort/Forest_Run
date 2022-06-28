
Shader "Custom/FogShader"
{
    Properties
    {
        _NoiseTex("Noise Texture", 2D) = "black" {}
        stopTime("time", float) = 0

    }
    SubShader
    {
        ZWrite off ZTest Always Cull off

        Pass {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ RADIAL_DIST
            #pragma multi_compile _ LINEAR EXP
            #pragma multi_compile _ ImplementNoise

            #include "UnityCG.cginc"

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 uv_depth : TEXCOORD1;
                float3 ray : TEXCOORD2;
            };

            sampler2D _CameraDepthTexture;
            sampler2D _MainTex;

            float4 _MainTex_ST;
            fixed4 _InitialFogColor;
            fixed4 _FinalFogColor;
            float _Start;
            float _End;
            float _Density;
            float _FogDensityWithSkyUp;
            float _FogDensityWithSkyDown;
            float _MixingFactor;

            // skybox information
            samplerCUBE _SkyCubemap;
            half4 _SkyCubemap_HDR;
            half4 _SkyTint;
            half _SkyExposure;
            float _SkyRotation;

            sampler2D _NoiseTex;
            sampler2D _Noise;
            float _NoiseMovingSpeed;
            float _NoiseForwardSpeed;

            float stopTime;

            // Applies one of standard fog formulas, given fog coordinate (i.e. distance)
            half ComputeFogFactor(float distance)
            {
                float fog = 0.0;
            #if LINEAR
                // factor = (end-distance)/(end-start)
                fog = (_End - distance) / (_End - _Start);
            #elif EXP
                // factor = exp(-density*z)
                fog = _Density * distance;
                fog = exp2(-fog);
            #else //EXP2
                // factor = exp(-(density*z)^2)
                fog = _Density * distance;
                fog = exp2(-(fog * fog));
            #endif
                return saturate(fog);
            }

            float ComputeDistance(float3 ray, float depth)
            {
                float dist;
            #if RADIAL_DIST
                dist = length(ray * depth);
            #else
                dist = depth * _ProjectionParams.z;
            #endif
                // Built-in fog starts at near plane, so match that by
                // subtracting the near value. (Not a perfect approximation)
                dist -= _ProjectionParams.y;
                return dist;
            }

            float3 RotateAroundYAxis(float3 v, float deg)
            {
                float alpha = deg * UNITY_PI / 180.0;
                float sina, cosa;
                sincos(alpha, sina, cosa);
                float2x2 m = float2x2(cosa, -sina, sina, cosa);
                return float3(mul(m, v.xz), v.y).xzy;
            }

            v2f vert (appdata_full v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord.xy;
                o.uv_depth = v.texcoord.xy;
                o.ray = RotateAroundYAxis(v.texcoord1.xyz, -_SkyRotation);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {

                half4 sceneColor = tex2D(_MainTex, i.uv);

                // get the depth value of this pixel
                float zsample = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv_depth);
                float depth = Linear01Depth(zsample * (zsample < 1.0));

                // Compute fog amount.
                float viewDistance = ComputeDistance(i.ray, depth) - _Start;
                half fogFactor = ComputeFogFactor(max(0.0, viewDistance));

                #if ImplementNoise
                    // noise
                    float speedX = _Time.y * _NoiseMovingSpeed;
                    float speedY = _Time.y * _NoiseMovingSpeed;
                    float noise = tex2D(_NoiseTex, i.uv + float2(speedX,speedY));

                    noise = pow(noise,depth * stopTime * _NoiseForwardSpeed);
                    fogFactor = smoothstep(0.0, 1.0, fogFactor * noise);
                #endif

                // Change from initial fog color to final fog color
                half4 fogColor = lerp(_InitialFogColor, _FinalFogColor, _MixingFactor);

                half3 skyColor = DecodeHDR(texCUBE(_SkyCubemap, i.ray), _SkyCubemap_HDR);
                skyColor *= _SkyTint.rgb * _SkyExposure * unity_ColorSpaceDouble;

                half DensityWithSky = lerp(_FogDensityWithSkyDown, _FogDensityWithSkyUp, saturate(i.vertex.y/_ScreenParams.y));
                // Lerp between the sky color and the fog color with fog density with sky first
                // Then lerp between source color to resultant color with the fog amount.
               return lerp(lerp(half4(skyColor, 1), fogColor, DensityWithSky), sceneColor, fogFactor);
            }
            ENDCG
        }
    }
    Fallback off
}
