Shader "Custom/SpriteOscillationShader"
{
    Properties
    {
        _Color1 ("Color1", Color) = (1,0,0,1)
        _Color2 ("Color2", Color) = (0,1,0,1)
        _Frequency ("Frequency", Range(0,10)) = 1
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Cutoff ("Alpha Cutoff", Range(0,1)) = .5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

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

            fixed4 _Color1;
            fixed4 _Color2;
            float _Frequency;

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv             : TEXCOORD0;
                float4 vertex         : SV_POSITION;
                fixed4 color          : COLOR;
            };

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.uv = IN.texcoord;
                OUT.color = IN.color;
                return OUT;
            }

            sampler2D _MainTex;
            float _Cutoff;

            fixed4 SampleSpriteTexture(float2 uv) 
            {
                fixed4 c = tex2D(_MainTex, uv);
                c.a *= step(_Cutoff, c.a);
                return c;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = SampleSpriteTexture (IN.uv) * IN.color;
                float t = 0.5 * (1.0 + sin(_Time.y * _Frequency));
                fixed4 color = lerp(_Color1, _Color2, t);
                c.rgb = c.a > 0.5 ? color.rgb : c.rgb;
                return c;
            }
        ENDCG
        }
    }
}
