Shader "CustomRenderTexture/CCTV_DistortionShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex("InputTex", 2D) = "white" {}
        _Intensity("Intensity", Range(0,10)) = 1
        _Interlace("Interlacing", Integer) = 3
    }

    SubShader
    {
        Blend One Zero

        Pass
        {
            Name "CCTV_DistortionShader"

            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0

            float4      _Color;
            sampler2D   _MainTex;
            float _Intensity;
            uint _Interlace;
            
            float osc_rand(float seed) {
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
                if (uint(coord.y) % _Interlace == 0) {
                    return color * ((sin(_Time.z * 4.) * 0.1) + 0.75) + (osc_rand(_Time.y) * 0.05);
                }
                return color;
            }

            float4 frag(v2f_customrendertexture IN) : SV_Target
            {
                float2 uv = IN.localTexcoord.xy;
                float2 dimensions = _CustomRenderTextureInfo.xy;
                float2 displacement = float2(0.005, 0.001) * (0.5 - float2(osc_rand(_Time.x * 37.0 * uv.y), osc_rand(_Time.y * 37.0 * uv.x))) * _Intensity;
                float2 pixel_coords = uv * dimensions;
                float4 color = tex2D(_MainTex, uv + displacement) * _Color;
                /*
                float dist2center = distance(uv, float2(0.5, 0.5));
                float vignette = 1 - dist2center;

                float4 vignetted_color = saturate(color * vignette);

                float3 luminance_weights = normalize(float3(0.2126, 0.7152, 0.0722));

                float greyscale_mapped = dot(luminance_weights, vignetted_color.xyz);
                */
                float4 interlaced = interlace(pixel_coords, color);
                //return float4(1.0, 0.0, 1.0, 1.0);
                return interlaced;
                // TODO: Replace this by actual code!
                //uint2 p = uv.xy * 256;
                //return countbits(~(p.x & p.y) + 1) % 2 * float4(uv, 1, 1) * color;
            }
            ENDCG
        }
    }
}
