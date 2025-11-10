// This shader fills the mesh shape with a color predefined in the code.
Shader "MarcsShaders/PointSurface"
{
    // The properties block of the Unity shader. In this example this block is empty
    // because the output color is predefined in the fragment shader code.
    Properties
    {
        [Header(Surface options)]
        [MainColor] _BaseColor ("Base Color", Color) = (1,1,1,1)
        [MainTexture] _ColorMap  ("Texture", 2D) = "white" {}
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }

    // The SubShader block containing the Shader code. 
    SubShader
    {
        // SubShader Tags define when and under which conditions a SubShader block or
        // a pass is executed.
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }

        Pass
        {
            Name "ForwardLit"  // for debugging
            Tags{"LightMode" = "UniversalForward"}
            // The HLSL code block. Unity SRP uses the HLSL language.
            HLSLPROGRAM
            // This line defines the name of the vertex shader. 
            #pragma vertex vert
            // This line defines the name of the fragment shader. 
            #pragma fragment frag

            #include "PointSurfaceHLSL.hlsl"
            ENDHLSL
        }
    } 
}