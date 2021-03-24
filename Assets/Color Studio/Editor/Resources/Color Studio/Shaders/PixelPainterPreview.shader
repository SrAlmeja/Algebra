Shader "Color Studio/PixelPainterPreview"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "black" {}
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
            #define GRID_WIDTH 0.49
            #define GRID_COLOR fixed4(0.7.xxx, 0.1)

			#include "UnityCG.cginc"

			struct appdata
			{
                float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
                float2 clipUV : TEXCOORD1;
			};

            
            sampler2D _GUIClipTexture;
            uniform float4x4 unity_GUIClipTextureMatrix;

			sampler2D _MainTex;
			float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            fixed4 _Color;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                float3 eyePos = UnityObjectToViewPos(v.vertex);
                o.clipUV = mul(unity_GUIClipTextureMatrix, float4(eyePos.xy, 0, 1.0));
				return o;
			}
			
			float4 frag (v2f i) : SV_Target {
                fixed4 texColor = tex2D(_MainTex, i.uv) * _Color;
                texColor.a *= tex2D(_GUIClipTexture, i.clipUV).a;

                fixed4 color = texColor;

                // grid
/*                float2 dd = abs(i.uv.xy - 0.5.xx);
                color = lerp(color, GRID_COLOR, (dd.x > GRID_WIDTH || dd.y > GRID_WIDTH) );
                color = saturate(color);
*/

				return color;
			}

			ENDCG
		}
	}
}

