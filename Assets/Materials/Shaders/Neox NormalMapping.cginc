#ifndef UNITY_SHADER_VARIABLES
    #define UNITY_SHADER_VARIABLES
    #include "UnityShaderVariables.cginc"    
#endif

sampler2D _NormalTex;
float _BumpScale;

void SampleNormalMap(inout float3 normal, float4 tangent, float2 uv)
{
    normal = UnpackScaleNormal(tex2D(_NormalTex, uv), _BumpScale);
    float3 binormal = cross(normal, tangent.xyz) * (tangent.w * unity_WorldTransformParams.w);
    normal = normalize(binormal);    
}

float GetDiffuse(inout float3 normal, float4 tangent, float2 uv)
{
    float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
    SampleNormalMap(normal, tangent, uv);
    return clamp(dot(normal, lightDir), 0.0, 1.0);
}