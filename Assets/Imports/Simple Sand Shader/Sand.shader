//MIT License
//
//Copyright(c) 2021 Daniel García Pampín
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files(the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions :
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

Shader "Custom/Sand"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _HeightMap("HeightMap", 2D) = "white" {}
        _HeightMapIntensity("HeightMapIntensity", Range(0, 1)) = 0.1
        [HideInInspector] _TrackedEntityPosition("TrackedEntityPosition", Vector) = (0, 0, 0, 0)
        [HideInInspector] _TrackedEntityVelocity("TrackedEntityVelocity", Vector) = (0, 0, 0, 0)
        [HideInInspector] _TrackedEntityRadius("TrackedEntityRadius", float) = 1.0
        [HideInInspector] _TrackedEntityPercentUndergroud("TrackedEntityPercentUndergroud", float) = 0.0
        _TrackedEntityMaxVelocity("TrackedEntityMaxVelocity", float) = 3.0
        _TrackedEntityVelocityContribution("TrackedEntityVelocityContribution", Range(0, 1)) = 0.5
        _UndulationInnerWidth("UndulationInnerWidth", float) = 0.1
        _UndulationOuterWidth("UndulationOuterWidth", float) = 0.25
        _UndulationMinHeight("UndulationMinHeight", float) = 0.0
        _UndulationMaxHeight("UndulationMaxHeight", float) = 0.3
        _Tesselation("Tessellation", Range(1,32)) = 3
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        CGPROGRAM

        #pragma surface surf BlinnPhong addshadow fullforwardshadows vertex:vert tessellate:tessDistance nolightmap
        #include "Tessellation.cginc"

        #define PI 3.1415926538

        float3 _TrackedEntityPosition;
        float3 _TrackedEntityVelocity;
        float _TrackedEntityRadius;
        float _TrackedEntityPercentUndergroud;
        float _TrackedEntityMaxVelocity;
        float _TrackedEntityVelocityContribution;
        float _UndulationInnerWidth;
        float _UndulationOuterWidth;
        float _UndulationMinHeight;
        float _UndulationMaxHeight;
        float _HeightMapIntensity;
        float _Tesselation;
        sampler2D _MainTex;
        sampler2D _HeightMap;
        uniform float4  _HeightMap_ST;

        struct Input 
        {
            float2 uv_MainTex;
        };

        struct appdata {
            float4 vertex : POSITION;
            float4 tangent : TANGENT;
            float3 normal : NORMAL;
            float2 texcoord : TEXCOORD0;
            float2 texcoord1 : TEXCOORD1;
        };

        float inverselerp(float x, float y, float z)
        {
            return (z - x) / (y - x);
        }

        float remap(float s, float a1, float a2, float b1, float b2)
        {
            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        }

        float4 tessDistance(appdata v0, appdata v1, appdata v2) {
            float minDist = 0.0;
            float maxDist = _UndulationInnerWidth + _UndulationOuterWidth + _TrackedEntityRadius + 0.5;

            float3 vertexWorldPosition = mul(unity_ObjectToWorld, v0.vertex);

            float3 vertexToEntity = _TrackedEntityPosition - vertexWorldPosition;

            float distance = length(vertexToEntity);
            distance = distance > maxDist ? 1 : _Tesselation;

            return distance;
        }

        void vert(inout appdata v) 
        {         
            // 1. Sand hole

            float3 vertexWorldPosition = mul(unity_ObjectToWorld, v.vertex);
            float3 sphereCenterWorldPosition = _TrackedEntityPosition;

            float3 sphereCenterToVertex = vertexWorldPosition - sphereCenterWorldPosition;
            float sphereCenterToVertexDistance = length(sphereCenterToVertex);

            if (sphereCenterToVertexDistance <= _TrackedEntityRadius)
            {
                float3 xz = sphereCenterToVertex;
                xz.y = 0;
                float x = length(xz);
                float y = sqrt(pow(_TrackedEntityRadius, 2) - pow(x, 2));

                v.vertex.y -= y - (-sphereCenterToVertex.y);
            }

            // 2. Sand undulation

            float3 sphereCenterToVertexXZ = sphereCenterToVertex;
            sphereCenterToVertexXZ.y = 0;
            float sphereCenterToVertexDistanceXZ = length(sphereCenterToVertexXZ);
            
            float noiseDisplacement = tex2Dlod(_HeightMap, float4(v.texcoord.x * _HeightMap_ST.x, v.texcoord.y * _HeightMap_ST.y,  0, 0)).r;
            
            _TrackedEntityVelocity = _TrackedEntityVelocity == float3 (0, 0, 0) ? float3(0, 0.000001, 0) : _TrackedEntityVelocity;
            float velocityDisplacement = dot(normalize(_TrackedEntityVelocity), normalize(sphereCenterToVertex));
            velocityDisplacement = remap(velocityDisplacement, -1, 1, 0, 1);
            velocityDisplacement = (min(length(_TrackedEntityVelocity), _TrackedEntityMaxVelocity) / _TrackedEntityMaxVelocity) * velocityDisplacement;
            velocityDisplacement *= _TrackedEntityVelocityContribution;
            velocityDisplacement = remap(velocityDisplacement, 0, 1, _UndulationMinHeight, _UndulationMaxHeight);
        
            // 2.1. Inner undulation
            
            float undulationDisplacement = inverselerp(0, _UndulationInnerWidth, sphereCenterToVertexDistanceXZ - _TrackedEntityRadius);

            if (undulationDisplacement >= 0 && undulationDisplacement <= 1) 
            {
                undulationDisplacement = (-1 * pow(1 - undulationDisplacement, 2)) + (0 * (1 - undulationDisplacement)) + 1;
                v.vertex.y += (undulationDisplacement * _TrackedEntityPercentUndergroud * velocityDisplacement);
                v.vertex.y += noiseDisplacement * _HeightMapIntensity * undulationDisplacement * _TrackedEntityPercentUndergroud;
            }
            else 
            {
                // 2.2 Outer undulation

                float undulationDisplacement = inverselerp(_UndulationOuterWidth, 0, sphereCenterToVertexDistanceXZ - _TrackedEntityRadius - _UndulationInnerWidth);

                if (undulationDisplacement >= 0 && undulationDisplacement <= 1)
                {
                    undulationDisplacement = 1 - (0.5f * cos((2 * PI * undulationDisplacement) / 2) + 0.5f);
                    v.vertex.y += (undulationDisplacement * _TrackedEntityPercentUndergroud * velocityDisplacement);
                    v.vertex.y += noiseDisplacement * _HeightMapIntensity * undulationDisplacement * _TrackedEntityPercentUndergroud;
                }         
            }
        }

        void surf(Input IN, inout SurfaceOutput o) 
        {
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
        }

        ENDCG
    }
        Fallback "Diffuse"
}