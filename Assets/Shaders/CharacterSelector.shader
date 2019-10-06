// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CharacterSelector"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_MainTex("_MainTex", 2D) = "white" {}
		_CutOut_Character("CutOut_Character", Range( 0 , 1)) = 0.5
		_HighlightCutout("HighlightCutout", Range( 0 , 1)) = 0.2
		_HighlightColor("HighlightColor", Color) = (1,0,0,1)
		_IsHighlighted("IsHighlighted", Int) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float4 _HighlightColor;
		uniform float _CutOut_Character;
		uniform float _HighlightCutout;
		uniform int _IsHighlighted;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 tex2DNode1 = tex2D( _MainTex, uv_MainTex );
			float temp_output_6_0 = step( tex2DNode1.a , _CutOut_Character );
			float4 lerpResult19 = lerp( tex2DNode1 , _HighlightColor , temp_output_6_0);
			o.Albedo = lerpResult19.rgb;
			o.Alpha = 1;
			float lerpResult24 = lerp( ( 1.0 - temp_output_6_0 ) , step( _HighlightCutout , tex2DNode1.a ) , (float)_IsHighlighted);
			clip( lerpResult24 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
350;403;1266;849;1003.77;-98.689;1.3;True;False
Node;AmplifyShaderEditor.SamplerNode;1;-791.1414,-86.841;Float;True;Property;_MainTex;_MainTex;1;0;Create;True;0;0;False;0;779bdb4d914592e45a29c37a9faad615;779bdb4d914592e45a29c37a9faad615;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;7;-849.48,143.9094;Float;False;Property;_CutOut_Character;CutOut_Character;2;0;Create;True;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;6;-443.0798,117.6457;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-841.475,335.1904;Float;False;Property;_HighlightCutout;HighlightCutout;3;0;Create;True;0;0;False;0;0.2;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;12;-466.1975,-217.6702;Float;False;Property;_HighlightColor;HighlightColor;4;0;Create;True;0;0;False;0;1,0,0,1;1,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;9;-446.3748,341.3436;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;23;-188.6249,235.2408;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;25;-395.2761,576.801;Float;False;Property;_IsHighlighted;IsHighlighted;5;0;Create;True;0;0;False;0;0;1;0;1;INT;0
Node;AmplifyShaderEditor.LerpOp;19;-190.1449,9.460271;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;24;-12.27612,316.8011;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;244.9391,10.84783;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;CharacterSelector;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;1;4
WireConnection;6;1;7;0
WireConnection;9;0;8;0
WireConnection;9;1;1;4
WireConnection;23;0;6;0
WireConnection;19;0;1;0
WireConnection;19;1;12;0
WireConnection;19;2;6;0
WireConnection;24;0;23;0
WireConnection;24;1;9;0
WireConnection;24;2;25;0
WireConnection;0;0;19;0
WireConnection;0;10;24;0
ASEEND*/
//CHKSM=A8FD6EE2EAEE1A78CB6CF280B2B87C0155EB628B