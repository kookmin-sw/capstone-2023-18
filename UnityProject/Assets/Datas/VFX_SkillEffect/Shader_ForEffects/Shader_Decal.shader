Shader "GAPH Custom Shader/Shader_Decal"{
		Properties{
			[Header(Main Settings)]
				[Space]
					[HDR]_TintColor("TintColor",Color) = (1,1,1,1)
					[Toggle(IS_USE_SECOND_COLOR)]_SecondColor("Is use second color",int) = 0
					[HDR]_TintColor2("Color2",Color) = (1,1,1,1)
					_MainTex("MainTexture",2D) = "white" {}
					_ColorFactor("Color Factor", float) = 1
					[Toggle(IS_TEXTURE_ANIMATE)]_TextureAnimate("Is Texture Animate",int) = 0
						_TextureAnimateSpeed("Texture Animate Speed",float) = 1.0
					[Toggle(IS_NORMAL_DISTORTION)]_NormalDistortion("Is Normal Distortion",int) = 0
						_NormalTex("Normal Tex",2D) = "white"{}
					_NormalDistortionFactor("Normal Distortion Factor",float) = 1.0
						[Toggle(IS_NORMAL_ANIMATE)]_NormalAnimate("Is Normal Animate",int) = 0
						_NormalAnimateSpeed("Normal Animate Speed",float) = 1.0
					[Toggle(IS_MASK_FADE)]_MaskFade("Is Mask Fade",int) = 0
						_MaskTex("Mask Tex",2D) = "white"{}
					_MaskCutOut("Mask CutOut",Range(0,1)) = 1
			[Header(Render)]
				[Space]
					[Toggle]_ZWrite("ZWrite On/Off", int) = 0
					[Enum(Culling Off,0, Culling Front, 1, Culling Back, 2)]_Culling("Culling",float) = 2
					[Enum(UnityEngine.Rendering.BlendMode)]_BlendSrc("BlendSrc", float) = 1
					[Enum(UnityEngine.Rendering.BlendMode)]_BlendDst("BlendDst", float) = 1
					_ZTest1("_ZTest1", int) = 5
		}

		Category{
			Tags{ "Queue" = "Geometry"  "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			Blend[_BlendSrc][_BlendDst]
			Cull Front
			ZTest[_ZTest1]
			ZWrite Off
			Stencil {
						Ref 1
						Comp notequal
			}


			SubShader{
				Pass{
						CGPROGRAM
						#pragma vertex vert
						#pragma fragment frag
						#pragma multi_compile_instancing

						#pragma shader_feature IS_MASK_FADE
						#pragma shader_feature IS_USE_SECOND_COLOR
						#pragma shader_feature IS_TEXTURE_ANIMATE
						#pragma shader_feature IS_NORMAL_ANIMATE
						#pragma shader_feature IS_NORMAL_DISTORTION

						#include "UnityCG.cginc"

						sampler2D _MainTex;
						sampler2D _NormalTex;
						sampler2D _MaskTex;

						sampler2D _CameraDepthTexture;

						float4 _MainTex_ST;
						float4 _NormalTex_ST;
						float4 _MaskTex_ST;
						float4 _MainTex_NextFrame;
						float4 _DepthPyramidScale;
						float _MaskCutOut;

						UNITY_INSTANCING_BUFFER_START(data)
							UNITY_DEFINE_INSTANCED_PROP(float4x4, _InverseTransformMatrix)
						#define _InverseTransformMatrix_arr data
							UNITY_DEFINE_INSTANCED_PROP(half4, _TintColor)
						#define _TintColor_arr data
						#ifdef IS_USE_SECOND_COLOR
								UNITY_DEFINE_INSTANCED_PROP(half4, _TintColor2)
							#define _TintColor2_arr data
						#endif
							UNITY_DEFINE_INSTANCED_PROP(half, _NormalAnimateSpeed)
						#define _NormalAnimateSpeed_arr data
							UNITY_DEFINE_INSTANCED_PROP(half, _TextureAnimateSpeed)
						#define _TextureAnimateSpeed_arr data
							UNITY_DEFINE_INSTANCED_PROP(half, _NormalDistortionFactor)
						#define _NormalDistortionFactor_arr data
							UNITY_DEFINE_INSTANCED_PROP(half, _ColorFactor)
						#define _ColorFactor_arr data
						UNITY_INSTANCING_BUFFER_END(data)

						struct appdata_t {
							float4 vertex : POSITION;
							float4 normal : NORMAL;
							half4 color : COLOR;
							float2 texcoord : TEXCOORD0;
							UNITY_VERTEX_INPUT_INSTANCE_ID
						};

						struct v2f {
							float4 vertex : SV_POSITION;
							half4 color : COLOR;
							float4 screenUV : TEXCOORD0;
							float3 ray : TEXCOODR1;
							float2 texcoord : TEXCOORD2;
							float2 texcoord2 : TEXCOORD3;
							float2 texcoord3 : TEXCOORD4;
							float3 normal : TEXCOORD5;
							UNITY_FOG_COORDS(7)
							UNITY_VERTEX_INPUT_INSTANCE_ID
							UNITY_VERTEX_OUTPUT_STEREO
						};

						v2f vert(appdata_t v)
						{
							v2f o;
							UNITY_INITIALIZE_OUTPUT(v2f, o);
							UNITY_SETUP_INSTANCE_ID(v);
							UNITY_TRANSFER_INSTANCE_ID(v, o);
							UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

							o.vertex = UnityObjectToClipPos(v.vertex);
							o.color = v.color;

							o.ray = UnityObjectToViewPos(v.vertex) * float3(-1, -1, 1);
							o.screenUV = ComputeScreenPos(o.vertex);
							o.normal = UnityObjectToWorldNormal(o.vertex);
							//o.screenUV.xy *= _DepthPyramidScale.xy;

							UNITY_TRANSFER_FOG(o, o.vertex);
							return o;
						}

						half4 frag(v2f i) : SV_Target
						{
							UNITY_SETUP_INSTANCE_ID(i);
							i.ray = i.ray* (_ProjectionParams.z / i.ray.z);
							float2 screenUV = i.screenUV.xy / i.screenUV.w;

							float depth = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture,screenUV));
							float4 vpos = float4(i.ray * depth, 1);
							float3 wpos = mul(unity_CameraToWorld, vpos).xyz;
							float3 opos = mul(unity_WorldToObject, float4(wpos,1)).xyz;
							float3 objAbs = abs(opos);
							clip(0.5f - objAbs);

							i.texcoord = saturate(opos.xz + 0.5) * _MainTex_ST.xy + _MainTex_ST.zw;
							i.texcoord2 = saturate(opos.xz + 0.5) * _NormalTex_ST.xy + _NormalTex_ST.zw;
							i.texcoord3 = saturate(opos.xz + 0.5) * _MaskTex_ST.xy + _MaskTex_ST.zw;


							#ifdef IS_NORMAL_DISTORTION
							half2 distort = UnpackNormal(tex2D(_NormalTex, i.texcoord2 * 1.0f + (UNITY_ACCESS_INSTANCED_PROP(_NormalAnimateSpeed_arr, _NormalAnimateSpeed) * _Time / 20))).rg;
							distort += UnpackNormal(tex2D(_NormalTex, i.texcoord2 * 1.0f - (UNITY_ACCESS_INSTANCED_PROP(_NormalAnimateSpeed_arr, _NormalAnimateSpeed)*_Time / 20) + float2(0.5f, 0.15f))).rg;
							distort += UnpackNormal(tex2D(_NormalTex, i.texcoord2 * 0.5f - (UNITY_ACCESS_INSTANCED_PROP(_NormalAnimateSpeed_arr, _NormalAnimateSpeed)*_Time / 10) + float2(0.15f, 0.5f))).rg;
							distort += UnpackNormal(tex2D(_NormalTex, i.texcoord2 * 0.5f - (UNITY_ACCESS_INSTANCED_PROP(_NormalAnimateSpeed_arr, _NormalAnimateSpeed)*_Time / 10) + float2(-0.5f, -0.15f))).rg;
							i.texcoord.xy += distort.xy * UNITY_ACCESS_INSTANCED_PROP(_NormalDistortionFactor_arr, _NormalDistortionFactor);
							#endif

							#ifdef IS_TEXTURE_ANIMATE
							half4 tex = tex2D(_MainTex, i.texcoord.xy - (UNITY_ACCESS_INSTANCED_PROP(_TextureAnimateSpeed_arr, _TextureAnimateSpeed) * _Time / 10));
							tex *= tex2D(_MainTex, i.texcoord.xy - (UNITY_ACCESS_INSTANCED_PROP(_TextureAnimateSpeed_arr, _TextureAnimateSpeed) * _Time / 10) + float2(0.25f, -0.25f));
							half4 tex2 = tex2D(_MainTex, i.texcoord.xy + (UNITY_ACCESS_INSTANCED_PROP(_TextureAnimateSpeed_arr, _TextureAnimateSpeed) * _Time / 10));
							tex2 *= tex2D(_MainTex, i.texcoord.xy + (UNITY_ACCESS_INSTANCED_PROP(_TextureAnimateSpeed_arr, _TextureAnimateSpeed) * _Time / 10) + float2(0.15f, -0.15f));
							tex = (tex + tex2) / 1.5f;
							#else
							half4 tex = tex2D(_MainTex, i.texcoord);
							#endif

							#ifdef IS_MASK_FADE
							half mask_a =pow( saturate(tex2D(_MaskTex, i.texcoord3) - (1-_MaskCutOut)),1.5f);
							#else
							half mask_a = tex.a * _MaskCutOut;
							#endif
							#ifdef IS_USE_SECOND_COLOR
								half4 res = tex * UNITY_ACCESS_INSTANCED_PROP(_TintColor2_arr, _TintColor2) *  UNITY_ACCESS_INSTANCED_PROP(_ColorFactor_arr, _ColorFactor);
								half alpha = res.a * i.color.a * mask_a *  UNITY_ACCESS_INSTANCED_PROP(_ColorFactor_arr, _ColorFactor)  * UNITY_ACCESS_INSTANCED_PROP(_TintColor2_arr, _TintColor2).a;
							#else			
								half4 res = tex * UNITY_ACCESS_INSTANCED_PROP(_TintColor_arr, _TintColor) *  UNITY_ACCESS_INSTANCED_PROP(_ColorFactor_arr, _ColorFactor);
								half alpha = res.a * i.color.a * mask_a *  UNITY_ACCESS_INSTANCED_PROP(_ColorFactor_arr, _ColorFactor)  * UNITY_ACCESS_INSTANCED_PROP(_TintColor_arr, _TintColor).a;
							#endif
								res.a = alpha;

							if (dot(vpos, i.vertex) != 0)
								return res;
							else
								return 0;
						}
					ENDCG
				}
			}
		}
}