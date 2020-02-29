Shader "Custom/PickableObjectShader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
		_HighlightColor("Highlight Color", Color) = (1,1,1,1)
		_Speed("Speed", float) = 1

		//https://www.febucci.com/2019/04/pickable-objects-shader/
    }

    SubShader
    {
        Tags
		{ 
			"RenderType"="Opaque" 
		}

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert

        struct Input
        {
            float vertexPos;
        };

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.vertexPos = v.vertex.z; 
			//passes the vertex data to pixel shader, 
			//you can change it to a different axis or scale it (divide or multiply the value)
		}

        fixed4 _Color;
		fixed4 _HighlightColor;
		float _Speed;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float value = sin(_Time.y * _Speed - IN.vertexPos);
			//float percent01 = (value + 1) / 2;
			float percent01 = saturate(value);

            o.Albedo = lerp(_Color, _HighlightColor, percent01);
			//o.Emission = lerp(fixed3(0, 0, 0), _HighlightColor.xyz, percent01);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
