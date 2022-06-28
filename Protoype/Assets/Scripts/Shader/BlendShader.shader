Shader "Custom/BlendShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BlendFct ("Blend Factor", Float) = 0.5
		_SpecColor("Color", Color) = (1.0,1.0,1.0)
		_Shininess("Shininess", Float) = 10000
		_BlendColor("Blend Color", Color) = (0.831, 0.686, 0.216, 0.0) // default set to gold colour
		
	}
	SubShader
	{
		Pass
		{
			Tags {"LightMode" = "ForwardBase"}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;	
			uniform float _BlendFct;
			uniform float _Glossiness;
			uniform float4 _SpecColor;
			uniform float4 _BlendColor;
			uniform float4 _Shininess;

			// color of directional light
			uniform float4 _LightColor0;

			struct vertIn
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
				float4 normalDir : TEXCOORD1;
			};

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				vertOut o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.uv = v.uv;
				o.normalDir = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject));
				return o;
			}
			
			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
				float3 normalDir = v.normalDir;
				float atten = 10.0;

				// lighting
				float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
				float3 diffuseReflection = atten * _LightColor0.xyz * max(0.0, dot(normalDir, lightDir));

				// specular highlights
				float3 lightReflectDirection = reflect(-lightDir, normalDir);
				float3 viewDirection = normalize(float4(_WorldSpaceCameraPos.xyz, 1.0) - mul(unity_ObjectToWorld, v.vertex));
				float3 lightSeeDirection = max(0.0,dot(lightReflectDirection, viewDirection));
				float3 shininessPower = lightSeeDirection * _Shininess;
				float3 specularReflection = atten * _SpecColor.rgb  * shininessPower;
				
				// final
				float3 lightFinal = diffuseReflection + specularReflection + UNITY_LIGHTMODEL_AMBIENT;

				// blend color
				fixed4 col =(tex2D(_MainTex, v.uv) * _BlendFct) + (_BlendColor * (1.0f - _BlendFct));

				return float4(lightFinal * 2 * col, 0.0);
			}
 
			ENDCG
		}
	}
}