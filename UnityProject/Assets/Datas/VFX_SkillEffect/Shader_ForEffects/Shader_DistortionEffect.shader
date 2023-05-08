Shader "GAPH Custom Shader/Distortion Effect" {
	Properties {
		_TintColor ("Tint Color", Color) = (1,1,1,1)
		_Mask ("Mask",2D) = "black"{}
		_NormalMap ("Normalmap", 2D) = "bump" {}
		_DistortFactor ("Distortion", Float) = 10
		_InvFade ("Soft Particles Factor", Range(0,10)) = 1.0
	}

	SubShader{
		GrabPass{
			"_GrabTexture"
			}

			Tags{ "Queue" = "Transparent"  "IgnoreProjector" = "True"  "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
			Lighting Off
			ZWrite Off

			Pass{

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma multi_compile_particles
				#include "UnityCG.cginc"

				struct appdata_t {
					float4 vertex : POSITION;
					float2 texcoord: TEXCOORD0;
					fixed4 color : COLOR;
				};

				struct v2f {
					float4 vertex : POSITION;
					float4 uvgrab : TEXCOORD0;
					float2 uvnormal : TEXCOORD1;
					float2 uvmask : TEXCOORD2;
					fixed4 color : COLOR;
					#ifdef SOFTPARTICLES_ON
						float4 projPos : TEXCOORD3;
					#endif
				};

				fixed4 _TintColor;

				sampler2D _Mask;
				sampler2D _NormalMap;
				sampler2D _GrabTexture;

				float _DistortFactor;
				float _ColorFactor;			

				float4 _NormalMap_ST;
				float4 _Mask_ST;
				float4 _GrabTexture_TexelSize;

				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);

					#ifdef SOFTPARTICLES_ON
						o.projPos = ComputeScreenPos (o.vertex);
						COMPUTE_EYEDEPTH(o.projPos.z);
					#endif
					o.color = v.color;

					#if UNITY_UV_STARTS_AT_TOP
						float scale = -1.0;
					#else
						float scale = 1.0;
					#endif

					//Set uvgrab value 
					o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y * scale) + o.vertex.w) * 0.5;
					o.uvgrab.zw = o.vertex.zw;

					o.uvnormal = TRANSFORM_TEX( v.texcoord, _NormalMap );
					o.uvmask = TRANSFORM_TEX(v.texcoord,_Mask);
					UNITY_TRANSFER_FOG(o, o.vertex);
					return o;
				}

				sampler2D _CameraDepthTexture;
				float _InvFade;

				half4 frag( v2f i ) : COLOR
				{
					#ifdef SOFTPARTICLES_ON
							float sceneZ = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
							float partZ = i.projPos.z;
							float fade = saturate(_InvFade * (sceneZ - partZ));
							i.color.a *= fade;
						#endif

							//Set normal tex
							half2 normal = UnpackNormal(tex2D(_NormalMap, i.uvnormal)).rg;
							//Set distort factor using normal, GrabTexture
							half2 distortValue = normal * _DistortFactor * _GrabTexture_TexelSize.xy;
							//Amplify distort offset factor if graphic API is DX11 or METAL.
							//OpenGLCore or OpenGL don't need this
							#if defined(SHADER_API_D3D11) || defined(SHADER_API_METAL) 
							distortValue *= 10;
							#endif
							//Add new distort data to original grab value
							i.uvgrab.xy = (distortValue * i.uvgrab.z) + i.uvgrab.xy;

							//Sample GrabTexture using updated grab value
							half4 distort = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
							half4 mask = tex2D(_Mask, i.uvmask);

							half4 res = distort;
							//Compose all
							res.a = _TintColor.a * i.color.a * mask.a;
							UNITY_APPLY_FOG(i.fogCoord, res);
							return res;
				}
			ENDCG
		}
	}
}