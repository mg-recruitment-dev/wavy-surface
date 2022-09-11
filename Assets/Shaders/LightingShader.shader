Shader "Custom/LightingShader"
{
    SubShader
    {
        Tags { "Queue" = "Geometry" "RenderType" = "Opaque" "LightMode" = "ForwardBase" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR0;
                fixed4 diff : COLOR1;
            };


            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;

                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = nl * _LightColor0;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = i.color * i.diff;
                return col;
            }
            ENDCG
        }
    }
}
