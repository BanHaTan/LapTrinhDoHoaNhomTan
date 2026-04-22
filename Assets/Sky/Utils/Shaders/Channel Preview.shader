
Shader "Hidden/BOXOPHOBIC/Helpers/Channel Preview"
{
	Properties
	{
		_PreviewChannel("PreviewChannel", Float) = 0
		_PreviewLinear("PreviewLinear", Float) = 0
		_PreviewTex("PreviewTex", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		

		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		AlphaToMask Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		

		
		Pass
		{
			Name "Unlit"

			CGPROGRAM

			#define ASE_VERSION 19801


			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float _PreviewChannel;
			uniform sampler2D _PreviewTex;
			uniform float4 _PreviewTex_ST;
			uniform float _PreviewLinear;


			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}

			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float ifLocalVar15 = 0;
				if( _PreviewChannel == 5.0 )
				ifLocalVar15 = ceil( ( i.ase_texcoord1.xy.x * 4.0 ) );
				else if( _PreviewChannel < 5.0 )
				ifLocalVar15 = _PreviewChannel;
				float Mask19 = ifLocalVar15;
				float2 uv_PreviewTex = i.ase_texcoord1.xy * _PreviewTex_ST.xy + _PreviewTex_ST.zw;
				float4 tex2DNode10 = tex2D( _PreviewTex, uv_PreviewTex );
				float3 linearToGamma11 = LinearToGammaSpace( tex2DNode10.rgb );
				float3 lerpResult13 = lerp( linearToGamma11 , tex2DNode10.rgb , _PreviewLinear);
				float3 ifLocalVar3 = 0;
				if( Mask19 == 0.0 )
				ifLocalVar3 = lerpResult13;
				float4 appendResult14 = (float4(lerpResult13 , tex2DNode10.a));
				float4 break2 = appendResult14;
				float ifLocalVar5 = 0;
				if( Mask19 == 1.0 )
				ifLocalVar5 = break2.x;
				float ifLocalVar6 = 0;
				if( Mask19 == 2.0 )
				ifLocalVar6 = break2.y;
				float ifLocalVar7 = 0;
				if( Mask19 == 3.0 )
				ifLocalVar7 = break2.z;
				float ifLocalVar9 = 0;
				if( Mask19 == 4.0 )
				ifLocalVar9 = break2.w;
				

				finalColor = float4( ( ifLocalVar3 + ifLocalVar5 + ifLocalVar6 + ifLocalVar7 + ifLocalVar9 ) , 0.0 );
				return finalColor;
			}
			ENDCG
		}
	}
	
	
	Fallback Off
}
