Shader "Lineage/URP/water_fountain"
{
    Properties
    {
        [Enum(UnityEngine.Rendering.CullMode)]_CullMode("Cull Mode",int)=2
        _WaterColor01("On the water Color01",color) = (1,1,1,1)
       _WaterColor02("Underwater Color",color) = (1,1,1,1)
        _depth("water_depth",float)=1

        // _value("参数",float)=1
        _BaseMap("BaseMap", 2D) = "white" {}
        _BaseUVspeed("BaseUVSpeed(XY) Distor(Z)",vector)=(0,0,0.3,0)
        _normal("NormalTexure",2D)= "white" {}
        _normalUVspeed("normalUV(xy)",vector)=(0,0,0,0)
        _distor("Distor",range(0,10))=1
        _specular_intensity("Highlight_Intensity",float)=1
        _smooth("smooth",float)=1
        _paomuTex("Foam(泡沫贴图)",2D)="white" {}
        _paomoSpeed("FormSpeed",Vector)=(0,0,0,0)
        _paomo_color("FoamColor",color)=(1,1,1,1)
        _paomobianyuan("FoamEdgeVoid(泡沫边缘虚化)",range(0,1))=0.1
        _paomoRange("FoamRange",range(0,10))=1
        _Speed("Underwater light velocity(焦散速度)",float)=0.1
        [Header(Reflection)]
        _ReflectionTex("SkyBox",Cube)="white" {}
        _ReflecPow("Refraction distortion(折射扭曲)",range(0,1))=0
        _ReflectIntensity("Refraction_Intensity",range(0,5))=1
        [Header(CausticTex)]
        _causticTex("Underwater light map(焦散贴图)",2D)="white" {}
        _CausticRange("UnderwaterlightRange(焦s散范围)",range(0,1))=1
                _waveTex("waveTex(R)dissolve(R)(波浪贴图+溶解)",2D)="white" {}
        _wave_intensity("waveIntensity(波浪大小)",range(0,1))=0.1
        _vertexMoveSpeed("vertexMoveSpeed(xy)  dissolveUVSpeed(ZW)",vector)=(0,0,0,0)
        _dissolve("DissolveRange(溶解)",range(-1,1))=00
        [Enum(Xmask,0,Ymask,1)]_maskCntor("X Y dissolveMaskCntor(遮罩切换)",float)=0

    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" "RenderPipeline" = "UniversalPipeline" }
        LOD 100
        Blend SrcAlpha OneMinusSrcalpha 
        //   ZWrite off
        cull [_CullMode]
        ZWrite off
        Pass
        {
            Name "Unlit"
            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            struct Attributes
            {
                float4 positionOS       : POSITION;
                float4 uv               : TEXCOORD0;
                half3 normalOS:NORMAL;
                half4 color:COLOR;
            };
            struct Varyings
            {
                float4 positionCS       : SV_POSITION;
                float4 uv           : TEXCOORD0;
                half4 color      : TEXCOORD1;
                float3 postionVS: TEXCOORD2;//观察空间下坐标
                float3 positionWS:TEXCOORD3;
                half3 normalWS:TEXCOORD4;
                float4 noamalUV:TEXCOORD5;
                float4 uv2:TEXCOORD6;
            };
            CBUFFER_START(UnityPerMaterial)
            half4 _WaterColor01;
            half _distor,_specular_intensity,_smooth,_paomoRange,_ReflectIntensity;
            half4 _WaterColor02,_paomo_color,_waveTex_ST;
            float4 _BaseMap_ST,_paomuTex_ST,_normal_ST,_causticTex_ST,_vertexMoveSpeed,_BaseUVspeed,_normalUVspeed,_paomoSpeed;
            float _Speed,_ReflecPow,_value,_CausticRange;
            half _paomobianyuan,_wave_intensity,_depth,_dissolve,_maskCntor;
            CBUFFER_END
            TEXTURE2D (_BaseMap);SAMPLER(sampler_BaseMap);
            TEXTURE2D (_normal);SAMPLER(sampler_normal);
            TEXTURE2D(_CameraOpaqueTexture);SAMPLER(sampler_CameraOpaqueTexture);//抓屏
            TEXTURE2D (_CameraDepthTexture);SAMPLER(sampler_CameraDepthTexture);//深度图
            TEXTURE2D (_paomuTex);SAMPLER(sampler_paomuTex);//深度图
            TEXTURECUBE (_ReflectionTex);SAMPLER(sampler_ReflectionTex);//天空
            TEXTURE2D (_causticTex);SAMPLER(sampler_causticTex);//深度图
            TEXTURE2D (_waveTex);SAMPLER(sampler_waveTex);//深度图
            // #define smp _linear_clampU_mirrorV
            // SAMPLER(smp);
            Varyings vert(Attributes v)
            {
                Varyings o = (Varyings)0;
                o.color=v.color;
                float3 positionOS= positionOS.xyz;
              float2 vestTexUV=v.uv.xy*_waveTex_ST.xy+_waveTex_ST.z+(_vertexMoveSpeed.xy*_Time.y);
              float2 vestTexUVG=v.uv.xy;
                //顶点位移
                half4 WaveTex=SAMPLE_TEXTURE2D_LOD(_waveTex, sampler_waveTex,  vestTexUV,0);
                half WaveTexG=SAMPLE_TEXTURE2D_LOD(_waveTex, sampler_waveTex,  vestTexUVG,0).g;
                o.uv2.xy=v.uv.xy;
                v.positionOS.y+=WaveTex.r*WaveTexG*_wave_intensity;
                //  v.positionOS.z+=sin(WaveTex*0.3*(v.positionOS.y));
                // v.positionOS.z+=(sin( ((v.positionOS.x-10)+(v.positionOS.y+112))+(_Time.y*2))*0.1);
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.positionWS=TransformObjectToWorld(v.positionOS.xyz);//本地空间转世界空间
                o.postionVS=TransformWorldToView( o.positionWS);//世界空间转观察空间
                o.normalWS=TransformObjectToWorldNormal(v.normalOS);
                float offset=+_Time.y*_Speed;
                float offset2=+_Time.y*_Speed*2;
                o.uv.xy = o.positionWS.xz*_paomuTex_ST.xy+offset2;
                o.noamalUV.xy=TRANSFORM_TEX(v.uv,_normal)+_normalUVspeed.xy*_Time.y;
                o.noamalUV.zw=TRANSFORM_TEX(v.uv,_normal)+_normalUVspeed.zw*_Time.y;
                o.uv.zw = v.uv.xy *_normal_ST.xy+ _Time.x;
                // o.fogCoord = ComputeFogFactor(o.positionCS.z);
                return o;
            }
            half4 frag(Varyings i) : SV_Target
            {
                float2 dissolveUV=i.uv2.xy*_waveTex_ST.xy+_waveTex_ST.zw+(_vertexMoveSpeed.zw*_Time.y);
                half WaveTex=SAMPLE_TEXTURE2D(_waveTex, sampler_waveTex, dissolveUV).r;
                // half WaveTexb=SAMPLE_TEXTURE2D(_waveTex, sampler_waveTex,  i.uv2.xy).b;
                half4 normalTex0 = SAMPLE_TEXTURE2D(_normal,sampler_normal,i.noamalUV.xy)*2-1;//法线变成-1到1
                half4 normalTex1 = SAMPLE_TEXTURE2D(_normal,sampler_normal,i.noamalUV.xy)*2-1;//法线变成-1到1
                half4 normalTex2 = SAMPLE_TEXTURE2D(_normal,sampler_normal,i.noamalUV.zw)*2-1;//法线变成-1到1
                half4 normalTex=min(normalTex1,normalTex2);
                float2 screenUV=i.positionCS.xy/_ScreenParams.xy;
                float2 distorUV=screenUV+normalTex0.xy*_distor/20;
                //  float2 paopaoUV=screenUV+_Time.x;
                //获取深度图
                half depthTex1=SAMPLE_TEXTURE2D(_CameraDepthTexture,sampler_CameraDepthTexture,distorUV).r;
                half depthScene1=LinearEyeDepth(depthTex1,_ZBufferParams);//转到观察空间下的深度
                half depthWatr1= depthScene1+i.postionVS.z;//比较观察空间Z和片段深度图在观察空间下的z值（深度图）
                //. return depthScene1;
                //获取深度图-------无扭曲-------
                //    half depthTex=SAMPLE_TEXTURE2D(_CameraDepthTexture,sampler_CameraDepthTexture,distorUV).r;
                half depthTex=SAMPLE_TEXTURE2D(_CameraDepthTexture,sampler_CameraDepthTexture,screenUV).r;
                half depthScene=LinearEyeDepth(depthTex,_ZBufferParams);//转到观察空间下的深度
                half depthWatr= depthScene+i.postionVS.z;//比较观察空间Z和片段深度图在观察空间下的z值（深度图）
                //水面扭曲-------------------------------重点---------------------------------------------------------
                // distorUV=lerp(screenUV,distorUV,saturate(depthWatr1));//处理接缝//利用深度图控制扭曲扭曲UV跟屏幕UV混合，处理灵界接缝
                distorUV=lerp(screenUV,distorUV,saturate(depthWatr1)*_BaseUVspeed.z);//处理接缝//利用深度图控制扭曲扭曲UV跟屏幕UV混合，处理灵界接缝
                //这里需要去理解物体背后的深度，更远的深度为负值。
                float2 opaqueUV=distorUV;
                if (depthWatr1<0)opaqueUV=screenUV;//  判断深度大小  <0时用原始的深度图  用原始的UV  大于0使用扭曲的Uv
                //这里需要去理解物体背后的深度，更远的深度为负值。
                half4 c;
                half4 baseMap = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture,opaqueUV);//抓屏
                float2 baseColUV=i.uv2.xy*_BaseMap_ST.xy+_BaseMap_ST.zw+(_BaseUVspeed.xy*_Time.y);
                half4 basecol = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap,baseColUV);//抓屏
               baseMap=lerp(baseMap,basecol,0.3);
        
                //水面高光
                //Speculer= 高光强度×高光颜色*dot((N,H),光滑度)
                Light light=GetMainLight();
                half3 L=light.direction;
                half3 V=normalize(_WorldSpaceCameraPos.xyz-i.positionWS.xyz);
                half3 H=normalize(L+V);//半角向量
                half3 N=i.normalWS;
                N=normalTex;
                half4 specular=_specular_intensity*pow(saturate(dot(N,H)),_smooth);
                // return specular;
                //反射
                half3 reflectionNormal=lerp(normalTex,i.normalWS,_ReflecPow);
                half3 reflectionUV=reflect(-V,reflectionNormal);//模拟反射   利用视线反方向（反射的方向接近视线的反方向）
                half fresnel=pow(1-saturate(dot(i.normalWS,V)),1);
                // return fresnel;
                half4 reflection=SAMPLE_TEXTURECUBE(_ReflectionTex,sampler_ReflectionTex,reflectionUV);
                reflection*=fresnel;
                // return  reflection;
                //焦散
                //原理：1获取深度图Z。 2获取模型观察空间下xyz 3利用相似三角形原理 求出模型观察空间下xyz
                float4 depthVS=1;
                depthVS.xy=i.postionVS.xy*depthScene/-i.postionVS.z;
                depthVS.z=depthScene;
                float3 depthWS=mul(unity_CameraToWorld,depthVS).xyz;//观察空间转到世界空间
                float2 causticUV01=depthWS.xz*_causticTex_ST.xy+depthWS.y*0.05+_Time.y*_Speed*float2(0.2,0.3)*0.1;
                half4 CausticTex01=SAMPLE_TEXTURE2D(_causticTex,sampler_causticTex,causticUV01);
                float2 causticUV02=depthWS.xz*_causticTex_ST.xy+depthWS.y*0.02+_Time.y*_Speed*float2(-0.1,-0.2)*0.4;
                half4 CausticTex02=SAMPLE_TEXTURE2D(_causticTex,sampler_causticTex,causticUV02);
                // return CausticTex01;
                half4  caustic=min(CausticTex01,CausticTex02);
                half CausticRange=depthWatr*_CausticRange;
                caustic*=(1-CausticRange);
                caustic=saturate(caustic);
                // 泡泡
                half paomoRange=depthWatr*_paomoRange;
                //  return caustic;
                 float2 paomoUV=i.uv2.xy*_paomuTex_ST.xy+_paomuTex_ST.zw+_paomoSpeed.xy*_Time.y;
                
                half paomoTex=SAMPLE_TEXTURE2D(_paomuTex,sampler_paomuTex,paomoUV).r;
                half mask=smoothstep(paomoTex,paomoTex+_paomobianyuan,1-paomoRange);
                half4 foam=paomoTex*mask;
                foam*=_paomo_color*3;
                c = baseMap;
                c+=foam;
                c.rgb*=(lerp(_WaterColor02,_WaterColor01,saturate((i.positionWS.y*0.2)+_depth)));
                
                // return c;
                c+=specular;
                c+=reflection*_ReflectIntensity;
                c+=caustic*_ReflectIntensity;
                c=saturate(c);
                //   c+=specular;
                // c.rgb = MixFog(c.rgb, i.fogCoord);
                c*=_WaterColor01;
                float distorMask=lerp(i.uv2.x,i.uv2.y,_maskCntor);


              c.a=smoothstep(_dissolve,_dissolve+0.2,WaveTex+distorMask*0.9);
              c.a*=i.color.a;
            //   return c.a;
            //  c.a=smoothstep(_dissolve+saturate(1-i.positionWS.y*WaveTexg),_dissolve+saturate(1-i.positionWS.y)*WaveTexg+0.1,WaveTex);
            //    c.a=smoothstep((WaveTex*saturate(1-i.positionWS.y+_dissolve)),WaveTex*saturate(1-i.positionWS.y+_dissolve)+0.1,1);


               
                // c.a*=WaveTexg;













                return c;
            }
            ENDHLSL
        }
    }

   
}
