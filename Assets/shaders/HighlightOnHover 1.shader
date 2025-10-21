Shader "Custom/HighlightOnHover1"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineSize ("Outline Size", Float) = 1.0
        _IsSelect("IsSelect", Float) = 0
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        LOD 100
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _OutlineColor;
            float _OutlineSize,_IsSelect;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                if(_IsSelect==0){
                    return tex2D(_MainTex, uv);
                }
                float alpha = tex2D(_MainTex, uv).a;

                float outline = 0.0;
                float2 offset = _OutlineSize / _ScreenParams.xy;

                // 周围8个方向采样 Alpha
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (x == 0 && y == 0) continue;

                        float2 sampleUV = uv + float2(x, y) * offset;
                        outline += tex2D(_MainTex, sampleUV).a;
                    }
                }

                fixed4 col = tex2D(_MainTex, uv);
                if (alpha == 0 && outline > 0)
                {
                    return _OutlineColor;
                }

                return col;
            }
            ENDCG
        }
    }
}
