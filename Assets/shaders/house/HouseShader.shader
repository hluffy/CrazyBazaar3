Shader "Custom/HouseShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _ShadowColor ("Shadow Color", Color) = (0.2,0.2,0.2,1)
        _HighlightColor ("Highlight Color", Color) = (1,1,0.8,1)
        _EdgeColor ("Edge Color", Color) = (0,0,0,1)
        _WaveFrequency ("Wave Frequency", Float) = 10
        _WaveAmplitude ("Wave Amplitude", Float) = 0.2
        _HighlightHeight ("Highlight Height", Float) = 1.5
        _ShadowHeight ("Shadow Height", Float) = 1.0
        _EdgeThickness ("Edge Thickness", Float) = 1.0
        _WatercolorTex ("Watercolor Texture", 2D) = "white" {}
        _WatercolorStrength ("Watercolor Strength", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float2 uv : TEXCOORD2;
                float3 viewDir : TEXCOORD3;
            };

            float4 _BaseColor;
            float4 _ShadowColor;
            float4 _HighlightColor;
            float4 _EdgeColor;
            float _WaveFrequency;
            float _WaveAmplitude;
            float _HighlightHeight;
            float _ShadowHeight;
            float _EdgeThickness;
            float _WatercolorStrength;
            sampler2D _WatercolorTex;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                float4 worldPos = TransformObjectToWorld(IN.positionOS); // Transform position to world space (float4)
                OUT.worldPos = worldPos.xyz; // Extract world position as float3
                OUT.worldNormal = TransformObjectToWorldNormal(IN.normalOS); // Transform normal to world space
                OUT.viewDir = normalize(_WorldSpaceCameraPos - worldPos.xyz); // Calculate the view direction
                OUT.positionHCS = TransformWorldToHClip(float4(worldPos.xyz, 1.0f)); // Convert world position to homogeneous clip space
                OUT.uv = IN.uv; // Pass UV for texture sampling
                return OUT;
            }


            float edgeFactor(float3 normal, float3 viewDir)
            {
                return pow(1.0 - saturate(dot(normalize(normal), normalize(viewDir))), _EdgeThickness);
            }

            float4 frag (Varyings IN) : SV_Target
            {
                float4 col = _BaseColor;

                // // 阴影波浪
                // float height = IN.worldPos.y;
                // float shadowWave = _WaveAmplitude * sin(_WaveFrequency * IN.worldPos.x + _Time.y);
                // if (height < _ShadowHeight + shadowWave)
                // {
                //     col = lerp(col, _ShadowColor, 1.0 - saturate((height - shadowWave) / _ShadowHeight));
                // }

                // // 顶部高光
                // if (height > _HighlightHeight)
                // {
                //     col = lerp(col, _HighlightColor, saturate((height - _HighlightHeight) / 0.5));
                // }

                // // 轮廓边缘色（面向摄像机）
                // float edge = edgeFactor(IN.worldNormal, IN.viewDir);
                // col = lerp(col, _EdgeColor, edge);

                // // 水彩纹理叠加
                // float4 watercolor = tex2D(_WatercolorTex, IN.uv);
                // col.rgb = lerp(col.rgb, col.rgb * watercolor.rgb, _WatercolorStrength * watercolor.a);

                return col;
            }
            ENDHLSL
        }
    }
}
