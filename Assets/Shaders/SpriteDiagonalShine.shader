Shader "ActionRPG/Sprite Diagonal Shine"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        [HideInInspector] _Color ("Tint", Color) = (1, 1, 1, 1)
        [HideInInspector] _RendererColor ("Renderer Color", Color) = (1, 1, 1, 1)
        _ShineColor ("Shine Color", Color) = (1, 1, 1, 1)
        _ShineIntensity ("Shine Intensity", Range(0, 2)) = 0.75
        _ShineWidth ("Shine Width", Range(0.01, 0.5)) = 0.12
        _ShineInterval ("Seconds Between Shines", Range(1, 5)) = 2.5
        _ShineDuration ("Sweep Duration", Range(0.1, 2)) = 0.65
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "IgnoreProjector" = "True"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            Tags { "LightMode" = "Universal2D" }

            HLSLPROGRAM
            #pragma vertex DiagonalShineVertex
            #pragma fragment DiagonalShineFragment

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float3 positionOS : POSITION;
                half4 color : COLOR;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                half4 color : COLOR;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _MainTex_TexelSize;
                half4 _Color;
                half4 _RendererColor;
                half4 _ShineColor;
                half _ShineIntensity;
                half _ShineWidth;
                float _ShineInterval;
                float _ShineDuration;
            CBUFFER_END

            Varyings DiagonalShineVertex(Attributes input)
            {
                Varyings output = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                output.positionCS = TransformObjectToHClip(input.positionOS);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.color = input.color * _Color * _RendererColor;
                return output;
            }

            half4 DiagonalShineFragment(Varyings input) : SV_Target
            {
                half4 sprite = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv) * input.color;

                float elapsed = fmod(_Time.y, max(_ShineInterval, 0.001));
                float sweepProgress = saturate(elapsed / max(_ShineDuration, 0.001));
                half sweepActive = 1.0h - step(_ShineDuration, elapsed);

                // Snap the UV to the texel grid so the shine band's diagonal
                // edge staircases along the art's pixels instead of cutting a
                // smooth sub-pixel line through them.
                float2 texelUV = (floor(input.uv * _MainTex_TexelSize.zw) + 0.5)
                    * _MainTex_TexelSize.xy;

                // uv.x + uv.y produces a bottom-left to top-right diagonal.
                float diagonalPosition = texelUV.x + texelUV.y;
                float sweepCenter = lerp(-_ShineWidth, 2.0 + _ShineWidth, sweepProgress);

                // Linear falloff toward the band edges. Because the diagonal
                // position is texel-snapped, each art pixel gets one discrete
                // brightness level, so the shine fades out without blending
                // inside any pixel and the edges stay crisp.
                half shineBand = saturate(
                    1.0 - abs(diagonalPosition - sweepCenter) / _ShineWidth);

                half shineAmount = shineBand * sweepActive * _ShineIntensity * _ShineColor.a;
                sprite.rgb += _ShineColor.rgb * shineAmount;
                return sprite;
            }
            ENDHLSL
        }
    }

    Fallback "Sprites/Default"
}
