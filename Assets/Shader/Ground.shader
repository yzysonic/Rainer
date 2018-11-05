Shader "Custom/Ground" {
	Properties {
		_Division ("Division", Float) = 0.3
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


		void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input,o);
      	}

		void surf (Input IN, inout SurfaceOutputStandard o) {

			float2 index = floor(IN.uv_MainTex);
			float3 uv = float3(IN.uv_MainTex, index.y * _Division + index.x);

			// Albedo comes from a texture tinted by color
			fixed4 c = UNITY_SAMPLE_TEX2DARRAY (_MainTex, uv) * _Color;

			// if((uv.z+((uint)(uv.z/_Division)%2)) % 2 == 0) c *= 0.8f;

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
