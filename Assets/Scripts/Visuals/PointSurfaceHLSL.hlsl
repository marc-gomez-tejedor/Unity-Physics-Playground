// The Core.hlsl file contains definitions of frequently used HLSL
// macros and functions, and also contains #include references to other
// HLSL files (for example, Common.hlsl, SpaceTransforms.hlsl, etc.).
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"


CBUFFER_START(UnityPerMaterial)
float4 _BaseColor;
//float4 _ColorMap_ST;
float _Smoothness;
float _Metallic;
float3 _ObjectMin;
float3 _ObjectMax;
CBUFFER_END 

//TEXTURE2D(_ColorMap); SAMPLER(sampler_ColorMap);

//          ---Macro example---
#define MY_MACRO(myStruct) myStruct.positionOS * 0.5

struct StructureA
{
    float3 positionOS;
    float otherValue;
};
struct StructureB
{
    float3 positionOS;
    float4 otherColor;
};
void Work(StructureA structA, StructureB structB)
{
    float3 a = MY_MACRO(structA);
    float3 b = MY_MACRO(structB);
}
//          ---------------------

// The structure definition defines which variables it contains.
// This example uses the Attributes structure as an input structure in
// the vertex shader.
struct Attributes
{
    // The positionOS variable contains the vertex positions in object
    // space.
    float4 positionOS : POSITION;
    //float2 uv : TEXCOORD0;
};

struct Varyings
{
    // The positions in this struct must have the SV_POSITION semantic.
    float4 positionHCS : SV_POSITION;
    float3 worldpos : TEXCOORD0;
    float3 local01 : TEXCOORD1;
    //float2 uv : TEXCOORD0;
};



// The vertex shader definition with properties defined in the Varyings 
// structure. The type of the vert function must match the type (struct)
// that it returns.
Varyings vert(Attributes IN)
{
    // Declaring the output object (OUT) with the Varyings struct.
    Varyings OUT;
    // The TransformObjectToHClip function transforms vertex positions
    // from object space to homogenous space
    OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
    OUT.worldpos = TransformObjectToWorld(IN.positionOS.xyz);
    OUT.local01 = (IN.positionOS - _ObjectMin) / (_ObjectMax - _ObjectMin);
    //OUT.uv = TRANSFORM_TEX(IN.uv, _ColorMap);
    // Returning the output.
    return OUT;
}

// The fragment shader definition.            
half4 frag(Varyings IN) : SV_Target
{
    // Defining the color variable and returning it.
    //float4 colorSample = SAMPLE_TEXTURE2D(_ColorMap, sampler_ColorMap, IN.uv);
    float4 worldColor = { 0, 0, 0, 0 };
    //worldColor.rg = IN.worldpos.xy * 0.5 + 0.5;
    float2 a = float2(0.005, 0.015);
    float2 b = float2(0.025, 0.025);
    float2 lbBorders = smoothstep(a, b, IN.local01.xy);
    float2 trBorders = smoothstep(a, b, 1.0-IN.local01.xy);
    half4 color;
    color = half4(1, 1, 1, 1);
    
    //paint red (unpaint blue and green)
    float m = smoothstep(0.25, 0.26, IN.local01.x);
    m += smoothstep(0.6, 0.59, IN.local01.y);
    m = clamp(m, 0, 1);
    color.gb = float2(m, m);
    color.r = m+0.13;
    float m2 = m* smoothstep(0.941, 0.94, IN.local01.x) + smoothstep(0.61, 0.6, IN.local01.y);
    m2 = clamp(m2, 0, 1);
    color.b = m2;
    color.g = m * (m2 + 0.32);
    float m3 = smoothstep(0.11, 0.111, IN.local01.y) + smoothstep(0.721, 0.72, IN.local01.x);
    m3 = clamp(m3, 0, 1);
    color.rg *= m3;
    color.g += m * (m2) * (m3 + .025);
    color.b += m * (m2) * (m3 - 0.93);
        
    float2 lt1 = float2(0., 0.42);
    float2 lt2 = float2(0.25, 0.61);
    float2 ltQ = step(lt2, IN.local01.xy);
    ltQ += step(lt1, 1-IN.local01.xy);
    ltQ *= (smoothstep(0.26, 0.261, IN.local01.x) + smoothstep(0.231, 0.23, IN.local01.x));
    ltQ *= (smoothstep(0.105, 0.1051, IN.local01.x) + smoothstep(0.0751, 0.075, IN.local01.x) +
    smoothstep(0.601, 0.6, IN.local01.y));
    
    ltQ *= (smoothstep(0.75, 0.751, IN.local01.x) + smoothstep(0.721, 0.72, IN.local01.x));
    ltQ *= (smoothstep(0.97, 0.971, IN.local01.x) + smoothstep(0.941, 0.94, IN.local01.x));
    ltQ *= (smoothstep(0.82, 0.821, IN.local01.y) + smoothstep(0.791, 0.79, IN.local01.y));
    ltQ *= ((smoothstep(0.11, 0.111, IN.local01.y) + smoothstep(0.081, 0.08, IN.local01.y)) +
    (smoothstep(0.231, 0.23, IN.local01.x)));
    m = clamp(ltQ, 0, 1);
    color.rgba *= ltQ.x * ltQ.y;
    color.rgba = clamp(color.rgba,0,1);
    color.rgba *= 0.35;
    
    
    //color = half4(c, c, c, c);
    //float c = lbBorders.x * lbBorders.y * trBorders.x * trBorders.y;
    //worldColor = float4(c,c,c,c);
    //half4 color = _BaseColor * worldColor;
    return color;
}