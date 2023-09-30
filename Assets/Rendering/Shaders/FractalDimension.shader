Shader "Custom/FractalDimension"
{
    // The properties block of the Unity shader. In this example this block is empty
    // because the output color is predefined in the fragment shader code.
    Properties
    { 
        [Header(Raymarching settings)]
        [Space(5)]
        _MAX_DST ("Max Distance", Range(1 ,30)) = 10 
        [IntRange] _MAX_STEP ("Max Steps", Range(50, 400)) = 150 
        [PowerSlider(10)] _EPSILON ("Epsilon", Range(0.0000001, 0.001)) = 0.0001
        _Scale ("Fractal scale", Range(1,2)) = 1.3
        _RepetitionScale ("Repetition scale", Range(1, 100)) = 1
        [KeywordEnum(Fixed, OW, WO, WS)] _OriginMode ("Origin mode", Integer) = 0
        _Origin ("Origin if Origin mode is 0", Vector) = (1, 1, 1, 0)
        [Toggle] _FlipMode ("Flip?", Integer) = 0

        [Header(Lighting settings)]
        [Space(5)]
        _MixColorA ("Color A", Color) = (1, 1, 1, 1)  
        _MixColorB ("Color B", Color) = (1, 1, 1, 1)  
        [Space(20)]
        _LightPosition ("Light position", Vector) = (1, 1, 1, 0)
        _SunColor ("Light Color", Color) = (1, 1, 1, 1)  
        _SunStrength ("Light Strength", Range(0,0.1)) = 0
        _Gloss ("Gloss", Range(0,20)) = 12
        [Space(20)]
        _FogColor ("Fog color", Color) = (0.5, 0.5, 0.5, 0.5)
        _FogDensity ("Fog density", Range(0,1)) = 0
        _FogOffset ("Fog offset", Range(0,100)) = 100
    }

    // The SubShader block containing the Shader code. 
    SubShader
    {
        // SubShader Tags define when and under which conditions a SubShader block or
        // a pass is executed.
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            Name "RayMarching pass"
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
            //#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl" 

            // The structure definition defines which variables it contains.
            // This example uses the Attributes structure as an input structure in
            // the vertex shader.
            struct Attributes
            {
                // The positionOS variable contains the vertex positions in object
                // space.
                float4 positionOS   : POSITION;
                // Make it work in VR
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                // The positions in this struct must have the SV_POSITION semantic.
                float4 positionHCS  : SV_POSITION; 
                float4 positionNDC  : TEXCOORD0;
                // Make it work in VR
                UNITY_VERTEX_OUTPUT_STEREO
            };            

            // The vertex shader definition with properties defined in the Varyings 
            // structure. The type of the vert function must match the type (struct)
            // that it returns.
            Varyings vert(Attributes IN)
            {
                // Declaring the output object (OUT) with the Varyings struct.
                Varyings OUT;
                // Make it work in VR
                UNITY_SETUP_INSTANCE_ID(IN); //Insert
                //UNITY_INITIALIZE_OUTPUT(Varyings, OUT); //Insert

                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT); //Insert
                
                //VertexPositionInputs spaces = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.positionNDC = ComputeScreenPos(OUT.positionHCS);
                // Returning the output.
                return OUT;
            }

            float _MAX_DST;
            float _EPSILON;
            uint _MAX_STEP;
            float _Scale;
            float _RepetitionScale;
            uint _OriginMode;
            float3 _Origin;
            uint _FlipMode;

            float4 _MixColorA;
            float4 _MixColorB;
            float4 _SunColor;
            float _Gloss;
            float3 _LightPosition;
            float _SunStrength;
            
            float _FogDensity, _FogOffset;
            float4 _FogColor;

            #define mod(x, y) ((x) - (y) * floor((x)/(y)))

            // taken from that one discord thing, and the distance estimator compendium
            // modified to have adjustable scale
            // Appollonian fractal sdf
            float de ( float3 p ) {
                float s = 3.0f, e;
                for ( int i = 0; i++ < 8; ) {
                    //p = ((p - 1.0) - 2.0 * floor((p-1.0) / 2.0)) - 1.0; // translation of glsl mod to hlsl
                    p = mod( p - 1.0f, 2.0f ) - 1.0f;
                    //s *= e = 1.3f / dot( p, p );
                    s *= e = _Scale / dot( p, p );
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
            float4 color(float3 position, float3 direction, float3 normal, float distance, uint steps) {
                
                float3 light = normalize(_LightPosition - position);
                float ca = saturate(dot(normal*0.5+0.5, light));
                float cb = saturate(float(steps) / _MAX_STEP);
                float4 col = saturate(lerp(_MixColorA, _MixColorB, cb));

                float3 sun = normalize(_LightPosition);
                col += _SunColor * max(0, dot(sun, normal)) * _SunStrength;
                
                float3 reflected = reflect(direction, normal);
                col += pow(max(dot(sun, reflected), 0), _Gloss) * _SunColor * 0.5 * _SunStrength;

                float fogfactor = (_FogDensity / sqrt(log(2))) * max(0, distance - _FogOffset);
                fogfactor = saturate(exp2(-fogfactor * fogfactor));
                float4 final = lerp(_FogColor, col, fogfactor);
                /* NOTE THIS NEEDS THE LIGHTING INCLUDE AT THE TOP OF THE FILE
                InputData input = (InputData)0;
                input.normalWS = normal;
                input.positionWS = position;
                input.viewDirectionWS = direction;

                SurfaceData surface = (SurfaceData)0;
                surface.albedo = _SunColor.rgb;
                surface.alpha = _SunColor.a;
                surface.specular = 1;
                surface.smoothness = _Gloss;
                float4 final;
                final = UniversalFragmentPBR(input, surface);
                */
                return final;
            }

            // determine the color of the pixel 
            half4 raymarch(float3 origin, float3 direction) {
                half4 output_color = half4(0.0, 0.0, 0.0, 1.0);
                float3 position = origin;
                float surface_dist = _MAX_DST;
                float ray_dist = 0;
                uint steps = 0;
                while (ray_dist < _MAX_DST && steps++ < _MAX_STEP){
                    // surface_dist = clamp(de(position), 0, _MAX_DST);
                    surface_dist = de(position);
                    if (surface_dist <= _EPSILON) {
                        //output_color.r = surface_dist;
                        float3 normal = estimate_normal(position + direction * surface_dist);
                        float4 col = color(position, direction, normal, ray_dist + surface_dist, steps);
                        output_color = col;
                        break;
                    }
                    position += direction * surface_dist;
                    ray_dist += surface_dist;
                }
                if (ray_dist >= _MAX_DST || steps >= _MAX_STEP) {
                    output_color = _FogColor;
                }
                return output_color;
            }


            
            // The fragment shader definition.            
            half4 frag(Varyings IN) : SV_Target
            {
                float2 ndc = IN.positionNDC.xy / IN.positionNDC.w; // This should let us determine what direction to raymarch in
                ndc = (ndc - 0.5) * 2; // because OF COURSE "normalized device coordinates" are not normalized device coordinates
                //return half4(ndc, 0, 1);
                // create a ray
                // Taken from Sebastian Lague's raymarching video
                float3 origin;
                float flip = _FlipMode ? -1 : 1;
                switch (_OriginMode) {
                    case 0:
                        origin = _Origin; break; // use static origin
                    case 1:
                        origin = mul(float4(_WorldSpaceCameraPos,0), unity_WorldToObject).xyz; break; // these seem to be the same when the origin is the same for all triangles
                    case 2: 
                        origin = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos,0)).xyz; break; // these seem to be the same when the origin is the same for all triangles
                    default:
                        origin = mul(unity_CameraToWorld, float4(0.0, 0.0, 0.0, 1.0)).xyz; break; // these seeem to be the same when the origin is the same for all triangles
                }    
                float4x4 invProj = unity_CameraInvProjection;//UNITY_MATRIX_P;
                //return half4(invProj[3].xyz, 1.0);
                // invProj[0][3] = 0;
                // invProj[1][3] = 0;
                // invProj[2][3] = 0;
                invProj[3].xyz = float3(0,0,0);
                origin /= _RepetitionScale;
                float3 direction = mul(invProj, float4(flip * ndc, 0.0, 1.0)).xyz;
                direction = mul(unity_CameraToWorld, float4(direction, 0.0)).xyz;
                //direction = mul(unity_WorldToCamera, float4(direction, 0.0)).xyz;
                direction = normalize(direction);

                half4 output_color = raymarch(flip * origin, direction);
                return output_color;
                // return half4(ndc , 0.0, 1.0);
            }
            ENDHLSL
        }
    }
}