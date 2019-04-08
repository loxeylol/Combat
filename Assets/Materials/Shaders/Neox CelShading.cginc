sampler2D _RampTex;

float3 SampleLUT(float ndotl)
{
    return tex2D(_RampTex, float2(ndotl, 0.5)).rgb;
}

float3 GetCelLighting(float3 normal)
{
    float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);				
    float ramp = clamp(dot(normal, lightDir), 0.0, 1.0);
    return SampleLUT(ramp);
}