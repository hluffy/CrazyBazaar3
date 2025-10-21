Shader "Custom/ExplosionEffect"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1,1,1,1)
        _EdgeColor("Edge Color", Color) = (1,0.5,0,1)
        _ExplosionStrength("Explosion Strength", Range(0, 2)) = 0
        _DissolveThreshold("Dissolve Threshold", Range(0,1)) = 0
        _EdgeWidth("Edge Width", Range(0, 0.5)) = 0.1
        _NoiseTex("Noise Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _NoiseTex;
            float4 _BaseColor;
            float4 _EdgeColor;
            float _ExplosionStrength;
            float _DissolveThreshold;
            float _EdgeWidth;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float noise : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                // 使用噪声控制爆炸形变
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float noise = tex2Dlod(_NoiseTex, float4(v.uv, 0, 0)).r;
                float3 offset = v.normal * (noise * _ExplosionStrength);
                v.vertex.xyz += offset;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.noise = noise;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float noiseVal = tex2D(_NoiseTex, i.uv * 2).r;

                // 溶解计算
                float dissolve = noiseVal - _DissolveThreshold;
                if (dissolve < 0) discard;

                // 边缘发光
                float edge = smoothstep(0, _EdgeWidth, dissolve);
                float3 color = lerp(_EdgeColor.rgb, _BaseColor.rgb, edge);

                return float4(color, edge);
            }
            ENDCG
        }
    }
}
