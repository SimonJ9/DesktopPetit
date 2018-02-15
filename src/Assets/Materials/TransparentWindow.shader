// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/TransparentWindow" 
{
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_TransparentColorKey("Transparent Color Key", Color) = (0, 0, 0, 1)
		_TransparencyTolerance("Transparency Tolerance", Float) = 0.01
	}
	SubShader
	{
		Pass 
		{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			//a? to vertex
			struct a2v
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};

			//vertex to fragment
			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			//vertex shader function
			v2f vert(a2v input)
			{
				v2f output;
				//transform points from object space to camera's clip space in homogeneous coordinates
				output.pos = UnityObjectToClipPos(input.pos);
				output.uv = input.uv;

				return output;
			}

			sampler2D _MainTex;
			float3 _TransparentColorKey;
			float _TransparencyTolerance;

			//frag shader function
			float4 frag(v2f input) : SV_Target
			{
				float4 color = tex2D(_MainTex, input.uv);

				float deltaR = abs(color.r - _TransparentColorKey.r);
				float deltaG = abs(color.g - _TransparentColorKey.g);
				float deltaB = abs(color.b - _TransparentColorKey.b);

				if (deltaR < _TransparencyTolerance &&
					deltaG < _TransparencyTolerance &&
					deltaB < _TransparencyTolerance)
				{
					return float4(0.0f, 0.0f, 0.0f, 0.0f);
				}

				return color;
			}
			ENDCG
		}
	}
}
