Shader "Custom/PoisonedShader"
{
    Properties
    {
        _Color ("Color", Color) = (0.5, 0, 0.5, 1)
        _WaveAmplitude ("Wave Amplitude", Range(0, 1)) = 0.1
        _WaveFrequency ("Wave Frequency", Range(0, 10)) = 1
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True"}
        
        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            fixed4 _Color;
            float _WaveAmplitude;
            float _WaveFrequency;
            sampler2D _MainTex;

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv             : TEXCOORD0;
                float4 vertex         : SV_POSITION;
            };

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.uv = IN.texcoord;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float2 uv = IN.uv;
                float offset = _WaveAmplitude * sin(_Time.y * _WaveFrequency + uv.x * 2.0 * 3.14159);
                uv.y = clamp(uv.y + offset, 0, 1);
                fixed4 c = tex2D(_MainTex, uv);
                c.rgb = lerp(c.rgb, _Color.rgb, _Color.a * c.a);
                return c;
            }
        ENDCG
        }
    }
}
