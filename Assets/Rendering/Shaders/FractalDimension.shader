Shader "Custom/FractalDimension"
{
    // The properties block of the Unity shader. In this example this block is empty
    // because the output color is predefined in the fragment shader code.
    Properties
    { 
        _MAX_DST ("Max Distance", Range(1 ,30)) = 10 
        _MAX_STEP ("", Range(50, 400)) = 150 
        _EPSILON ("Epsilon", Float) = 0.0001
        _LightDir ("Light direction", Vector) = (1, 1, 1)
        _OriginMode ("Origin mode", Integer) = 0
        _Origin ("Origin if Origin mode is 0", Vector) = (1, 1, 1)
    }

    // The SubShader block containing the Shader code. 
    SubShader
    {
        // SubShader Tags define when and under which conditions a SubShader block or
        // a pass is executed.
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }

        Pass
        {
            // The HLSL code block. Unity SRP uses the HLSL language.
            HLSLPROGRAM
            // This line defines the name of the vertex shader. 
            #pragma vertex vert
            // This line defines the name of the fragment shader. 
            #pragma fragment frag

            // The Core.hlsl file contains definitions of frequently used HLSL
            // macros and functions, and also contains #include references to other
            // HLSL files (for example, Common.hlsl, SpaceTransforms.hlsl, etc.).
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"            

            // The structure definition defines which variables it contains.
            // This example uses the Attributes structure as an input structure in
            // the vertex shader.
            struct Attributes
            {
                // The positionOS variable contains the vertex positions in object
                // space.
                float4 positionOS   : POSITION;                 
            };

            struct Varyings
            {
                // The positions in this struct must have the SV_POSITION semantic.
                float4 positionHCS  : SV_POSITION; 
                float4 positionNDC  : TEXCOORD0;
            };            

            // The vertex shader definition with properties defined in the Varyings 
            // structure. The type of the vert function must match the type (struct)
            // that it returns.
            Varyings vert(Attributes IN)
            {
                // Declaring the output object (OUT) with the Varyings struct.
                Varyings OUT;

                VertexPositionInputs spaces = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionHCS = spaces.positionCS;
                OUT.positionNDC = spaces.positionNDC;
                // Returning the output.
                return OUT;
            }

            float3 _LightDir;
            float _MAX_DST;
            float _EPSILON;
            uint _MAX_STEP;
            uint _OriginMode;
            float3 _Origin;

            #define mod(x, y) (x) - (y) * floor((x)/(y))

            // taken from that one discord thing, and the distance estimator compendium
            // Appollonian fractal sdf
            float de ( float3 p ) {
                float s = 3.0f, e;
                for ( int i = 0; i++ < 8; ) {
                    //p = ((p - 1.0) - 2.0 * floor((p-1.0) / 2.0)) - 1.0; // translation of glsl mod to hlsl
                    p = mod( p - 1.0f, 2.0f ) - 1.0f;
                    s *= e = 1.4f / dot( p, p );
                    p *= e;
                }
                return length( p.yz ) / s;
            }
            float3 estimate_normal(float3 p) {
                float3 dx = float3(_EPSILON, 0.0, 0.0);
                float3 dy = float3(0.0, _EPSILON, 0.0);
                float3 dz = float3(0.0, 0.0, _EPSILON);
                float x = de(p + dx) - de(p - dx);
                float y = de(p + dy) - de(p - dy);
                float z = de(p + dz) - de(p - dz);
                return normalize(float3(x, y, z));
            }

            // determine the color of the pixel 
            half4 raymarch(float3 origin, float3 direction) {
                half4 output_color = half4(0.0, 0.0, 0.0, 1.0);
                float3 position = origin;
                float surface_dist = _MAX_DST;
                float ray_dist = 0;
                uint steps = 0;
                while (ray_dist < _MAX_DST && steps++ < _MAX_STEP){
                    //output_color.r = steps / _MAX_STEP;
                    surface_dist = clamp(de(position), 0, _MAX_DST);
                    //output_color.r = surface_dist / 100000000;
                    if (surface_dist <= _EPSILON) {
                        //output_color.r = surface_dist;
                        float3 normal = estimate_normal(position + direction * surface_dist);
                        output_color.xyz = normal;
                        break;
                    }
                    position += direction * surface_dist;
                    ray_dist += surface_dist;
                }
                return output_color;
            }

            
            // The fragment shader definition.            
            half4 frag(Varyings IN) : SV_Target
            {
                float2 ndc = IN.positionNDC.xy / IN.positionNDC.w; // This should let us determine what direction to raymarch in

                // create a ray
                // Taken from Sebastian Lague's raymarching video
                float3 origin;
                switch (_OriginMode) {
                    case 0:
                        origin = _Origin; break;
                    case 1:
                        origin = mul(_WorldSpaceCameraPos, unity_WorldToObject); break;
                    case 2: 
                        origin = mul(unity_WorldToObject, _WorldSpaceCameraPos); break;
                    default:
                        origin = mul(unity_CameraToWorld, float4(0.0, 0.0, 0.0, 1.0)).xyz; break;
                }    
                float3 direction = mul(unity_CameraInvProjection, float4(ndc, 0.0, 1.0)).xyz;
                direction = mul(unity_CameraToWorld, float4(direction, 0.0)).xyz;
                direction = normalize(direction);

                half4 output_color = raymarch(origin, direction);
                return output_color;
                // return half4(ndc , 0.0, 1.0);
            }
            ENDHLSL
        }
    }
}