// 添加了 id mask 
Shader "Custom/Mytile4"
{
    // 通过 csv 传递当前和邻居
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise", 2D) = "gray" {} 
        
        _Scale("Scale", Float) = 0.25 // Scale factor

        // _GrassTex("Grass Texture", 2D) = "white" {} 
        // _StoneTex("Stone Texture", 2D) = "white" {} 
        // _SeaTex("Sea Texture", 2D) = "white" {} 
        // _MapleTex("Maple Texture", 2D) = "white" {} 


        _NibaAtlasTex ("Niba Atlas Texture", 2D) = "white" {}  // 基础图集  带边缘
        _GrassAtlasTex ("Grass Atlas Texture", 2D) = "white" {}  // 基础图集  带边缘
        _StoneAtlasTex ("Stone Atlas Texture", 2D) = "white" {}  // 基础图集  带边缘
        _MapleAtlasTex ("Maple Atlas Texture", 2D) = "white" {}  // 基础图集  带边缘
        _SeaAtlasTex ("Sea Atlas Texture", 2D) = "white" {}  // 基础图集  带边缘

        
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
            
 
                // Atlas 配置（固定大小）
            #define ATLAS_COLS 10
            #define ATLAS_ROWS 10
            #define TILE_SIZE 64.0
            #define ATLAS_SIZE 640.0

            sampler2D _MainTex, _NoiseTex, _MapleTex,_GrassTex,_StoneTex,_SeaTex;
            float  _Scale;
            
            //  添加了 idmask
            float _ChunkSize;
            sampler2D _NibaAtlasTex,_GrassAtlasTex,_StoneAtlasTex,_MapleAtlasTex,_SeaAtlasTex;

            
            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0;  fixed4 color : COLOR;};
            struct v2f { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; float2 worldPos : TEXCOORD1;fixed4 color : COLOR;};
           
              // 绑定的 Buffer
            StructuredBuffer<int> _TileIdBuffer0;
            float4 _ChunkOrigin0; // x=chunkX, y=chunkY

            int _MapWidth;
            int _MapHeight;

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
            
         
            int GetValueByKey(int key)
            {
                if (key == 0000) return 0;   
                if (key == 1000) return 1;   
                if (key == 0100) return 2;   
                if (key == 0010) return 3;   
                if (key == 0001) return 4;   
                if (key == 1100) return 5;   
                if (key == 0110) return 6;   
                if (key == 0011) return 7;   
                if (key == 1001) return 8;   
                if (key == 1010) return 9;   
                if (key == 0101) return 10;   

                if (key == 1110) return 12;   
                if (key == 1110) return 13;   
                if (key == 1110) return 14;   
                if (key == 1110) return 11;    // 这里是11 没错
                if (key == 1110) return 15;   
              
                return 0;
            }
                // 从边缘图集中，寻找某一个小图 index = 22，找第22个小图 ，0，0.1，0.2，0.3，0.4，0.5 ，0.1代表第二列，
              fixed4 GetAtlas(v2f i,int index,int tileId){

                         // 计算小图在 Atlas 中的行列

                float2 tileUV = i.uv; // 假设在 [0,1]

                // 第一张小图在 Atlas 左下角，UV范围 [0, TILE_SIZE/ATLAS_SIZE]
                float2 uvOffset;
                float uvScale = 0.1;  // 小图 64*64 ，大图 640*640， 所以scale= 64 / 640 缩放
                
                // 下面是值 第三列，第五行  52
                // uvOffset.x = 0.2;                   // 左边起点
                // uvOffset.y = 1-0.6;       // 左上角 Y 起点，因为读取的时候，是从大图的左下角开始读
                // 52  --- > (0.2,0.4)
                uvOffset.x = (index%10)*uvScale;                   // 左边起点
                uvOffset.y = 1-(index/10+1)*uvScale;       // 左上角 Y 起点，因为读取的时候，是从大图的左下角开始读

                // 计算 Atlas UV
                float2 atlasUV = uvOffset+ tileUV * uvScale;


                if(tileId==0) return tex2D(_NibaAtlasTex, atlasUV);
                if(tileId==1) return tex2D(_GrassAtlasTex, atlasUV);
                if(tileId==2) return tex2D(_StoneAtlasTex, atlasUV);
                if(tileId==3) return tex2D(_MapleAtlasTex, atlasUV);
                if(tileId==4) return tex2D(_SeaAtlasTex, atlasUV);
              
                return fixed4(0, 0, 0, 1); 
              }
                int GetTileId(StructuredBuffer<int> buf, float2 uv, float2 origin)
            {
                int localX = (int)(uv.x * _ChunkSize);
                int localY = (int)(uv.y * _ChunkSize);
                int idx = localY * _ChunkSize + localX;
                return buf[idx];
            }
        
            fixed4 frag (v2f i) : SV_Target
            {
             
               float2 overlayUV=i.uv;
                // 原始地皮混合逻辑（保持不变）
                fixed4 baseColor = tex2D(_MainTex, overlayUV);

                 int id = GetTileId(_TileIdBuffer0, i.uv, _ChunkOrigin0.xy);

                return baseColor;

       

            }
            ENDCG
        }
    }
}