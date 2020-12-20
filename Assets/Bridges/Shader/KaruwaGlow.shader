Shader "Custom/KaruwaGlow" 
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}

		_KaruwaGlowColor ("Glow Color", Color) = (1,1,1,1)
		_KaruwaGlowPower ("Glow Power", Range(0.0,2.5)) = 1.0
		_KaruwaGlowTex ("Glow Texture", 2D) = "black" {}
		_KaruwaGlowTexColor ("Glow Texture Color", Color) = (1,1,1,1)
		_KaruwaGlowTexStrength ("Glow Texture Strength ", Range(0.0,10.0)) = 1.0
	}

	SubShader 
	{
	Tags { "RenderType"="KaruwaGlow" }
	LOD 200

CGPROGRAM
#pragma surface surf Lambert
#pragma target 2.0

sampler2D _MainTex;
fixed4 _Color;

sampler2D _KaruwaGlowTex ;
half _KaruwaGlowTexStrength;
fixed4 _KaruwaGlowTexColor;

struct Input
 {
	float2 uv_MainTex;
	float2 uv_KaruwaGlowTex;
};

void surf (Input IN, inout SurfaceOutput o) 
{
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	fixed3 d = tex2D(_KaruwaGlowTex, IN.uv_KaruwaGlowTex) *	_KaruwaGlowTexColor*1.2;
	c.rgb += (d.rgb * _KaruwaGlowTexStrength);
	o.Albedo = c.rgb;
	o.Alpha = c.a;
}
ENDCG
}

Fallback "Legacy Shaders/VertexLit"
}
