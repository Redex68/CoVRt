Shader "CustomRenderTexture/VignetteGreyscale"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex("InputTex", 2D) = "white" {}
     }

     SubShader
     {
        Blend One Zero

        Pass
        {
            Name "VignetteGreyscale"

            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0

            float4      _Color;
            sampler2D   _MainTex;

            float4 frag(v2f_customrendertexture IN) : SV_Target
            {
                float2 uv = IN.localTexcoord.xy;
                float4 color = tex2D(_MainTex, uv) * _Color;
                float dist2center = distance(uv, float2(0.5, 0.5));
                float vignette = 1 - dist2center;

                float4 vignetted_color = saturate(color * vignette);

                float3 luminance_weights = normalize(float3(0.2126, 0.7152, 0.0722));

                float greyscale_mapped = dot(luminance_weights, vignetted_color.xyz);
                
                return vignetted_color;
            }
            ENDCG
        }
    }
}
