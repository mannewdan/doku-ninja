// Made with Amplify Shader Editor v1.9.1.6
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/IconBasic"
{
	Properties
	{
		_Opacity("Opacity", Float) = 1
		_Color("Color", Color) = (1,1,1,1)
		_Texture("Texture", 2D) = "white" {}
		_Alpha("Alpha", 2D) = "white" {}
		_RotationMask("RotationMask", 2D) = "white" {}
		_RotationSpeed("RotationSpeed", Float) = 0
		_HighlightColor("HighlightColor", Color) = (0,0,0,0)
		_Desaturate("Desaturate", Float) = 0
		_DesaturateMultiply("DesaturateMultiply", Float) = 1
		_HighlightPower("HighlightPower", Float) = 0
		_RotationCenter("RotationCenter", Vector) = (0.5,0.5,0,0)
		_Lighten("Lighten", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		ZWrite On
		ZTest LEqual
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog 
		struct Input
		{
			float2 uv_texcoord;
			float2 uv2_texcoord2;
		};

		uniform float _Lighten;
		uniform float4 _Color;
		uniform sampler2D _Texture;
		uniform float4 _Texture_ST;
		uniform float2 _RotationCenter;
		uniform float _RotationSpeed;
		uniform float _Desaturate;
		uniform float _DesaturateMultiply;
		uniform float4 _HighlightColor;
		uniform float _HighlightPower;
		uniform sampler2D _Alpha;
		uniform float4 _Alpha_ST;
		uniform sampler2D _RotationMask;
		uniform float4 _RotationMask_ST;
		uniform float _Opacity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Texture = i.uv_texcoord * _Texture_ST.xy + _Texture_ST.zw;
			float temp_output_10_0 = ( _Time.y * _RotationSpeed );
			float cos14 = cos( temp_output_10_0 );
			float sin14 = sin( temp_output_10_0 );
			float2 rotator14 = mul( uv_Texture - _RotationCenter , float2x2( cos14 , -sin14 , sin14 , cos14 )) + _RotationCenter;
			float3 desaturateInitialColor19 = ( _Color * tex2D( _Texture, rotator14 ) ).rgb;
			float desaturateDot19 = dot( desaturateInitialColor19, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar19 = lerp( desaturateInitialColor19, desaturateDot19.xxx, _Desaturate );
			float lerpResult28 = lerp( 1.0 , _DesaturateMultiply , _Desaturate);
			float3 temp_output_30_0 = ( desaturateVar19 * lerpResult28 );
			float4 lerpResult23 = lerp( float4( temp_output_30_0 , 0.0 ) , ( float4( temp_output_30_0 , 0.0 ) * _HighlightColor ) , _HighlightPower);
			float4 lerpResult25 = lerp( lerpResult23 , _HighlightColor , ( _HighlightPower * 0.25 ));
			o.Emission = ( _Lighten + lerpResult25 ).rgb;
			float2 uv_Alpha = i.uv_texcoord * _Alpha_ST.xy + _Alpha_ST.zw;
			float cos8 = cos( temp_output_10_0 );
			float sin8 = sin( temp_output_10_0 );
			float2 rotator8 = mul( uv_Alpha - _RotationCenter , float2x2( cos8 , -sin8 , sin8 , cos8 )) + _RotationCenter;
			float2 uv1_RotationMask = i.uv2_texcoord2 * _RotationMask_ST.xy + _RotationMask_ST.zw;
			o.Alpha = ( tex2D( _Alpha, rotator8 ).r * tex2D( _RotationMask, uv1_RotationMask ).r * _Opacity );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19106
Node;AmplifyShaderEditor.TexturePropertyNode;16;-2429.471,-106.378;Inherit;True;Property;_Texture;Texture;2;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;12;-2382.974,199.1836;Inherit;False;Property;_RotationSpeed;RotationSpeed;5;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;11;-2377.674,125.1835;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-2136.964,-28.34412;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;27;-2162.653,252.8955;Inherit;False;Property;_RotationCenter;RotationCenter;11;0;Create;True;0;0;0;False;0;False;0.5,0.5;0.5,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-2115.175,122.5835;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;14;-1810.5,-22.97272;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;17;-1460.391,-284.7025;Inherit;False;Property;_Color;Color;1;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;-1548.343,-104.8214;Inherit;True;Property;_TextureSample1;Texture Sample 1;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;-1254.54,-20.21222;Inherit;False;Property;_DesaturateMultiply;DesaturateMultiply;8;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-1203.421,-99.01245;Inherit;False;Property;_Desaturate;Desaturate;7;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1186.001,-200.7553;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DesaturateOpNode;19;-965.4933,-201.4794;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;28;-799.54,-140.2122;Inherit;False;3;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;2;-2429.64,332.2567;Inherit;True;Property;_Alpha;Alpha;3;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-611.6848,-201.9665;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;22;-611.3145,-84.68015;Inherit;False;Property;_HighlightColor;HighlightColor;6;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-2137.132,410.2905;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-335.3143,-142.6801;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-379.2396,-8.960989;Inherit;False;Property;_HighlightPower;HighlightPower;9;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;8;-1810.668,415.6619;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;23;-134.3144,-200.6801;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-127.0674,-4.62965;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.25;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-1546.624,529.837;Inherit;True;Property;_RotationMask;RotationMask;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;1;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-1548.512,333.813;Inherit;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;31;-1388.985,720.9839;Inherit;False;Property;_Opacity;Opacity;0;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;25;211.4775,-99.2074;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;33;232.029,-177.6936;Inherit;False;Property;_Lighten;Lighten;12;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-1178.813,363.4408;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;447.029,-123.6936;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;708.935,4.663351;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Custom/IconBasic;False;False;False;False;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;False;False;Back;1;False;;3;False;;False;0;False;;0;False;;False;0;Custom;0.5;True;False;0;True;Transparent;;Transparent;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;False;2;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;10;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;2;16;0
WireConnection;10;0;11;0
WireConnection;10;1;12;0
WireConnection;14;0;15;0
WireConnection;14;1;27;0
WireConnection;14;2;10;0
WireConnection;13;0;16;0
WireConnection;13;1;14;0
WireConnection;18;0;17;0
WireConnection;18;1;13;0
WireConnection;19;0;18;0
WireConnection;19;1;20;0
WireConnection;28;1;29;0
WireConnection;28;2;20;0
WireConnection;30;0;19;0
WireConnection;30;1;28;0
WireConnection;9;2;2;0
WireConnection;21;0;30;0
WireConnection;21;1;22;0
WireConnection;8;0;9;0
WireConnection;8;1;27;0
WireConnection;8;2;10;0
WireConnection;23;0;30;0
WireConnection;23;1;21;0
WireConnection;23;2;24;0
WireConnection;26;0;24;0
WireConnection;4;0;2;0
WireConnection;4;1;8;0
WireConnection;25;0;23;0
WireConnection;25;1;22;0
WireConnection;25;2;26;0
WireConnection;7;0;4;1
WireConnection;7;1;6;1
WireConnection;7;2;31;0
WireConnection;32;0;33;0
WireConnection;32;1;25;0
WireConnection;0;2;32;0
WireConnection;0;9;7;0
ASEEND*/
//CHKSM=009FD19F16A240964A0C82E9988E73370B4AF47B