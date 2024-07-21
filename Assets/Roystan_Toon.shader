Shader "Roystan/Toon" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_MainTex ("Main Texture", 2D) = "white" {}
		[HDR] _AmbientColor ("Ambient Color", Vector) = (0.4,0.4,0.4,1)
		[HDR] _SpecularColor ("Specular Color", Vector) = (0.9,0.9,0.9,1)
		_Glossiness ("Glossiness", Float) = 32
		[HDR] _RimColor ("Rim Color", Vector) = (1,1,1,1)
		_RimAmount ("Rim Amount", Range(0, 1)) = 0.716
		_RimThreshold ("Rim Threshold", Range(0, 1)) = 0.1
		_FadeIntensity ("Fade Intensity", Range(0, 1)) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}