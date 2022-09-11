Shader "Custom/Wireframe_WebGL"
{
	Properties
	{
		_WireColor("WireColor", Color) = (1,1,1,1)
		_Color("Color", Color) = (1,1,1,1)
	}

		SubShader
	{

		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM

			#include "UnityCG.cginc"
			#pragma  vertex   vert
			#pragma  fragment frag

			half4 _WireColor, _Color;

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
			};

			struct v2f
			{
				float4  vertex : SV_POSITION;
				float3  bary : TEXCOORD1;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.bary = v.color.rgb;

				return o;
			}

			half4 frag(v2f IN) : COLOR
			{
				float d = min(IN.bary.x, min(IN.bary.y, IN.bary.z));
				d = 1 - smoothstep(0, 0.04, d);

				return lerp(_Color, _WireColor, d);
			}

			ENDCG
		}
	}
}
