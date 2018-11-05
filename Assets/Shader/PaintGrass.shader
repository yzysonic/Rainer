Shader "Custom/PaintGrass"
{
	Properties
	{
		_GrassTex("GrassTex", 2D) = "white"
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			struct fout
			{
				fixed4 col1 : SV_TARGET0;
				fixed4 col2 : SV_TARGET1;
				fixed4 col3 : SV_TARGET2;
				fixed4 col4 : SV_TARGET3;
			};

			UNITY_DECLARE_TEX2DARRAY(_MainTex);
			sampler2D _GrassTex;
			float4 _MainTex_ST;
			float2 _CenterUV;
			float _BlushScale;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fout frag (v2f i)
			{
				fout o;
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
