
Shader "Perchang/PeopleOcclusion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CameraFeed ("Texture", 2D) = "white" {}
        _OcclusionDepth ("Texture", 2D) = "white" {}
        _OcclusionStencil ("Texture", 2D) = "white" {}
        _UVMultiplierLandScape ("UV MultiplerLandScape", Float) = 0.0
        _UVMultiplierPortrait ("UV MultiplerPortrait", Float) = 0.0
        _UVFlip ("Flip UV", Float) = 0.0
        _ONWIDE("Onwide", Int) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D_float _OcclusionDepth;
            sampler2D _OcclusionStencil;
            sampler2D _CameraFeed;
            sampler2D_float _CameraDepthTexture;
            float _UVMultiplierLandScape;
            float _UVMultiplierPortrait;
            float _UVFlip;
            int _ONWIDE;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                if(_ONWIDE == 1)
                {
                    o.uv1 = float2(v.uv.x, (1.0 - (_UVMultiplierLandScape * 0.5f)) + (v.uv.y / _UVMultiplierLandScape));
                    o.uv2 = float2(lerp(1.0 - o.uv1.x, o.uv1.x, _UVFlip), lerp(o.uv1.y, 1.0 - o.uv1.y, _UVFlip));
                }
                else
                {
                    o.uv1 = float2((1.0 - (_UVMultiplierPortrait * 0.5f)) + (v.uv.x / _UVMultiplierPortrait), v.uv.y);
                    o.uv2 = float2(lerp(1.0 - o.uv1.y, o.uv1.y, 0), lerp(o.uv1.x, 1.0 - o.uv1.x, 1));
                }
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 cameraFeedCol = tex2D(_CameraFeed, i.uv1 * _Time);
                float sceneDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv));
                float4 stencilCol = tex2D(_OcclusionStencil, i.uv2);
                float occlusionDepth = tex2D(_OcclusionDepth, i.uv2) * 0.625; //0.625 hack occlusion depth based on real world observation
                            
                float showOccluder = step(occlusionDepth, sceneDepth) * stencilCol.r; // 1 if (depth >= ocluderDepth && stencil)

                return lerp(col, cameraFeedCol, showOccluder);
            }

            ENDCG
        }
    }
}