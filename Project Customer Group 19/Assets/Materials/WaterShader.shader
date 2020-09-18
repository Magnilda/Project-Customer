// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/WaterShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_UVScale("UV Scale", Range(0, 20)) = 1.0
		_Speed("Speed", Range(0, 1)) = 1.0
		_HeightScale("Height Scale", Range(0, 1)) = 1.0
		_WaveWidth("Wave Width", Range(0, 10)) = 1.0
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input
		{
			float3 worldPos;
			float3 worldNormal; INTERNAL_DATA
		};

		half _Glossiness;
		half _Metallic;
		half _UVScale;
		half _Speed;
		half _HeightScale;
		half _WaveWidth;
		fixed4 _Color;

#include "UnityCG.cginc"

		float3 mod289(float3 x)
		{
			return x - floor(x / 289.0) * 289.0;
		}

		float2 mod289(float2 x)
		{
			return x - floor(x / 289.0) * 289.0;
		}

		float3 permute(float3 x)
		{
			return mod289((x * 34.0 + 1.0) * x);
		}

		float3 taylorInvSqrt(float3 r)
		{
			return 1.79284291400159 - 0.85373472095314 * r;
		}

		float snoise(float2 v)
		{
			const float4 C = float4(0.211324865405187, // (3.0-sqrt(3.0))/6.0
				0.366025403784439, // 0.5*(sqrt(3.0)-1.0)
				-0.577350269189626, // -1.0 + 2.0 * C.x
				0.024390243902439); // 1.0 / 41.0
			float2 i = floor(v + dot(v, C.yy));
			float2 x0 = v - i + dot(i, C.xx);

			float2 i1;
			i1.x = step(x0.y, x0.x);
			i1.y = 1.0 - i1.x;

			float2 x1 = x0 + C.xx - i1;
			float2 x2 = x0 + C.zz;

			i = mod289(i);
			float3 p = permute(permute(i.y + float3(0.0, i1.y, 1.0)) + i.x + float3(0.0, i1.x, 1.0));

			float3 m = max(0.5 - float3(dot(x0, x0), dot(x1, x1), dot(x2, x2)), 0.0);
			m = m * m;
			m = m * m;

			float3 x = 2.0 * frac(p * C.www) - 1.0;
			float3 h = abs(x) - 0.5;
			float3 ox = floor(x + 0.5);
			float3 a0 = x - ox;

			m *= taylorInvSqrt(a0 * a0 + h * h);

			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.y = a0.y * x1.x + h.y * x1.y;
			g.z = a0.z * x2.x + h.z * x2.y;
			return 130.0 * dot(m, g);
		}

		float getHeight(float2 position)
		{
			float height = snoise(float2((_Time.g * _Speed) + (position.x * _UVScale * _WaveWidth), position.y * _UVScale)) * _HeightScale;
			height += snoise(float2((_Time.g * _Speed * 4) + (position.x * _UVScale * _WaveWidth * 2), position.y * _UVScale * 2)) * _HeightScale * 0.2;
			return height;
		}

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			float height = getHeight(float2(IN.worldPos.x, IN.worldPos.z));

			float3 posForward = float3(0.0, getHeight(float2(IN.worldPos.x, IN.worldPos.z + 0.01)), 0.01);
			float3 posRight = float3(0.01, getHeight(float2(IN.worldPos.x + 0.01, IN.worldPos.z)), 0.0);
			float3 pos = float3(0.0, height, 0.0);

			float3 fwd = normalize(posForward - pos);
			float3 right = normalize(posRight - pos);

			float3 vertNormal = WorldNormalVector(IN, o.Normal);
			float3 normal = mul(normalize(cross(fwd, right) + float3(0.0, 0.0, 1.0)), unity_WorldToObject);
			float3 bitangent = normalize(cross(vertNormal, float3(1.0, 0.0, 0.0)));
			float3 tangent = normalize(cross(vertNormal, bitangent));
			float3x3 tbn = float3x3(tangent, bitangent, vertNormal);

			normal = normalize(mul(tbn, normal));

			// Albedo comes from a texture tinted by color
			float4 c = _Color;

			o.Albedo = c.rgb;
			o.Normal = normal;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
		FallBack "Diffuse"
}
