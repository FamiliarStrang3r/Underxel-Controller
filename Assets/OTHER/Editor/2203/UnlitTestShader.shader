Shader "Unlit/UnlitTestShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_ColorA("A", Color) = (1, 0, 0, 1)
		_ColorB("B", Color) = (1, 1, 0, 1)
		_Origin("Origin", float) = 0
		_Spread("Spread", Range(0.001, 1)) = 0
		_Percent("Percent", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags
		{
			"RenderType"="Opaque"
			"PreviewType"="Plane"
		}
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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

			fixed _Percent;
			fixed4 _ColorA;
			fixed4 _ColorB;
			float _Origin;
			float _Spread;

            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;
				//float2 uv01 = i.uv * 0.5 + 0.5;

				//float d1 = length(uv.x - 0.5);

				//fixed4 c = lerp(_ColorA, _ColorB, step(0, _Percent - uv.y));
				//fixed4 f = smoothstep(0, 1, d1);

				float amount = i.worldPos.x;
				//amount = step(0, _Percent - uv.x);
				amount -= _Origin;
				amount /= _Spread;
				fixed4 col = lerp(_ColorA, _ColorB, amount);

                return col;
            }
            ENDCG
        }
    }
}
