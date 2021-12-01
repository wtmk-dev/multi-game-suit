Shader "Unlit/BugCrawlShader"
{
    Properties
    {
        _ColorA("Color A", Color) = (1,1,1,1)
        _ColorB("Color B", Color) = (1,1,1,1)
        _ColorStart("Color Start", Range(0,1)) = 0
        _ColorEnd("Color End", Range(0,1)) = 1
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag          

            #include "UnityCG.cginc"

            #define TAU 6.28318530718

            float4 _ColorA;
            float4 _ColorB;
            float _ColorStart;
            float _ColorEnd;

            struct MeshData
            {
                float3 normal : NORMAL;
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float3 normal : TEXCOORD1;
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.uv = v.uv; //TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(Interpolators i) : SV_Target
            {
                // sample the texture
                float xOffset = cos(i.uv.x * TAU * 32) * 0.06;
                float t = cos((i.uv.y + xOffset + _Time.y) * 1.73205080757 * 1) * 0.5 + 0.5;


                float4 graident = lerp(_ColorA, _ColorB, i.uv.y) * t;

                return graident;
            }
            ENDCG
        }
    }
}
