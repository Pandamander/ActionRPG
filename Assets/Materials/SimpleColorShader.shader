Shader "Custom/SpriteOutline"
{
    Properties
    {
        _Color ("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineThickness ("Outline Thickness", Range(0, 0.1)) = 0.02
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
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
            float _OutlineThickness;
            sampler2D _MainTex;
            float _Cutoff;

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
                float2 step = _OutlineThickness * float2(1, 1);
                fixed4 c = tex2D(_MainTex, IN.uv);
                fixed4 outlineColor = _Color;

                if(c.a < _Cutoff)
                {
                    fixed4 left = tex2D(_MainTex, IN.uv - step.x * float2(1, 0));
                    fixed4 right = tex2D(_MainTex, IN.uv + step.x * float2(1, 0));
                    fixed4 down = tex2D(_MainTex, IN.uv - step.y * float2(0, 1));
                    fixed4 up = tex2D(_MainTex, IN.uv + step.y * float2(0, 1));

                    if(left.a > _Cutoff || right.a > _Cutoff || up.a > _Cutoff || down.a > _Cutoff)
                    {
                        c = outlineColor;
                    }
                }

                return c;
            }
        ENDCG
        }
    }
}
