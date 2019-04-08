#ifndef UNITY_CG
    #include "UnityCG.cginc"
    #define UNITY_CG
    #define UNITY_SHADER_VARIABLES
    #define UNITY_INSTANCING
    #define HLSL_SUPPORT
#endif

#ifndef UNITY_SHADER_VARIABLES
    #include "UnityShaderVariables.cginc"
    #define UNITY_SHADER_VARIABLES
#endif

#ifndef UNITY_AUTO_LIGHT
    #include "AutoLight.cginc"
    #define UNITY_AUTO_LIGHT
#endif

#ifndef UNITY_LIGHTING_COMMON
    #include "UnityLightingCommon.cginc"
    #define UNITY_LIGHTING_COMMON
#endif

UnityLight CalculateLight(float3 worldPos, float3 normal)
{
    UnityLight light;

    #if defined(POINT) || defined(POINT_COOKIE) || defined(SPOT)
        light.dir = normalize(_WorldSpaceLightPos0.xyz - worldPos);
        UNITY_LIGHT_ATTENUATION(attenuation, 0, worldPos);
    #else
        light.dir = _WorldSpaceLightPos0.xyz;
        float attenuation = 1.0;
    #endif

    light.color = _LightColor0.rgb * attenuation;

    light.ndotl = clamp(dot(normal, light.dir), 0.0, 1.0);
    return light;
}

#ifdef SHADOWS_SCREEN
    UnityLight CalculateLightAndShadows(v2f i)
    {
        UnityLight light;

        #if defined(POINT) || defined(POINT_COOKIE) || defined(SPOT)
            light.dir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos);
            UNITY_LIGHT_ATTENUATION(attenuation, i, i.worldPos);
        #else
            light.dir = _WorldSpaceLightPos0.xyz;
            UNITY_LIGHT_ATTENUATION(attenuation, i, i.worldPos);
            //float attenuation = 1.0;
        #endif

        light.color = _LightColor0.rgb * attenuation;

        light.ndotl = clamp(dot(i.normal, light.dir), 0.0, 1.0);
        return light;
    }
#endif

float3 ComputeVertexLights(float3 worldPos, float3 normal)
{
    #ifndef VERTEXLIGHT_ON
        return float3(0.0, 0.0, 0.0);
    #else
        return Shade4PointLights(
            unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
            unity_LightColor[0].rgb, unity_LightColor[1].rgb,
            unity_LightColor[2].rgb, unity_LightColor[3].rgb,
            unity_4LightAtten0, worldPos, normal
        );
    #endif
}

float _IndirectLight;
void AddIndirectLight(inout UnityLight l, float3 indirectLightColor)
{
    float3 indirect  = max(0, _IndirectLight * indirectLightColor);
    //l.color = saturate(l.color + indirect);
    //l.color += indirect;
    l.color = clamp(l.color + indirect, 0.0, 4.0);
}