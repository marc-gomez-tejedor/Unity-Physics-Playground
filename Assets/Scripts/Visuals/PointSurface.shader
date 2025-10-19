Shader "Graph/PointSurface"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalRenderPipeline" }
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceData.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 worldPos    : TEXCOORD0;
                float3 normalWS    : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _Smoothness;
                float _Metallic;
            CBUFFER_END

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                return OUT;
            }

            // "ConfigureSurface"
            void ConfigureSurface(Varyings IN, inout SurfaceData surfaceData)
            {   
                float3 worldColor = {0.0,0.0,0.0};
                worldColor.rg = IN.worldPos.xy * 0.5 + 0.5;
                // worldColor = abs(normalize(worldColor)); // optional: nicer mapping
                float3 albedo = worldColor * _BaseColor.rgb;
                surfaceData.albedo = albedo;
                surfaceData.specular = 0.5;     // specular intensity
                surfaceData.metallic = _Metallic;
                surfaceData.smoothness = _Smoothness;
                surfaceData.normalTS = half3(0.0, 0.0, 1.0);
                surfaceData.occlusion = 1.0;    // default full occlusion
                surfaceData.emission = 0.0;     // no emissive color
                surfaceData.alpha = 0.0;
                surfaceData.clearCoatMask = 0.0;
                surfaceData.clearCoatSmoothness = 0.0;
            }


            half4 frag (Varyings IN) : SV_Target
            {
                SurfaceData surfaceData;
                ConfigureSurface(IN, surfaceData);

                // Compute lighting (URP helper)
                // InputData lightingInput;
                // lightingInput.positionWS = IN.worldPos;
                // lightingInput.normalWS = normalize(IN.normalWS);
                // lightingInput.viewDirectionWS = GetWorldSpaceViewDir(IN.worldPos);
                // lightingInput.shadowCoord = TransformWorldToShadowCoord(IN.worldPos);
                // lightingInput.bakedGI = SAMPLE_GI(IN.worldPos, lightingInput.normalWS);
                // lightingInput.vertexLighting = 0;
                // lightingInput.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(IN.positionHCS);
                // lightingInput.fogCoord = 0;

                // Apply main light (PBR lighting)
                // half4 color = UniversalFragmentPBR(lightingInput, surfaceData);
                // _BaseColor.rgb = float3(IN.worldPos);
                half4 color = _BaseColor;
                color.rgb *= surfaceData.albedo;
                return color;
            }

            ENDHLSL
        }
    }
}
