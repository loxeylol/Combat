#ifndef UNITY_SHADER_VARIABLES
    #include "UnityShaderVariables.cginc"
    #define UNITY_SHADER_VARIABLES
#endif

float _SpecularThreshold;
float _SpecularSize;
fixed4 _SpecularColor;

float GetSpecular(float3 normal, float3 viewDir)
{
    float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
    float3 reflectionDir = normalize(reflect(-lightDir, normal));           // Not normalizing here will lead to star-shaped highlights    
    float specular = clamp(dot(viewDir, reflectionDir), 0.0, 1.0);
    specular = pow(specular, 100 * (1.0 - _SpecularSize));
    specular = step(_SpecularThreshold, specular);
    return specular;
}

void AddSpecular(inout float4 albedo, float3 normal, float3 viewDir)
{
    if(_SpecularSize <= 0.0)
        return;

    float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
    float3 reflectionDir = normalize(reflect(-lightDir, normal));           // Not normalizing here will lead to star-shaped highlights    
    
    float specular = clamp(dot(viewDir, reflectionDir), 0.0, 1.0);
    specular = pow(specular, 100 * (1.0 - _SpecularSize));

    specular *= step(_SpecularThreshold, specular);                          // step -> 0 if specular < _SpecularThreshold, 1 otherwise

    albedo = lerp(albedo, _SpecularColor, specular);
}