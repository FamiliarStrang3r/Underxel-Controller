Shader "Unlit/GenaShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Speed("Speed", float) = 0
    }

    SubShader
    {
		Tags 
		{ 
			"RenderType" = "Transparent"
			"Queue" = "Transparent"//Background, Geometry(or Opaque), AlphaTest, Transparent, Overlay
			"PreviewType" = "Plane"
		}

        Pass
        {
			Blend SrcAlpha OneMinusSrcAlpha

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				fixed4 red = fixed4(i.uv.r, i.uv.g, 0, 1);
				fixed4 blue = fixed4(i.uv.r, i.uv.g, 1, 1);
				float sinValue = sin(_Time.y * _Speed);
				float percent01 = (sinValue + 1) / 2;
				fixed4 color = lerp(red, blue, percent01);

				fixed4 col = tex2D(_MainTex, i.uv) * color;
                return col;
            }
            ENDCG
        }
    }
}
