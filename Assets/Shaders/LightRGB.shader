Shader "Custom/LightRGB_TopBottomCenter"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Aperture ("Beam Aperture", Range(0,1)) = 0.25
        _CenterSize ("Center Size", Range(0,0.5)) = 0.1
        _FadeStart ("Fade Start (0-0.5)", Range(0,0.5)) = 0.35
        _FadeEndAlpha ("Fade End Alpha", Range(0,1)) = 0.0
        _StenccilRef ("StenccilRef", Range(0,7)) = 1

    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "PreviewType"="Sprite" "CanUseSpriteAtlas"="True" }

        Cull Off
        ZWrite Off
        Blend One One

        Pass
        {
            Stencil
            {
                Ref [_StenccilRef]
                Comp Always
                Pass Replace
            }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float _Aperture;
            float _CenterSize;
            float _FadeStart;
            float _FadeEndAlpha;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 topCenter = float2(0.5, 0.5 + _CenterSize);
                float2 bottomCenter = float2(0.5, 0.5 - _CenterSize);

                float t = saturate((i.uv.y - (0.5 - _CenterSize)) / (2.0 * _CenterSize));
                float2 origin = lerp(bottomCenter, topCenter, t);

                float2 v = i.uv - origin;
                float r = length(v);
                float angle = atan2(v.y, v.x);

                float centerAngle = 0.0;
                float delta = abs(atan2(sin(angle - centerAngle), cos(angle - centerAngle)));

                if (i.uv.x >= 0.2 && i.uv.x <= 0.2 + _CenterSize &&
                    i.uv.y >= 0.2 - _CenterSize && i.uv.y <= 0.2 + _CenterSize)
                {
                    discard;
                }
                else
                {
                    if (delta > _Aperture * UNITY_PI)
                        discard;
                }

                float alpha = 1.0;
                if (r > _FadeStart)
                {
                    float tFade = saturate((r - _FadeStart) / (0.5 - _FadeStart));
                    alpha = lerp(1.0, _FadeEndAlpha, tFade);
                }

                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                col *= alpha;

                return col;
            }

            ENDCG
        }
    }
}
