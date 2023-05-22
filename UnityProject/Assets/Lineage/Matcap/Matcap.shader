Shader "Lineage/URP/Matcup"
{
    Properties
    {
        _BaseColor("Base Color",color) = (1,1,1,1)
        _Matcap("matcap", 2D) = "black" {}
        _NormalMap  ("法线贴图", 2D) = "bump" {}
        _NormalScale("法綫强度",range(0,3))=1
        _bacemap("bacemap", 2D) = "white" {}




    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType" = "Opaque" "IgnoreProjector" = "True" "RenderPipeline" = "UniversalPipeline" }
        LOD 100

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
                float2 uv               : TEXCOORD0;
                float3 normalOS   : NORMAL;       // 法线信息
                float4 tangent  : TANGENT;      // 切线信息



            };

            struct Varyings
            {
                float4 positionCS       : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float3 nDirWS : TEXCOORD2;      // 世界法线方向
                float3 tDirWS : TEXCOORD3;      // 世界切线方向
                float3 bDirWS : TEXCOORD4;      // 世界副切线方向

            };

            CBUFFER_START(UnityPerMaterial)
            half4 _BaseColor;
            float4 _Matcap_ST,_NormalMap_ST;
            half _NormalScale;
            CBUFFER_END
            TEXTURE2D (_Matcap);SAMPLER(sampler_Matcap);
            TEXTURE2D (_NormalMap);SAMPLER(sampler_NormalMap);
            TEXTURE2D (_bacemap);SAMPLER(sampler_bacemap);


            

            
            
            Varyings vert(Attributes v)
            {
                Varyings o = (Varyings)0;
                o.uv = v.uv;

                o.nDirWS=TransformObjectToWorldNormal(v.normalOS);

                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.tDirWS = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz); // 切线方向 OS>WS
                o.bDirWS = normalize(cross(o.nDirWS, o.tDirWS) * v.tangent.w);  // 根据nDir tDir求bDir



                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float2 normalUV=i.uv*_NormalMap_ST.xy+_NormalMap_ST.zw;
                float3 nDirTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_NormalMap,sampler_NormalMap,normalUV),_NormalScale).rgb;//法线贴图采样解码
                float3x3 TBN = float3x3(i.tDirWS, i.bDirWS, i.nDirWS);
                float3 nDirWS = normalize(mul(nDirTS, TBN));      
                float3 nDirVS = mul(UNITY_MATRIX_V, nDirWS);        // 计算MatcapUV
                float2 matcapUV = nDirVS.rg * 0.5 + 0.5;






                




                half4 c;
                half4 baseMap = SAMPLE_TEXTURE2D(_bacemap, sampler_bacemap, i.uv);

                half4 matcap = SAMPLE_TEXTURE2D(_Matcap, sampler_Matcap, matcapUV);
                c = matcap * _BaseColor;
                return c;
            }
            ENDHLSL
        }
    }


}
