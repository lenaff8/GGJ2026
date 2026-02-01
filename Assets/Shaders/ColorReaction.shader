Shader "Custom/ColorReaction"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _LightMap ("Light RT", 2D) = "black" {}
        _RequiredColor ("Required Color", Color) = (1,0,0,1) 
        _Threshold ("Threshold", Range(0,1)) = 0.1
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
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
                float4 worldPos : TEXCOORD1;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            sampler2D _LightMap;
            float _Threshold;
            fixed4 _RequiredColor;
            float4 _CameraMin;
            float4 _CameraSize;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 baseCol = tex2D(_MainTex, i.uv) * i.color;

                float2 worldUV = (i.worldPos.xy - _CameraMin.xy) / _CameraSize.xy;

                if (worldUV.x < 0 || worldUV.x > 1 || worldUV.y < 0 || worldUV.y > 1)
                    discard;

                fixed4 lightCol = tex2D(_LightMap, worldUV);

                bool matchR = (_RequiredColor.r > 0) ? (lightCol.r > _Threshold) : (lightCol.r <= _Threshold);
                bool matchG = (_RequiredColor.g > 0) ? (lightCol.g > _Threshold) : (lightCol.g <= _Threshold);
                bool matchB = (_RequiredColor.b > 0) ? (lightCol.b > _Threshold) : (lightCol.b <= _Threshold);

                if (!(matchR && matchG && matchB))
                    discard;

                 return baseCol;
            }

            ENDCG
        }
    }
}
