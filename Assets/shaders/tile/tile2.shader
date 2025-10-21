Shader "Custom/Mytile2"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise", 2D) = "gray" {} 
        
        _Scale("Scale", Float) = 0.25 // Scale factor

        _GrassTex("Grass Texture", 2D) = "white" {} 
        _StoneTex("Stone Texture", 2D) = "white" {} 
        _SeaTex("Sea Texture", 2D) = "white" {} 
        _MapleTex("Maple Texture", 2D) = "white" {} 
  
 
    }
    
    SubShader
    {
        Tags { 
            "Queue"="Transparent" 
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
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
            // #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            // 地皮类型和优先级（与之前相同）
            #define TURF_EMPTY -1
            #define TURF_GRASS 0
            #define TURF_STONE 1
            #define TURF_OCEAN 2
            #define TURF_MAPLE 3

            sampler2D _MainTex, _NoiseTex, _MapleTex,_GrassTex,_StoneTex,_SeaTex;
            float  _Scale;

            
            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0;  fixed4 color : COLOR;};
            struct v2f { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; float2 worldPos : TEXCOORD1;fixed4 color : COLOR;};
           
            v2f vert (appdata v) { 
                 v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                // o.worldPos = worldPos.xy;
                o.worldPos = worldPos.xyz;
                o.uv = v.uv;
                o.color = v.color; // 传递到片元着色器。tile里面默认的 Color属性
                return o;
             }
            
         
              // 地皮优先级 (数值越高优先级越高)
            int GetTurfPriority(int turfType) {
                 const int priorities[4] = { 
                    1, // 草地
                    3, // 石头
                    2, // 海 
                    4  // 枫树林 (最高优先级)
                };
                return priorities[turfType];
            }
            // 获取地皮类型 (通过RGB通道)  用来识别地皮
            fixed4 GetTurfType(fixed4 pixel,v2f i) { 
                float2 overlayUV = i.worldPos * _Scale;
                // float2 overlayUV=i.uv;
                
                // if (pixel.a = 0 ) return  tex2D(_MainTex, overlayUV) ; 
                if (pixel.r < 0.1 && pixel.g < 0.1 && pixel.b < 0.1)  return tex2D(_MainTex, overlayUV) ; 
                if (pixel.r > 0.5 & pixel.g > 0.5 & pixel.b > 0.5) return tex2D(_NoiseTex, overlayUV);

                if (pixel.r < 0.5 & pixel.g > 0.5 & pixel.b < 0.5) return tex2D(_GrassTex, overlayUV);
                
                if (pixel.r > 0.5 & pixel.g > 0.5 & pixel.b < 0.5) return tex2D(_StoneTex, overlayUV);
                if (pixel.r < 0.5 & pixel.g < 0.5 & pixel.b > 0.5) return tex2D(_SeaTex, overlayUV);

                if (pixel.r > 0.5 & pixel.g < 0.5 & pixel.b < 0.5) return tex2D(_MapleTex, overlayUV);
                
 
              
               
               
                return tex2D(_MainTex, overlayUV);
             }
 
             fixed4 GetOverlayTexture(fixed4 pixel,v2f i) { 
                float2 overlayUV = i.worldPos * _Scale;
                // float2 overlayUV=i.uv;
                
                // if (pixel.a = 0 ) return  tex2D(_MainTex, overlayUV) ; 

                // if (pixel.r < 0.1 && pixel.g < 0.1 && pixel.b < 0.1)  return tex2D(_MainTex, overlayUV) ; 
                // if (pixel.r > 0.5 & pixel.g > 0.5 & pixel.b > 0.5) return tex2D(_NoiseTex, overlayUV);

                // if (pixel.r < 0.5 & pixel.g > 0.5 & pixel.b < 0.5) return tex2D(_GrassTex, overlayUV);
                
                // if (pixel.r > 0.5 & pixel.g > 0.5 & pixel.b < 0.5) return tex2D(_StoneTex, overlayUV);
                // if (pixel.r < 0.5 & pixel.g < 0.5 & pixel.b > 0.5) return tex2D(_SeaTex, overlayUV);

                if (pixel.r > 0.8 & pixel.g > 0.8 & pixel.b > 0.8) return tex2D(_StoneTex, overlayUV); // 石头
                // if (pixel.r < 0.2 & pixel.g > 0.2 & pixel.b > 0.5) return tex2D(_MainTex, overlayUV); // 水
                
 
              
               
               
                return tex2D(_NoiseTex, overlayUV);
             }
            
            fixed4 frag (v2f i) : SV_Target
            {
                 float2 overlayUV = i.worldPos * _Scale;
                 overlayUV=i.uv;
                // 原始地皮混合逻辑（保持不变）
                fixed4 baseColor = tex2D(_MainTex, overlayUV);
                
                // baseColor.rgb = i.color.rgb;

                 
                fixed4 newColor = GetTurfType(baseColor,i);

                // 盖上世界纹理
            //      float2 worldUv = i.worldPos * _Scale;
            //    fixed4 overlayColor=  tex2D(_NoiseTex, worldUv);
            //     if (pixel.r > 0.8 & pixel.g > 0.8 & pixel.b > 0.8) overlayColor= tex2D(_StoneTex, worldUv);

             

               if (baseColor.r < 0.2 & baseColor.g > 0.2 & baseColor.b > 0.5) {
                // 水，不需要大纹理  但是需要波纹
                    //  float time = _Time.y * 0.2f;

                    // // 波纹扰动 (正弦函数)
                    // float waveX = sin(i.uv.y * 14.0f + time) * 0.01f;
                    // float waveY = cos(i.uv.x * 14.0f + time) * 0.01f;

                    // float2 uvRipple = i.uv + float2(waveX, waveY);

                    // // // 采样水面纹理
                    // // fixed4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uvRipple);
                    // float4 col = tex2D(_MainTex,uvRipple);
                    return baseColor;
               }
                
               // 给草，铺上大纹理
               fixed4 overlayColor= GetOverlayTexture(baseColor,i);

                fixed3 finalColor = lerp(baseColor.rgb, overlayColor.rgb, overlayColor.a);
              fixed4 fcolor=  fixed4(finalColor, 1.0f);
              return fcolor;
            }
            ENDCG
        }
    }
}