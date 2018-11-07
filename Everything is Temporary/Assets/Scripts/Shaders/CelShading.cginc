#include "UnityPBSLighting.cginc"

inline half4 LightingCel (SurfaceOutputStandard s, UnityGI gi)
{
    // TODO: Customize this function.
    half NdotL = floor (dot (s.Normal, gi.light.dir) * 4) / 4;
    half4 c;
    c.rgb = s.Albedo * gi.light.color.rgb * NdotL;
    c.a = s.Alpha;
    return c;
}

inline void LightingCel_GI (SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi)
{
    LightingStandard_GI(s, data, gi);
}