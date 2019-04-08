uniform float4 _OutlineColor;
uniform float _OutlineSize;

struct appdata
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
};

struct v2f
{
    float4 pos : SV_POSITION;
    float4 color : COLOR;
};

v2f vert(appdata i)
{
    v2f o;

    float4 normalDir = float4(normalize(i.normal), 0);
    float4 newPos = i.vertex + (0.1 * normalDir * _OutlineSize);        // normal extrusion technique
    
    o.pos = UnityObjectToClipPos(newPos);
    o.color = _OutlineColor;

    return o;
}

float4 frag(v2f i) : COLOR
{
    return i.color;
}