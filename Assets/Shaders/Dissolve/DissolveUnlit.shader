Shader "Unlit/DissolveUnlit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "white" {}
		[Header(Dissolve)]
		_Amount("Amount", Range(0, 1)) = 0
		_EdgeSize("Edge width", Range(0, 0.1)) = 0
		_EdgeColour1("Edge color 1", Color) = (1, 1, 1, 1)
		_EdgeColour2("Edge color 2", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags
		{ 
			"RenderType"="Transparent"
			"Queue" = "Transparent"
			"PreviewType" = "Plane"
		}

		Blend SrcAlpha OneMinusSrcAlpha

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
            float4 _MainTex_ST;

			sampler2D _NoiseTex;
			fixed _Amount;
			fixed _EdgeSize;
			fixed4 _EdgeColour1;
			fixed4 _EdgeColour2;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

			fixed4 getcol(float2 uv)
			{
				fixed cutout = tex2D(_NoiseTex, uv).r;
                fixed4 col = tex2D(_MainTex, uv);

				clip(cutout - _Amount);
				//if (cutout < _Amount) discard;//same as higher

				if(cutout < col.a && cutout < _Amount + _EdgeSize)
				{
					fixed percent01 = (cutout -_Amount) / _EdgeSize;
					col = lerp(_EdgeColour1, _EdgeColour2, percent01);
				}
					
                return col;
			}

            fixed4 frag (v2f i) : SV_Target
            {
				return getcol(i.uv);
            }
            ENDCG
        }
    }
}
