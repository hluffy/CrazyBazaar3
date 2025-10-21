Shader "Custom/MyTerrainShader"
{
    Properties
    {
        _DebugMode("Debug Mode", Range(0,2)) = 0

        _Control ("Control (RGBA)", 2D) = "white" {}   // 底图，用来控制混合
        _TexR ("Grass Texture (R)", 2D) = "white" {}
        _TexG ("Stone Texture (G)", 2D) = "white" {}
        _TexB ("Sea Texture (B)", 2D) = "white" {}
        _TexA ("Maple Texture (A)", 2D) = "white" {}
        _Scale ("UV Scale", Float) = 1.0
    }

    SubShader
    {
        Tags 
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
            "Queue"="Geometry"
            "TerrainCompatible"="True"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _Control;
            sampler2D _TexR, _TexG, _TexB, _TexA;
            float _Scale;
            int _DebugMode;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.uv = worldPos.xz * _Scale;   // ✅ 用地形的世界坐标作为UV
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 control = tex2D(_Control, i.uv);

                // 调试模式：直接返回control的颜色
                if (_DebugMode == 1) return control;

                // 调试模式：显示R通道纹理
                if (_DebugMode == 2) return tex2D(_TexR, i.uv);

                // 默认混合模式
                float total = control.r + control.g + control.b + control.a + 1e-6;
                control /= total;

                return tex2D(_TexR, i.uv) * control.r
                    + tex2D(_TexG, i.uv) * control.g
                    + tex2D(_TexB, i.uv) * control.b
                    + tex2D(_TexA, i.uv) * control.a;
            }
            ENDCG
        }
    }
}

