Shader "Graph/PointSurface"
{
    Properties
    {
        _Smoothness ("Smoothness", Range(0,1)) = 0.5

        _Saturation ("Saturation", Range(-1,1)) = 0.5
    }
    SubShader
    {
        CGPROGRAM
        #pragma surface ConfigureSurface Standard fullforwardshadows
        #pragma target 3.0

        struct Input 
        {
            float3 worldPos;
            float4 _Time;
        };

        float _Smoothness;
        float _Saturation;

        void ConfigureSurface (Input input, inout SurfaceOutputStandard surface)
        {
            surface.Albedo = saturate(input.worldPos * (0.25 + 0.25) * _Saturation);
            surface.Smoothness = _Smoothness;
            
        }

        ENDCG
    }
    FallBack "Diffuse"
}
