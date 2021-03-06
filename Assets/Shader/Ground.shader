﻿Shader "Custom/Ground" {
	Properties {
		_RangeLineWidth ("RangeLineWidth", Float) = 0.01
		_RangeRadius ("RangeRadius", Float) = 0.46
		_GrassFieldOpaticy("GrassFieldOpaticy", Range(0,1)) = 0.0
		_GrassTilingScale("GrassTilingScale", Float) = 1.0
		_Color ("Color", Color) = (1,1,1,1)
		_RangeLineColor ("RangeLineColor", Color) = (1,0,0,1)
		_MainTex ("MainTex", 2D) = "white" {}
		_GrassTex ("GrassTex", 2D) = "white" {}
		_GrassMask ("GrassMask", 2D) = "white" {}
		_GrassField("GrassField", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _GrassTex;
		sampler2D _GrassMask;
		sampler2D _GrassField;

		struct Input {
			float2 uv_MainTex;
			float2 uv_GrassMask;
		};

		float _RangeRadius;
		float _RangeLineWidth;
		float _GrassFieldOpaticy;
		float _GrassTilingScale;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _RangeLineColor;


		void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input,o);
      	}

		void surf (Input IN, inout SurfaceOutputStandard o) {

			// Albedo comes from a texture tinted by color
			fixed4 g = tex2D(_GrassMask, IN.uv_GrassMask);
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * (1-g.a) + tex2D(_GrassTex, IN.uv_MainTex*_GrassTilingScale)*g.a;

			fixed4 gf = tex2D(_GrassField, float2(1,1)-IN.uv_GrassMask);
			float opacity = gf.a * _GrassFieldOpaticy;
			c = c * (1 - opacity) + gf * opacity;

			o.Albedo = c.rgb * _Color;

			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			// o.Alpha = c.a;
			o.Alpha = 1;

			float2 r = IN.uv_GrassMask - float2(0.5f, 0.5f);
			float r2 = dot(r,r);
			if(r2 >= _RangeRadius*_RangeRadius && r2 <= (_RangeRadius+_RangeLineWidth)*(_RangeRadius+_RangeLineWidth)){
				o.Albedo *= _RangeLineColor;
			}

		}
		ENDCG
	}
	FallBack "Diffuse"
}
