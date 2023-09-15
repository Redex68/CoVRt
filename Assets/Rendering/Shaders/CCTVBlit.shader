Shader "CCTVBlit"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 100
        ZWrite Off Cull Off
        Pass
        {
            Name "CCTVBlitPass"

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            // The Blit.hlsl file provides the vertex shader (Vert),
            // input structure (Attributes) and output strucutre (Varyings)
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            #pragma vertex Vert
            #pragma fragment frag

            TEXTURE2D_X(_CameraColorTexture);
            SAMPLER(sampler_CameraColorTexture);

            float _Intensity;
            // oscillating randomness
            float osc_rand(float seed){
                return frac(
                    sin(
                        dot(
                            float2(seed, seed), 
                            float2(12.9898, 78.233)
                            )
                        )
                    * 43758.5453);
            }

            float4 interlace(float2 coord, float4 color) {
                if (uint(coord.y) % 3 == 0){
                    return color * ((sin(_Time.y * 4.) * 0.1) + 0.75) + (osc_rand(_Time.y) * 0.05);
                }
                return color;
            }

            half4 frag (Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                float2 uv = input.texcoord;

                float2 displacement = float2(0.005, 0.001) * (0.5 - float2(osc_rand(_Time.y * 37.0 * uv.y), osc_rand(_Time.y * 37.0 * uv.x))); 

                float4 color = SAMPLE_TEXTURE2D_X(_CameraColorTexture, sampler_CameraColorTexture, uv + displacement);

                float2 pixel_coord = uv * _ScreenParams.xy;

                float dist2center = distance(uv, float2(0.5,0.5));
                float vignette = 1 - dist2center;

                float4 vignetted_color = saturate(color * vignette);

                float3 luminance_weights = normalize(float3(0.2126, 0.7152, 0.0722));

                float greyscale_mapped = dot(luminance_weights, vignetted_color.xyz);

                //float2 crt_line = fmod(uv.y, 1.0 / 120.0);

                return interlace(pixel_coord, greyscale_mapped);
            }
            ENDHLSL
        }
    }
}
