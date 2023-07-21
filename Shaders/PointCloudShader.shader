Shader "Custom/PointCloudShader"
{
	Properties
	{
		_PointSize("PointSize", Float) = 1
		_MainTex ("Texture", 2D) = "white" {}
	}

	SubShader
	{
		Pass
		{
			LOD 200

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct VertexInput
			{
				float4 v : POSITION;
				float4 color: COLOR;
				float2 uv : TEXCOORD0;
			};

			struct VertexOutput
			{
				float4 pos : SV_POSITION;
				float4 col : COLOR;
				float size : PSIZE;
				float2 uv : TEXCOORD0;
			};

			float _PointSize;
			sampler2D _MainTex;

			VertexOutput vert(VertexInput v)
			{
				VertexOutput o;
				o.pos = UnityObjectToClipPos(v.v);
				o.size = 3.0;
				o.col = v.color;
				o.uv = v.uv;

				return o;
			}

			// float4 frag(VertexOutput o) : SV_Target
			fixed4  frag(VertexOutput o) : SV_Target
			{
				fixed4 color = tex2D(_MainTex, o.uv);
				return color;
			}

			ENDCG
		}
	}
}
