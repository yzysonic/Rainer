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

			sampler2D _MainTex;
			sampler2D _GrassTex;
			float4 _MainTex_ST;
			float4 _CenterUV;
			float _BlushSize;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 r = i.uv-_CenterUV.xy;
				float r2 = dot(r, r);
				fixed4 source = tex2D(_MainTex, i.uv);

				if(r2 > _BlushSize * _BlushSize)
					return source;

				float a = 1-r2/(_BlushSize * _BlushSize);

				if(source.a > a)
					return source;

				return fixed4(0,1,0,1)*a;
			}
			ENDCG
		}
	}
}
