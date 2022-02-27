// Alan Zucconi
// Journey Sand Shader
// Full tutorial here:
// https://www.alanzucconi.com/?p=10050
Shader "Alan Zucconi/Journey/Journey Sand" {
	Properties
	{
		_MainTex ("Albedo (RGB)", 2D) = "white" {}



		[Space(10)]
		[Header(Sand)]
		[Toggle] _SandEnabled("Sand Enabled", Int) = 1
		_SandTex("Sand Direction (RGB)", 2D) = "white" {}
		_SandStrength("Sand Strength", Range(0,1)) = 0.1



		[Space(10)]
		[Header(Terrain Diffuse)]
		[Toggle] _DiffuseEnabled("Terrain Diffuse Enabled", Int) = 1
		_TerrainColor("Terrain Color", Color) = (1,1,1,1)
		_ShadowColor("Shadow Color", Color) = (1,1,1,1)



		[Space(10)]
		[Header(Rim Lighting)]
		[Toggle] _RimEnabled("Rim Lighting Enabled", Int) = 1
		_TerrainRimPower("Terrain Rim Power", Float) = 1
		_TerrainRimStrength("Terrain Rim Power", Float) = 1
		_TerrainRimColor("Terrain Rim Color", Color) = (1,1,1,1)



		[Space(10)]
		[Header(Ocean Specular)]
		[Toggle] _OceanSpecularEnabled("Ocean Specular Enabled", Int) = 1
		_OceanSpecularPower("Ocean Specular Power", Float) = 1
		_OceanSpecularStrength("Ocean Specular Strengh", Float) = 1
		_OceanSpecularColor("Ocean Specular Color", Color) = (1,1,1,1)


		[Space(10)]
		[Header(Waves)]
		[Toggle] _WavesEnabled("Waves Enabled", Int) = 1
		_SteepnessSharpnessPower("Steepness Factor", Float) = 1
		_XZBlendPower("XZ Factor", Float) = 1
		_ShallowXTex("Shallow X Texture", 2D) = "white" {}
		_ShallowZTex("Shallow Z Texture", 2D) = "white" {}
		_SteepXTex("Steep X Texture", 2D) = "white" {}
		_SteepZTex("Steep Z Texture", 2D) = "white" {}
		_WaveBlend("Wave Blend", Range(0,1))=0.1


		[Space(10)]
		[Header(Glitter)]
		[Toggle] _GlitterEnabled("Glitter Enabled", Int) = 1
		_GlitterTex("Glitter Direction (R)", 2D) = "white" {}
		_GlitterThreshold("Glitter Threshold", Range(0,1)) = 1
		[HDR] _GlitterColor("Glitter Color", Color) = (1,1,1,1)
		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Journey fullforwardshadows
		#pragma target 4.0

		sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;

			float3 worldPos;


			// https://docs.unity3d.com/Manual/SL-SurfaceShaders.html
			float3 worldNormal;
			INTERNAL_DATA
		};

		// Passed to the lighting function
		float3 worldPos;


		// https://answers.unity.com/questions/301103/fading-in-a-normal-map.html
		// https://keithmaggio.wordpress.com/2011/02/15/math-magician-lerp-slerp-and-nlerp/
		inline float3 normalerp(float3 n1, float3 n2, float t)
		{
			return normalize(lerp(n1, n2, t));
		}




		// ---------------------------------
		// --- Waves (Details Heightmap)
		// ---------------------------------
		int _WavesEnabled;
		float _SteepnessSharpnessPower;
		float _XZBlendPower;
		sampler2D_float _ShallowXTex;	float4 _ShallowXTex_ST;
		sampler2D_float _ShallowZTex;	float4 _ShallowZTex_ST;
		sampler2D_float _SteepXTex;		float4 _SteepXTex_ST;
		sampler2D_float _SteepZTex;		float4 _SteepZTex_ST;
		float _WaveBlend;
		// W: world position
		// N: surface normal
		// L: light direction
		// V: view direction
		float3 WavesNormal (float3 W, float3 N, Input IN)
		{
			if (_WavesEnabled == 0)
				return N;

			// https://unitygem.wordpress.com/shader-part-2/
			// Converts the current surface normal from TANGENT to WORLD space
			// This allows us to compare it with UP_WORLD and RIGHT_WORLD
			float3 N_WORLD = WorldNormalVector(IN, N);

			// Calculates "steepness"
			//  => 0: shallow (flat surface)
			//  => 1: steep (90 degrees surface)
			float3 UP_WORLD = float3(0, 1, 0);
			float steepness = saturate(dot(N_WORLD, UP_WORLD));

			steepness = pow(steepness, _SteepnessSharpnessPower);
			steepness = 1 - steepness;
			//return float3(steepness, steepness, steepness);

			// Calculates "xness"
			// Slopes can be facing X or Z direction
			//  => 0: slope facing Z
			//  => 1: slope facing X
			float3 RIGHT_WORLD = float3(1, 0, 0);
			float xness = abs(dot(N_WORLD, RIGHT_WORLD)) * 2;
			//return float3(xness, xness, xness);

			// We need to sharpen xness around 0.5
			xness = xness * 2 - 1; // [0,1]->[-1,+1]
			//return float3(xness, xness, xness);
			xness = pow(abs(xness), 1.0 / _XZBlendPower) * sign(xness); // Sharpen around 0.5
			xness = xness * 0.5 + 0.5; // [-1, +1]->[0,1]
			//return float3(xness, xness, xness);//*(1 - steepness);

			// [0,1]->[-1,+1]
			float2 uv = W.xz;
			float3 shallowX	= UnpackNormal(tex2D(_ShallowXTex,	TRANSFORM_TEX(uv, _ShallowXTex	)));
			float3 shallowZ	= UnpackNormal(tex2D(_ShallowZTex,	TRANSFORM_TEX(uv, _ShallowZTex	)));
			float3 steepX	= UnpackNormal(tex2D(_SteepXTex,	TRANSFORM_TEX(uv, _SteepXTex	)));
			float3 steepZ	= UnpackNormal(tex2D(_SteepZTex,	TRANSFORM_TEX(uv, _SteepZTex	)));

			// Wave steep
			float3 S = normalerp
			(
				normalerp(shallowZ, shallowX, xness),
				normalerp(steepZ,   steepX,   xness),
				steepness
			);

			// Rotates N towards S based on _WaveBlend
			float3 Ns = normalerp(N, S, _WaveBlend);
			return Ns;
		}
		// ---------------------------------





		// ---------------------------------
		// --- Sand Normal
		// ---------------------------------
		int _SandEnabled;
		sampler2D_float _SandTex;
		float4 _SandTex_ST;
		float _SandStrength;
		// W: world position
		// N: surface normal
		// L: light direction
		// V: view direction
		float3 SandNormal (float3 W, float3 N)
		{
			if (_SandEnabled == 0)
				return N;

			// Random direction
			// [0,1]->[-1,+1]
			float2 uv = W.xz;
			float3 S = normalize(tex2D(_SandTex, TRANSFORM_TEX(uv, _SandTex)).rgb * 2 - 1);
			// Rotates N towards Ns based on _SandStrength
			float3 Ns = normalerp(N, S, _SandStrength);
			return Ns;
		}
		// ---------------------------------




		// ---------------------------------
		// --- Diffuse
		// ---------------------------------
		int _DiffuseEnabled;
		float3 _TerrainColor;
		float3 _ShadowColor;
		// N: surface normal
		// L: light direction
		float3 TerrainDiffuse(float3 N, float3 L)
		//float3 TerrainDiffuse(float3 N, float3 L, float3 V)
		{
			float NdotL = saturate(4 * dot(N * float3(1,0.3,1), L));

			if (_DiffuseEnabled == 0)
				NdotL = saturate(dot(N, L));

			float3 color = lerp(_ShadowColor, _TerrainColor, NdotL);
			return color;
		}
		// ---------------------------------




		// ---------------------------------
		// --- Rim Lighting
		// ---------------------------------
		int _RimEnabled;
		float _TerrainRimPower;
		float _TerrainRimStrength;
		float3 _TerrainRimColor;
		// N: surface normal
		// V: view direction
		float3 RimLighting(float3 N, float3 V)
		{
			if (_RimEnabled == 0)
				return 0;

			float rim = 1.0 - saturate(dot(N, V));
			rim = saturate(pow(rim, _TerrainRimPower) * _TerrainRimStrength);
			rim = max(rim, 0); // Never negative
			return rim * _TerrainRimColor;
		}
		// ---------------------------------





		// ---------------------------------
		// --- Ocean Specular
		// ---------------------------------
		int _OceanSpecularEnabled;
		float _OceanSpecularPower;
		float _OceanSpecularStrength;
		float3 _OceanSpecularColor;
		// N: surface normal
		// L: light direction
		// V: view direction
		float3 OceanSpecular (float3 N, float3 L, float3 V)
		{
			if (_OceanSpecularEnabled == 0)
				return 0;

			// Blinn-Phong
			float3 H = normalize(V + L); // Half direction
			float NdotH = max(0, dot(N, H));
			float specular = pow(NdotH, _OceanSpecularPower) * _OceanSpecularStrength;
			return specular * _OceanSpecularColor;
		}
		

		

		// ---------------------------------
		// --- Ocean Specular
		// ---------------------------------
		int _GlitterEnabled;
		sampler2D_float _GlitterTex;
		float4 _GlitterTex_ST;
		float _GlitterThreshold;
		float3 _GlitterColor;
		// N: surface normal
		// L: light direction
		// V: view direction
		float3 GlitterSpecular (float3 N, float3 L, float3 V, float3 W)
		{
			if (_GlitterEnabled == 0)
				return 0;

			// Random glitter direction
			// [0,1]->[-1,+1]
			float2 uv = W.xz;
			float3 G = normalize(tex2D(_GlitterTex, TRANSFORM_TEX(uv, _GlitterTex)).rgb * 2 - 1);

			// Light that reflects on the glitter and hits the eye
			float3 R = reflect(L, G);
			float NdotH = max(0, dot(R, V));
			
			// Only the strong ones
			if (NdotH < _GlitterThreshold)
				return 0;

			// Debug glitter
			//return NdotH * fixed3(1, 1, 1);

			return NdotH * _GlitterColor;
		}



		inline float4 LightingJourney (SurfaceOutput s, fixed3 viewDir, UnityGI gi)
		{
			// Original colour
			//fixed4 pbr = LightingStandard(s, viewDir, gi);

			float3 L = gi.light.dir;
			float3 V = viewDir;
			float3 N = s.Normal;
			
			float3 W = worldPos;


			// ------------------------------------------
			float3 diffuseColor	= TerrainDiffuse	(N, L);
			float3 rimColor		= RimLighting		(N, V);
			float3 oceanColor	= OceanSpecular		(N, L, V);
			float3 glitterColor	= GlitterSpecular	(N, L, V, W);



			float3 specularColor = saturate(max(rimColor, oceanColor));
			float3 color = diffuseColor + specularColor + glitterColor;
			return float4(color * s.Albedo, 1);
		}
 
		void LightingJourney_GI(SurfaceOutput s, UnityGIInput data, inout UnityGI gi)
		{
			//LightingStandard_GI(s, data, gi); 
		}



		void surf (Input IN, inout SurfaceOutput o)
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;

			worldPos = IN.worldPos;

			float3 W = worldPos;
			float3 N = float3(0, 0, 1); // Normal (in tangent space)

			N = WavesNormal(W, N, IN);
			//o.Albedo = N;
			N = SandNormal (W, N);

			o.Normal = N;
			//o.Normal = float3(0, 0, 1);
		}
		ENDCG
	}
	FallBack "Diffuse"
}