Shader "OrbWars/Cel_Transparent"
{
	Properties
	{
		_Color ("Color", Color) = (1, 1, 1, 1)		
		_MainTex ("Albedo", 2D) = "white" {}
		[NoScaleOffset] _RampTex ("Toon Ramp", 2D) = "white" {}
		_SpecularThreshold("Specular Sharpness", Range(0, 1)) = 0.5
		_SpecularSize("Specular Size", Range(0, 0.99)) = 0.0
		_SpecularColor ("Specular Color", Color) = (1, 1, 1, 1)
		_IndirectLight ("Indirect Light", Range(0, 3)) = 1
		_Tint("Tint", Color) = (1, 1, 1, 1)
		_TintEffect("Tint Effect", Range(0, 1)) = 0
	}
	SubShader
	{
		/* GLOBAL INCLUDES */
		CGINCLUDE			
			#include "UnityCG.cginc"
				#define UNITY_CG
				#define UNITY_SHADER_VARIABLES
				#define UNITY_INSTANCING
				#define HLSL_SUPPORT
			
			// Required for SHADOW_COORDS and secondary light pass
			#include "AutoLight.cginc"
			 	#define UNITY_AUTO_LIGHT
				#define UNITY_SHADOW_LIBRARY
				#define HLSL_SUPPORT
			
			// Required for UnityLight-struct
			#include "UnityLightingCommon.cginc"			
				#define UNITY_SHADOW_LIBRARY
				#define UNITY_LIGHTING_COMMON
			
			#include "Neox CelShading.cginc"
			#include "Neox Specular.cginc"
			#include "Neox ColorTinting.cginc"
		ENDCG

		/* MAIN PASS */
		Pass
		{
			Tags
			{
				"RenderType" = "Transparent"
				"Queue" = "Transparent"
				"LightMode" = "ForwardBase"
			}

			Cull Off
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			/* Pragma */
			#pragma vertex vert
			#pragma fragment frag
			
			#pragma multi_compile _ VERTEXLIGHT_ON
			#pragma multi_compile _ SHADOWS_SCREEN

			/* Properties */
			float4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			/* Structs */
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float3 worldPos : TEXCOORD1;
				float3 viewDir : TEXCOORD2;
				float3 vertexLightColor : TEXCOORD3;
				SHADOW_COORDS(4)
			};

			/* Includes */
			#include "Neox LightCalc.cginc"				// Requires v2f struct definition
			
			/* Functions */
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);					// Apply Scale and Offset from _MainTex_ST to UVs				
				o.normal = UnityObjectToWorldDir(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.viewDir = normalize(_WorldSpaceCameraPos - o.worldPos);
				o.vertexLightColor = ComputeVertexLights(o.pos, o.normal);

				TRANSFER_SHADOW(o);				
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 albedo = tex2D(_MainTex, i.uv) * _Color;						// Get albedo
				AddSpecular(albedo, i.normal, i.viewDir);							// Add specular
				
				#ifdef SHADOWS_SCREEN
					UnityLight light = CalculateLightAndShadows(i);						// Get light data with shadows
				#else
					UnityLight light = CalculateLight(i.worldPos, i.normal);			// Get light data
				#endif

				AddIndirectLight(light, i.vertexLightColor);						//light.color += max(0, i.vertexLightColor);	// Add vertex lights
				float3 celLighting = SampleLUT(light.ndotl);						// Sample from LUT				

				float3 rgb = albedo * celLighting * light.color;					// Combine values

				ApplyTint(rgb);														// Lerp to tint

				return float4(rgb, _Color.a);
			}

			ENDCG
		}

		/* SHADOW CASTER PASS */
		//Pass
		//{
		//	Tags
		//	{
		//		"LightMode" = "ShadowCaster"
		//	}

		//	CGPROGRAM

		//	/* Pragma */
		//	#pragma target 3.0

		//	#pragma vertex vert
		//	#pragma fragment frag
		//	
		//	#pragma multi_compile_shadowcaster
		//	
		//	/* Includes */
		//	#include "Neox ShadowCaster.cginc"
		//	
		//	ENDCG
		//}
	}
}
