// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/UnitFace"
{
	Properties
	{
		_Alpha("Alpha", 2D) = "white" {}
		[Toggle]_MaskIsDefaultColors("MaskIsDefaultColors", Float) = 0
		_Mask1("Mask 1", 2D) = "black" {}
		_D("D", Color) = (0,0,0,0)
		_R1("R1", Color) = (0,0,0,0)
		_G1("G1", Color) = (0,0,0,0)
		_B1("B1", Color) = (0,0,0,0)
		_Mask2("Mask 2", 2D) = "black" {}
		_R2("R2", Color) = (0,0,0,0)
		_G2("G2", Color) = (0,0,0,0)
		_B2("B2", Color) = (0,0,0,0)
		_HighlightPower("HighlightPower", Float) = 0
		_HighlightColor("HighlightColor", Color) = (0,0,0,0)
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_PupilSize("PupilSize", Float) = 0
		_PupilOffset("PupilOffset", Vector) = (0,0,0,0)
		_PupilColor("PupilColor", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "AlphaTest+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float2 uv_texcoord;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform sampler2D _Alpha;
		uniform float4 _Alpha_ST;
		uniform float _MaskIsDefaultColors;
		uniform float4 _D;
		uniform float4 _R1;
		uniform sampler2D _Mask1;
		uniform float4 _Mask1_ST;
		uniform float4 _PupilColor;
		uniform float2 _PupilOffset;
		uniform float _PupilSize;
		uniform float4 _G1;
		uniform float4 _B1;
		uniform float4 _R2;
		uniform sampler2D _Mask2;
		uniform float4 _Mask2_ST;
		uniform float4 _G2;
		uniform float4 _B2;
		uniform float4 _HighlightColor;
		uniform float _HighlightPower;
		uniform float _Cutoff = 0.5;

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float2 uv_Alpha = i.uv_texcoord * _Alpha_ST.xy + _Alpha_ST.zw;
			float2 uv_Mask1 = i.uv_texcoord * _Mask1_ST.xy + _Mask1_ST.zw;
			float4 tex2DNode2 = tex2D( _Mask1, uv_Mask1 );
			float4 lerpResult18 = lerp( _D , _R1 , tex2DNode2.r);
			float2 uv_TexCoord35 = i.uv_texcoord * float2( 4,4 ) + _PupilOffset;
			float2 _Vector2 = float2(1,1);
			float2 temp_output_42_0 = ( uv_TexCoord35 % _Vector2 );
			float2 uv_TexCoord64 = i.uv_texcoord * float2( 4,4 ) + ( _PupilOffset * float2( -1,1 ) );
			float2 ifLocalVar68 = 0;
			if( uv_TexCoord35.x >= 0.0 )
				ifLocalVar68 = temp_output_42_0;
			else
				ifLocalVar68 = ( ( uv_TexCoord64 % _Vector2 ) * float2( -1,1 ) );
			float4 lerpResult44 = lerp( lerpResult18 , _PupilColor , step( distance( ifLocalVar68 , float2( 0.5,0.5 ) ) , _PupilSize ));
			float4 lerpResult16 = lerp( lerpResult44 , _G1 , tex2DNode2.g);
			float4 lerpResult15 = lerp( lerpResult16 , _B1 , tex2DNode2.b);
			float2 uv_Mask2 = i.uv_texcoord * _Mask2_ST.xy + _Mask2_ST.zw;
			float4 tex2DNode19 = tex2D( _Mask2, uv_Mask2 );
			float4 lerpResult14 = lerp( lerpResult15 , _R2 , tex2DNode19.r);
			float4 lerpResult13 = lerp( lerpResult14 , _G2 , tex2DNode19.g);
			float4 lerpResult12 = lerp( lerpResult13 , _B2 , tex2DNode19.b);
			float4 lerpResult9 = lerp( (( _MaskIsDefaultColors )?( tex2DNode2 ):( lerpResult12 )) , _HighlightColor , _HighlightPower);
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float4 clampResult8 = clamp( ( UNITY_LIGHTMODEL_AMBIENT + ase_lightColor ) , float4( 0,0,0,1 ) , float4(1.2,1.2,1.2,1) );
			c.rgb = ( lerpResult9 * clampResult8 * ase_lightAtten ).rgb;
			c.a = 1;
			clip( tex2D( _Alpha, uv_Alpha ).r - _Cutoff );
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows noshadow exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				UnityGI gi;
				UNITY_INITIALIZE_OUTPUT( UnityGI, gi );
				o.Alpha = LightingStandardCustomLighting( o, worldViewDir, gi ).a;
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18935
139;835;1564;769;5904.138;763.8222;1.3;True;False
Node;AmplifyShaderEditor.Vector2Node;66;-5495.846,-60.12012;Inherit;False;Constant;_Vector0;Vector 0;18;0;Create;True;0;0;0;False;0;False;-1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;40;-5512.59,-587.855;Inherit;False;Property;_PupilOffset;PupilOffset;15;0;Create;True;0;0;0;False;0;False;0,0;3.18,0.04;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-5297.738,-78.32007;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;69;-5069.536,-304.9222;Inherit;False;Constant;_Vector2;Vector 2;18;0;Create;True;0;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;64;-5102.334,-126.258;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;4,4;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;67;-4750.856,98.87975;Inherit;False;Constant;_Vector1;Vector 1;18;0;Create;True;0;0;0;False;0;False;-1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;35;-5107.595,-628.1854;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;4,4;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleRemainderNode;50;-4810.685,-127.9628;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleRemainderNode;42;-4816.642,-509.7221;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-4511.857,-124.8138;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;-1,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ConditionalIfNode;68;-4282.618,-602.2346;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;2;-3986.008,50.09582;Inherit;True;Property;_Mask1;Mask 1;2;0;Create;True;0;0;0;False;0;False;-1;None;1566a1db9be229544b9a667344e509c9;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;26;-3876.543,-1028.403;Inherit;False;Property;_D;D;3;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.234,0.234,0.234,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;38;-4080.475,-496.5457;Inherit;False;Property;_PupilSize;PupilSize;14;0;Create;True;0;0;0;False;0;False;0;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;34;-4065.913,-601.0474;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;20;-3872.37,-829.186;Inherit;False;Property;_R1;R1;4;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;45;-3481.712,-472.3218;Inherit;False;Property;_PupilColor;PupilColor;16;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.2509804,0.2509804,0.2509804,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;37;-3866.549,-601.9449;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;18;-3490.114,-792.4378;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;44;-3119.829,-650.8079;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;22;-3125.45,-500.0525;Inherit;False;Property;_G1;G1;5;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.2352941,0.2352941,0.2352941,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;16;-2771.346,-520.5236;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;30;-2768.023,-134.456;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;21;-2771.846,-400.1294;Inherit;False;Property;_B1;B1;6;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.482,0.2601705,0.2601705,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;15;-2394.228,-415.0256;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;19;-2391.757,90.23174;Inherit;True;Property;_Mask2;Mask 2;7;0;Create;True;0;0;0;False;0;False;-1;None;ab19d24bf0c959c438cbebae79211ef7;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;23;-2393.445,-280.7835;Inherit;False;Property;_R2;R2;8;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.6901961,0.372549,0.372549,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;14;-2029.934,-299.7824;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;24;-2032.12,-175.3571;Inherit;False;Property;_G2;G2;9;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;25;-1655.451,-69.66125;Inherit;False;Property;_B2;B2;10;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.397,0.3267988,0.28187,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;29;-1603.588,186.8558;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;13;-1656.343,-195.1424;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightColorNode;4;-951.0883,594.2213;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.FogAndAmbientColorsNode;6;-1048.177,515.0083;Inherit;False;UNITY_LIGHTMODEL_AMBIENT;0;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;12;-1322.048,-90.55625;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;32;-2401.025,291.0609;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;31;-1036.259,138.05;Inherit;False;Property;_MaskIsDefaultColors;MaskIsDefaultColors;1;0;Create;True;0;0;0;False;0;False;0;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-797.2613,421.8718;Inherit;False;Property;_HighlightPower;HighlightPower;11;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;11;-1012.255,258.3862;Inherit;False;Property;_HighlightColor;HighlightColor;12;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;7;-730.9378,514.5485;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector4Node;33;-1010.255,715.2665;Inherit;False;Constant;_MaxLighting;Max Lighting;14;0;Create;True;0;0;0;False;0;False;1.2,1.2,1.2,1;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LightAttenuation;3;-598.0739,643.7747;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;8;-549.4523,515.4146;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,1;False;2;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;9;-551.7009,240.7869;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-166.1947,239.974;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-379.5551,19.82372;Inherit;True;Property;_Alpha;Alpha;0;0;Create;True;0;0;0;False;0;False;-1;None;da404ae0273cc2446bd32b0cbc59f76f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;35.16718,-0.6841431;Float;False;True;-1;6;ASEMaterialInspector;0;0;CustomLighting;Custom/UnitFace;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Opaque;;AlphaTest;ForwardOnly;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;13;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;65;0;40;0
WireConnection;65;1;66;0
WireConnection;64;1;65;0
WireConnection;35;1;40;0
WireConnection;50;0;64;0
WireConnection;50;1;69;0
WireConnection;42;0;35;0
WireConnection;42;1;69;0
WireConnection;63;0;50;0
WireConnection;63;1;67;0
WireConnection;68;0;35;1
WireConnection;68;2;42;0
WireConnection;68;3;42;0
WireConnection;68;4;63;0
WireConnection;34;0;68;0
WireConnection;37;0;34;0
WireConnection;37;1;38;0
WireConnection;18;0;26;0
WireConnection;18;1;20;0
WireConnection;18;2;2;1
WireConnection;44;0;18;0
WireConnection;44;1;45;0
WireConnection;44;2;37;0
WireConnection;16;0;44;0
WireConnection;16;1;22;0
WireConnection;16;2;2;2
WireConnection;30;0;2;3
WireConnection;15;0;16;0
WireConnection;15;1;21;0
WireConnection;15;2;30;0
WireConnection;14;0;15;0
WireConnection;14;1;23;0
WireConnection;14;2;19;1
WireConnection;29;0;19;3
WireConnection;13;0;14;0
WireConnection;13;1;24;0
WireConnection;13;2;19;2
WireConnection;12;0;13;0
WireConnection;12;1;25;0
WireConnection;12;2;29;0
WireConnection;32;0;2;0
WireConnection;31;0;12;0
WireConnection;31;1;32;0
WireConnection;7;0;6;0
WireConnection;7;1;4;0
WireConnection;8;0;7;0
WireConnection;8;2;33;0
WireConnection;9;0;31;0
WireConnection;9;1;11;0
WireConnection;9;2;10;0
WireConnection;5;0;9;0
WireConnection;5;1;8;0
WireConnection;5;2;3;0
WireConnection;0;10;1;1
WireConnection;0;13;5;0
ASEEND*/
//CHKSM=75212827007FECCBCB08971E0A5714C81D260B62