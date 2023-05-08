// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PBRMaskTint"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_Mask01("Mask01", 2D) = "white" {}
		_Mask02("Mask02", 2D) = "white" {}
		_SAM("SAM", 2D) = "white" {}
		_Emission("Emission", 2D) = "white" {}
		_Color01("Color01", Color) = (0.8078431,0.127983,0,0)
		_Color02("Color02", Color) = (0.0919526,0.3396226,0,0)
		_Color03("Color03", Color) = (0,0.1684175,0.7264151,0)
		_Color04("Color04", Color) = (0.8588235,0.811749,0.01176471,0)
		_Color05("Color05", Color) = (0.3828606,0.01214843,0.8584906,0)
		_Color06("Color06", Color) = (0.490566,0.2113431,0,0)
		[HDR]_EmissionColor("EmissionColor", Color) = (1,0.6413792,0,0)
		_EmissionPower("EmissionPower", Range( 0 , 4)) = 1
		_OverallBrightness("OverallBrightness", Range( 0 , 4)) = 1
		_Color01Power("Color01Power", Range( 0 , 4)) = 1
		_Color02Power("Color02Power", Range( 0 , 4)) = 2
		_Color03Power("Color03Power", Range( 0 , 4)) = 1
		_Color04Power("Color04Power", Range( 0 , 4)) = 1
		_Color05Power("Color05Power", Range( 0 , 4)) = 1
		_Color06Power("Color06Power", Range( 0 , 4)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _Mask01;
		uniform float4 _Mask01_ST;
		uniform float4 _Color01;
		uniform float _Color01Power;
		uniform float4 _Color02;
		uniform float _Color02Power;
		uniform float4 _Color03;
		uniform float _Color03Power;
		uniform sampler2D _Mask02;
		uniform float4 _Mask02_ST;
		uniform float4 _Color04;
		uniform float _Color04Power;
		uniform float4 _Color05;
		uniform float _Color05Power;
		uniform float4 _Color06;
		uniform float _Color06Power;
		uniform float _OverallBrightness;
		uniform sampler2D _Emission;
		uniform float4 _Emission_ST;
		uniform float4 _EmissionColor;
		uniform float _EmissionPower;
		uniform sampler2D _SAM;
		uniform float4 _SAM_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 tex2DNode2 = tex2D( _Albedo, uv_Albedo );
			float2 uv_Mask01 = i.uv_texcoord * _Mask01_ST.xy + _Mask01_ST.zw;
			float4 tex2DNode5 = tex2D( _Mask01, uv_Mask01 );
			float4 temp_cast_0 = (tex2DNode5.r).xxxx;
			float4 temp_cast_1 = (tex2DNode5.g).xxxx;
			float4 temp_cast_2 = (tex2DNode5.b).xxxx;
			float2 uv_Mask02 = i.uv_texcoord * _Mask02_ST.xy + _Mask02_ST.zw;
			float4 tex2DNode39 = tex2D( _Mask02, uv_Mask02 );
			float4 temp_cast_3 = (tex2DNode39.r).xxxx;
			float4 temp_cast_4 = (tex2DNode39.g).xxxx;
			float4 temp_cast_5 = (tex2DNode39.b).xxxx;
			float4 blendOpSrc20 = tex2DNode2;
			float4 blendOpDest20 = ( ( min( temp_cast_0 , _Color01 ) * _Color01Power ) + ( min( temp_cast_1 , _Color02 ) * _Color02Power ) + ( min( temp_cast_2 , _Color03 ) * _Color03Power ) + ( min( temp_cast_3 , _Color04 ) * _Color04Power ) + ( min( temp_cast_4 , _Color05 ) * _Color05Power ) + ( min( temp_cast_5 , _Color06 ) * _Color06Power ) );
			float4 lerpResult19 = lerp( tex2DNode2 , ( ( saturate( ( blendOpSrc20 * blendOpDest20 ) )) * _OverallBrightness ) , ( tex2DNode5.r + tex2DNode5.g + tex2DNode5.b + tex2DNode39.r + tex2DNode39.g + tex2DNode39.b ));
			o.Albedo = lerpResult19.rgb;
			float2 uv_Emission = i.uv_texcoord * _Emission_ST.xy + _Emission_ST.zw;
			float4 blendOpSrc55 = tex2D( _Emission, uv_Emission );
			float4 blendOpDest55 = _EmissionColor;
			o.Emission = ( ( saturate( ( blendOpSrc55 * blendOpDest55 ) )) * _EmissionPower ).rgb;
			float2 uv_SAM = i.uv_texcoord * _SAM_ST.xy + _SAM_ST.zw;
			float4 tex2DNode4 = tex2D( _SAM, uv_SAM );
			o.Metallic = tex2DNode4.b;
			o.Smoothness = tex2DNode4.r;
			o.Occlusion = tex2DNode4.g;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	//CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16100
0;0;1920;1018;2474.897;887.2007;2.189411;True;True
Node;AmplifyShaderEditor.SamplerNode;5;-2220.088,-303.5516;Float;True;Property;_Mask01;Mask01;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;6;-1733.469,-15.58027;Float;False;Property;_Color01;Color01;5;0;Create;True;0;0;False;0;0.8078431,0.127983,0,0;0,0.1394524,0.8088235,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;39;-2207.864,411.1313;Float;True;Property;_Mask02;Mask02;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;45;-1756.749,1553.88;Float;False;Property;_Color06;Color06;10;0;Create;True;0;0;False;0;0.490566,0.2113431,0,0;0,0.6176471,0.05537525,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;31;-1736.224,500.0226;Float;False;Property;_Color03;Color03;7;0;Create;True;0;0;False;0;0,0.1684175,0.7264151,0;0.4557808,0,0.6176471,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;40;-2026.842,874.9028;Float;False;Property;_Color04;Color04;8;0;Create;True;0;0;False;0;0.8588235,0.811749,0.01176471,0;0,0.6176471,0.05537525,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;44;-1834.067,1203.921;Float;False;Property;_Color05;Color05;9;0;Create;True;0;0;False;0;0.3828606,0.01214843,0.8584906,0;0,0.6176471,0.05537525,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;13;-1728.011,228.2081;Float;False;Property;_Color02;Color02;6;0;Create;True;0;0;False;0;0.0919526,0.3396226,0,0;0.4557808,0,0.6176471,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMinOpNode;15;-1390.061,-112.6214;Float;True;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-1463.482,128.3846;Float;False;Property;_Color01Power;Color01Power;14;0;Create;True;0;0;False;0;1;1;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMinOpNode;41;-1373.498,952.1104;Float;True;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-1437.975,449.8496;Float;False;Property;_Color02Power;Color02Power;15;0;Create;True;0;0;False;0;2;2;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMinOpNode;32;-1391.32,617.3629;Float;True;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMinOpNode;16;-1380.832,203.2694;Float;True;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMinOpNode;49;-1340.691,1600.192;Float;True;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1432.417,850.5742;Float;False;Property;_Color03Power;Color03Power;16;0;Create;True;0;0;False;0;1;1;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMinOpNode;48;-1332,1271.052;Float;True;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-1414.683,1851.656;Float;False;Property;_Color06Power;Color06Power;19;0;Create;True;0;0;False;0;1;1;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-1429.989,1499.063;Float;False;Property;_Color05Power;Color05Power;18;0;Create;True;0;0;False;0;1;1;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-1438.958,1189.747;Float;False;Property;_Color04Power;Color04Power;17;0;Create;True;0;0;False;0;1;1;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-1064.4,1136.132;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-1047.003,1563.31;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-1170.954,249.5483;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-1122.296,484.8992;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-1066.438,849.673;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-1169.706,35.47358;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;2;-1690.349,-611.7342;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-750.0344,-49.93935;Float;True;6;6;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;20;-419.7831,-247.2208;Float;False;Multiply;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;53;-684.936,312.3511;Float;True;Property;_Emission;Emission;4;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;52;-655.5795,631.9944;Float;False;Property;_EmissionColor;EmissionColor;11;1;[HDR];Create;True;0;0;False;0;1,0.6413792,0,0;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;23;-361.6252,-43.78682;Float;False;Property;_OverallBrightness;OverallBrightness;13;0;Create;True;0;0;False;0;1;1;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-47.55901,-281.549;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;55;-293.7591,359.7623;Float;True;Multiply;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;21;-1354.774,-349.2806;Float;True;6;6;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-301.1263,695.4223;Float;False;Property;_EmissionPower;EmissionPower;12;0;Create;True;0;0;False;0;1;2;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;19;278.7873,-463.7424;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;42.92146,367.7753;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;4;241.7,-180.7062;Float;True;Property;_SAM;SAM;3;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;759.6049,-117.2978;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;PBRMaskTint;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;0;5;1
WireConnection;15;1;6;0
WireConnection;41;0;39;1
WireConnection;41;1;40;0
WireConnection;32;0;5;3
WireConnection;32;1;31;0
WireConnection;16;0;5;2
WireConnection;16;1;13;0
WireConnection;49;0;39;3
WireConnection;49;1;45;0
WireConnection;48;0;39;2
WireConnection;48;1;44;0
WireConnection;50;0;48;0
WireConnection;50;1;46;0
WireConnection;51;0;49;0
WireConnection;51;1;47;0
WireConnection;37;0;16;0
WireConnection;37;1;38;0
WireConnection;34;0;32;0
WireConnection;34;1;33;0
WireConnection;43;0;41;0
WireConnection;43;1;42;0
WireConnection;35;0;15;0
WireConnection;35;1;36;0
WireConnection;18;0;35;0
WireConnection;18;1;37;0
WireConnection;18;2;34;0
WireConnection;18;3;43;0
WireConnection;18;4;50;0
WireConnection;18;5;51;0
WireConnection;20;0;2;0
WireConnection;20;1;18;0
WireConnection;22;0;20;0
WireConnection;22;1;23;0
WireConnection;55;0;53;0
WireConnection;55;1;52;0
WireConnection;21;0;5;1
WireConnection;21;1;5;2
WireConnection;21;2;5;3
WireConnection;21;3;39;1
WireConnection;21;4;39;2
WireConnection;21;5;39;3
WireConnection;19;0;2;0
WireConnection;19;1;22;0
WireConnection;19;2;21;0
WireConnection;56;0;55;0
WireConnection;56;1;54;0
WireConnection;0;0;19;0
WireConnection;0;2;56;0
WireConnection;0;3;4;3
WireConnection;0;4;4;1
WireConnection;0;5;4;2
ASEEND*/
//CHKSM=6CB6C0374C00D4B306B2356135CE2970E0A2B286