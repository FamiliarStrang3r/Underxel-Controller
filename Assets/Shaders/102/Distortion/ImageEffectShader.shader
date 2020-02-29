Shader "Hidden/102-ImageEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
		[MaterialToggle] _Inverted("Inverted", float) = 0
		_Speed ("Color Speed", float) = 1
		_DisplaceTex("Displacement Texture", 2D) = "white" {}
		_Amount("Amount", Range(0, 1)) = 1
		_DisplaceSpeed("Speed", Vector) = (0, 0, 0, 0)
    }

    SubShader
    {
		Tags { "PreviewType"="Plane" }
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
			float _Speed;
			sampler2D _DisplaceTex;
			float _Amount;
			float2 _DisplaceSpeed;
			sampler2D _Mask;
			float _Inverted;

			fixed4 GetUVColor(float2 uv)
			{
				fixed4 red = fixed4(uv.r, uv.g, 0, 1);
				fixed4 blue = fixed4(uv.r, uv.g, 1, 1);
				float sinValue = sin(_Time.y * _Speed);
				float percent01 = (sinValue + 1) / 2;
				fixed4 color = lerp(red, blue, percent01);

				return color;
			}

			fixed4 GetWaveEffectColor(float2 uv)
			{
				float2 distuv = float2(uv.x - _Time.x * _DisplaceSpeed.x, uv.y - _Time.x * _DisplaceSpeed.y);

				float2 disp = tex2D(_DisplaceTex, distuv).xy;//animated
                //float2 disp = tex2D(_DisplaceTex, uv).xy;//not animated
				disp = ((disp * 2) - 1) * _Amount / 10;//converted to range -1..1

				fixed4 col = tex2D(_MainTex, uv + disp);

				return col;
			}

			fixed4 GetGrayscale(fixed4 c)
			{
				return (c.r + c.g + c.b) / 3;
			}

            fixed4 frag (v2f i) : SV_Target
            {
				fixed4 col = GetWaveEffectColor(i.uv) * GetUVColor(i.uv);
				//return col;

				float4 def = tex2D(_MainTex, i.uv);

				if (_Inverted == 0) return lerp(def, col, tex2D(_Mask, i.uv).r);
				else return lerp(col, def, tex2D(_Mask, i.uv).r);
            }

            ENDCG
        }
    }
}
