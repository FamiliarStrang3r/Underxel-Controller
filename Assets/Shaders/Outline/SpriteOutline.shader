Shader "Unlit/SpriteOutline"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white" {}
		_OutlineColor("Outline Color", Color) = (1,1,1,1)
		[MaterialToggle] _Outline("Outline", float) = 0
		_Size("Size", Range(0, 3)) = 1
		//[MaterialToggle] _PixelSnap ("Pixel snap", Float) = 0
		//[PerRendererData]

		//https://www.febucci.com/2019/06/sprite-outline-shader/
    }

    SubShader
    {
		Tags
		{
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
			"PreviewType" = "Plane"
			//"CanUseSpriteAtlas" = "True"
		}

		//Cull Off //easy way to make double-sided
 
		Blend SrcAlpha OneMinusSrcAlpha
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			//#pragma multi_compile _ PIXELSNAP_ON //user for pixel-perfect
 
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
            float4 _MainTex_TexelSize;
 
			fixed _Outline;
			fixed4 _OutlineColor;
			fixed _Size;
 
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
				
				if(_Outline > 0)
				{
					fixed leftPixel = tex2D(_MainTex, i.uv - float2(_MainTex_TexelSize.x * _Size, 0)).a;
					fixed rightPixel = tex2D(_MainTex, i.uv + float2(_MainTex_TexelSize.x * _Size, 0)).a;
					fixed upPixel = tex2D(_MainTex, i.uv + float2(0, _MainTex_TexelSize.y * _Size)).a;
					fixed bottomPixel = tex2D(_MainTex, i.uv - float2(0, _MainTex_TexelSize.y * _Size)).a;
 
					//fixed innerOutline = (1 - leftPixel * upPixel * rightPixel * bottomPixel) * col.a;
					fixed outline = max(max(leftPixel, upPixel), max(rightPixel, bottomPixel)) - col.a;
					return lerp(col, _OutlineColor, outline);
				}

				return col;
            }
            ENDCG
        }
    }
}