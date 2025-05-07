Shader "Custom/PlayerOutlineShader"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineWidth ("Outline Width", Range(0.0, 0.05)) = 0.005 // 기본값 줄임
    }

    SubShader
    {
        Tags { "Queue"="Transparent" }
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _OutlineColor;
            float _OutlineWidth;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv; // UV 좌표 변경
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // 투명한 픽셀 제외 (원본 텍스처 유지)
                if (texColor.a < 0.1)
                    discard;

                // 주변 픽셀 샘플링 (상, 하, 좌, 우)
                float2 offset = float2(_OutlineWidth, _OutlineWidth);
                fixed4 texUp    = tex2D(_MainTex, i.uv + float2(0, offset.y));
                fixed4 texDown  = tex2D(_MainTex, i.uv - float2(0, offset.y));
                fixed4 texLeft  = tex2D(_MainTex, i.uv - float2(offset.x, 0));
                fixed4 texRight = tex2D(_MainTex, i.uv + float2(offset.x, 0));

                // 경계 감지 (주변 픽셀 중 투명한 영역이 있으면 아웃라인 생성)
                float edgeFactor = (1 - texUp.a) + (1 - texDown.a) + (1 - texLeft.a) + (1 - texRight.a);
                fixed4 outline = _OutlineColor * saturate(edgeFactor);

                fixed4 finalColor = texColor;  // 원본 색상 유지
                finalColor.rgb = texColor.rgb + outline.rgb * outline.a;

                return finalColor; // 원본 텍스처 + 경계를 활용한 아웃라인
            }
            ENDCG
        }
    }
}
