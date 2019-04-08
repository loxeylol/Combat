#ifndef UNITY_CG
    #include "UnityCG.cginc"
    #define UNITY_CG
    #define UNITY_SHADER_VARIABLES
    #define UNITY_INSTANCING
    #define HLSL_SUPPORT
#endif

struct appdata 
{
    float4 position : POSITION;
    float3 normal : NORMAL;
};

struct v2f 
{
    V2F_SHADOW_CASTER;
};

float4 vert(appdata v) : SV_POSITION
{
    v2f o;
    float4 position = UnityClipSpaceShadowCasterPos(v.position.xyz, v.normal);
    return UnityApplyLinearShadowBias(position);
    // TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
    // return o;
}

half4 frag() : SV_TARGET
{
    return 0;
}

// float4 frag(v2f i) : SV_Target
// {
// 	SHADOW_CASTER_FRAGMENT(i)
// }