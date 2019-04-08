fixed4 _Tint;
float _TintEffect;

void ApplyTint(inout float3 color)
{
    color = lerp(color, _Tint, _TintEffect);
}