Shader "Custom/MoreTilemapTexture"
{
    Properties
    {
        _MainTex ("Tilemap Color", 2D) = "white" {}
        _Texture1 ("Grass Texture", 2D) = "white" {}
        _Texture2 ("Water Texture", 2D) = "white" {}
        _Texture3 ("Snow Texture", 2D) = "white" {}
        _Texture4 ("Stone Texture", 2D) = "white" {}
    
        _ColorThreshold ("Color Match Threshold", Range(0, 0.5)) = 0.1
        _TextureAlpha ("Texture Opacity", Range(0, 1)) = 0.5 // 默认透明度 50%
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha // 启用 Alpha 混合

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
                float4 vertex : SV_POSITION;
                float2 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _Texture1, _Texture2, _Texture3, _Texture4;
            float _ColorThreshold;
            float _TextureAlpha; // 控制纹理透明度

            // 预定义 5 种颜色（RGB 格式）
            static const float3 COLOR_PINK = float3(1.0, 0.73, 0.73);    // #FFBABA
            static const float3 COLOR_BLUE = float3(0.73, 0.76, 1.0);    // #BAC1FF
            static const float3 COLOR_CYAN = float3(0.73, 0.95, 1.0);    // #BAF3FF
            static const float3 COLOR_GREEN = float3(0.73, 1.0, 0.80);   // #BAFFCD
            static const float3 COLOR_YELLOW = float3(1.0, 0.99, 0.73);  // #FFFDBA

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                

                o.uv = v.uv;
                return o;
            }
            //  v2f vert(appdata_t v)
            // {
            //     v2f o;
            //     o.vertex = UnityObjectToClipPos(v.vertex);
                
            //     // Calculate the world position (in 2D space, so xy)
            //     float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
            //     o.worldPos = worldPos.xzy;
                
            //     o.uv = v.texcoord;
            //     return o;
            // }

            // 计算颜色距离（平方距离优化性能）
            float ColorDistance(float3 a, float3 b)
            {
                float3 diff = a - b;
                return dot(diff, diff);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 tileColor = tex2D(_MainTex, i.uv);
                float3 tileRGB = tileColor.rgb;

                // 计算与 5 种颜色的距离
                float distPink = ColorDistance(tileRGB, COLOR_PINK);
                float distBlue = ColorDistance(tileRGB, COLOR_BLUE);
                float distCyan = ColorDistance(tileRGB, COLOR_CYAN);
                float distGreen = ColorDistance(tileRGB, COLOR_GREEN);
                float distYellow = ColorDistance(tileRGB, COLOR_YELLOW);

                // 找出最小距离
                float minDist = min(min(min(min(distPink, distBlue), distCyan), distGreen), distYellow);

                // 默认使用 Tile 原始颜色
                fixed4 finalColor = tileColor;

                // 如果匹配到颜色，则混合纹理
                if (minDist < _ColorThreshold)
                {
                    fixed4 textureColor = tileColor; // 默认 fallback
                    if (minDist == distPink) textureColor = tex2D(_Texture1, i.uv);
                    else if (minDist == distBlue) textureColor = tex2D(_Texture2, i.uv);
                    else if (minDist == distCyan) textureColor = tex2D(_Texture3, i.uv);
                    else if (minDist == distGreen) textureColor = tex2D(_Texture4, i.uv);
                 
                    // float2 overlayUV = i.worldPos * 0.2;
                    // fixed4 overlayColor = tex2D(textureColor, overlayUV);


                    // 使用 lerp 混合基础颜色和纹理（透明度由 _TextureAlpha 控制）
                    finalColor = lerp(tileColor, textureColor, _TextureAlpha);
                    finalColor.a = 1.0; // 确保最终不透明（或保留 textureColor.a）
                }

                return finalColor;
            }
            ENDCG
        }
    }
}