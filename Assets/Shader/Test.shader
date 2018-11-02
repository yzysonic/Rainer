﻿Shader "Custom/Test" {
	Properties {
		_Division ("Division", Float) = 10
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2DArray) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert
		#pragma enable_d3d11_debug_symbols

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		UNITY_DECLARE_TEX2DARRAY(_MainTex);

		struct Input {
			float2 uv_MainTex;
		};

		float _Division;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float4 _MainTex_TexelSize;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input,o);
      	}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			float2 index;
			index.x = IN.uv_MainTex.x * (_MainTex_TexelSize.z-1) / (_MainTex_TexelSize.z/_Division);
			index.y = IN.uv_MainTex.y * (_MainTex_TexelSize.w-1) / (_MainTex_TexelSize.w/_Division);
			float slice = index.y * _Division + index.x;
			float3 uv = float3(IN.uv_MainTex.xy * _Division, 0);
			fixed4 c = UNITY_SAMPLE_TEX2DARRAY (_MainTex, uv) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
