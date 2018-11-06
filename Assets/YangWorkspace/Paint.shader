// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Paint"{
	Properties{
		[HideInInspector]
		_MainTex("MainTex", 2D) = "white"
		//[HideInInspector]
		//_Blush("Blush", 2D) = "white"
		//[HideInInspector]
		//_BlushScale("BlushScale", FLOAT) = 0.1
		//[HideInInspector]
		//_PaintUV("Hit UV Position", VECTOR) = (0,0,0,0)
	}

		SubShader{

			CGINCLUDE

				struct app_data {
					float4 vertex:POSITION;
					float4 uv:TEXCOORD0;
				};

				struct v2f {
					float4 screen:SV_POSITION;
					float4 uv:TEXCOORD0;
				};

				sampler2D _MainTex;
				sampler2D _Blush;
				float4 _PaintUV;
				float _BlushScale;
				float4 _BlushColor;
			ENDCG

			Pass{
				CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag

				v2f vert(app_data i) {
					v2f o;
					o.screen = UnityObjectToClipPos(i.vertex);
					o.uv = i.uv;
					return o;
				}


				float4 frag(v2f i) : SV_TARGET {
					float2 v = i.uv - _PaintUV;
					float rv = dot(v, v);
					float rb = _BlushScale*_BlushScale;

					if (rv < rb)
					{
						float blend = rv / rb;

						return tex2D(_Blush, i.uv)*(1 - blend) + tex2D(_MainTex, i.uv)*blend;
					}

					return tex2D(_MainTex, i.uv);
				}

				ENDCG
			}
		}
}