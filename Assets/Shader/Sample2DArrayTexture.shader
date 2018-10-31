// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Example/Sample2DArrayTexture"
{
    Properties
    {
        _2DArray ("Tex", 2DArray) = "" {}
        _SliceRange ("Slices", Range(0,16)) = 6
        _UVScale ("UVScale", Float) = 1.0
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // texture arrays are not available everywhere,
            // only compile shader on platforms where they are
            #pragma require 2darray

            #include "UnityCG.cginc"

            struct v2f
            {
                float3 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            UNITY_DECLARE_TEX2DARRAY(_2DArray);
            float _SliceRange;
            float _UVScale;
            // sampler2D _MainTex;
            float4 _2DArray_ST;


            v2f vert (float4 vertex : POSITION, float2 uv : TEXCOORD0)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(vertex);
                // o.uv.xy = (vertex.xy + 0.5) * _UVScale;
                o.uv.xy = TRANSFORM_TEX(uv, _2DArray);
                o.uv.z = (vertex.z + 0.5) * _SliceRange;
                return o;
            }


            half4 frag (v2f i) : SV_Target
            {
                return UNITY_SAMPLE_TEX2DARRAY(_2DArray, i.uv);
            }
            ENDCG
        }
    }
}