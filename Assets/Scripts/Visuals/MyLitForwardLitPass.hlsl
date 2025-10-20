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
    float left = step(0.1, IN.local01.x);
    float bottom = step(0.1, IN.local01.y);
    float c = left * bottom;
    worldColor = float4(c,c,c,c);
    half4 color = _BaseColor * worldColor;
    return color;
}