Shader "ActionRPG/Sprite Pulse Outline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        [HideInInspector] _Color ("Tint", Color) = (1, 1, 1, 1)
        [HideInInspector] _RendererColor ("Renderer Color", Color) = (1, 1, 1, 1)
        _OutlineColor ("Outline Color", Color) = (1, 0.85, 0.3, 1)
        _OutlineThickness ("Outline Thickness (Pixels)", Range(1, 4)) = 1
        _AlphaCutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
        _PulseSpeed ("Pulse Speed", Range(0, 10)) = 2
        _PulseMinBrightness ("Pulse Min Brightness", Range(0, 1)) = 0.6
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
            #pragma vertex PulseOutlineVertex
            #pragma fragment PulseOutlineFragment

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
                half4 _OutlineColor;
                float _OutlineThickness;
                half _AlphaCutoff;
                float _PulseSpeed;
                half _PulseMinBrightness;
            CBUFFER_END

            Varyings PulseOutlineVertex(Attributes input)
            {
                Varyings output = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                output.positionCS = TransformObjectToHClip(input.positionOS);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.color = input.color * _Color * _RendererColor;
                return output;
            }

            half SampleAlpha(float2 uv)
            {
                return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv).a;
            }

            half4 PulseOutlineFragment(Varyings input) : SV_Target
            {
                half4 sprite = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv) * input.color;

                // Opaque sprite pixels render unchanged; the outline only fills
                // transparent pixels that border the sprite.
                if (sprite.a >= _AlphaCutoff)
                {
                    return sprite;
                }

                // Offset in texels so the outline is a consistent pixel width
                // regardless of sprite dimensions.
                float2 offset = _MainTex_TexelSize.xy * _OutlineThickness;

                half neighborAlpha = SampleAlpha(input.uv + float2(offset.x, 0));
                neighborAlpha = max(neighborAlpha, SampleAlpha(input.uv - float2(offset.x, 0)));
                neighborAlpha = max(neighborAlpha, SampleAlpha(input.uv + float2(0, offset.y)));
                neighborAlpha = max(neighborAlpha, SampleAlpha(input.uv - float2(0, offset.y)));

                // Hard cutoff keeps the outline edge crisp; no smoothing.
                half isOutline = step(_AlphaCutoff, neighborAlpha);

                // Pulse dims the outline color toward a minimum brightness and
                // back. Brightness-only modulation keeps the silhouette stable.
                half pulse = lerp(_PulseMinBrightness, 1.0h,
                    0.5h + 0.5h * sin(_Time.y * _PulseSpeed));

                half4 outline;
                outline.rgb = _OutlineColor.rgb * pulse;
                outline.a = _OutlineColor.a * isOutline;
                return outline;
            }
            ENDHLSL
        }
    }

    Fallback "Sprites/Default"
}
